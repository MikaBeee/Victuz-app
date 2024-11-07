using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Victuz.Data;
using Victuz.Models.Businesslayer;
using Victuz.Models.Viewmodels;

namespace Victuz.Controllers.DataController
{

    public class UsersController : Controller
    {
        private readonly VictuzDB _context;
        private readonly PasswordHasher<User> _passwordHasher;

        public UsersController(VictuzDB context, ILogger<UsersController> logger)
        {

            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var victuzDB = _context.users.Include(u => u.Role);
            return View(await victuzDB.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public async Task<List<UserVM>> AllUsers()
        {
            var victuzDB = _context.users
                .Include(u => u.Role)
                .Select(u => new UserVM
                {
                    UserId = u.UserId,
                    UserName = u.UserName,
                    Password = u.Password,
                    RoleId = u.RoleId,
                    Role = u.Role
                })
                .ToListAsync();

            return await victuzDB;
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.role, "RoleId", "RoleName");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,UserName,Password,RoleId")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.role, "RoleId", "RoleName", user.RoleId);
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.role, "RoleId", "RoleName", user.RoleId);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,UserName,Password,RoleId")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.role, "RoleId", "RoleName", user.RoleId);
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.users.FindAsync(id);
            if (user != null)
            {
                _context.users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Login()
        {
            return View();
        }

        // POST: Login a user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.users
                    .Include(u => u.Role)
                    .SingleOrDefaultAsync(u => u.UserName == model.UserName);

                if (user != null)
                {

                  

                    var result = _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);

                   
                    if (result == PasswordVerificationResult.Success)
                    {
                        var roleName = user.Role?.RoleName;

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, model.UserName),
                            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                            new Claim(ClaimTypes.Role, roleName ?? ""),
                            
                            

                        };



                        var claimsIdentity = new ClaimsIdentity(claims, "Cookie");

                        await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(claimsIdentity));

                        return RedirectToAction("Index", "Home");

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }
            return View(model);
        }


        public IActionResult AccountReg()
        {
            ViewData["RoleId"] = new SelectList(_context.role, "RoleId", "RoleName");
            return View();
        }


        //Post Register a new user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AccountReg([Bind("UserId, UserName, Password, RoleId, ConfirmPassword")] RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {
                // Check if the username already exists
                var existingUser = await _context.users
                    .FirstOrDefaultAsync(u => u.UserName == registerVM.UserName);

                if (existingUser != null)
                {
                    // Add a model state error if the username is taken
                    ModelState.AddModelError("UserName", "Username is already taken.");
                }
                else
                {
                    // Create a new User object from the view model
                    var user = new User
                    {
                        UserName = registerVM.UserName,
                        Password = _passwordHasher.HashPassword(new User(), registerVM.Password), // Hash the password
                        RoleId = registerVM.RoleId
                    };

                    // Add user to the database
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            // If we got this far, something failed; redisplay the form with the same model
            ViewData["RoleId"] = new SelectList(_context.role, "RoleId", "RoleName", registerVM.RoleId);
            return View(registerVM); // Pass the RegisterVM back to the view
        }

        //LOGOUT
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("Index", "Home"); // Redirect to home or login page after logout
        }

        private bool UserExists(int id)
        {
            return _context.users.Any(e => e.UserId == id);
        }
    }
}
