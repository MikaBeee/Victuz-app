﻿using System;
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
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
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
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index()
        {
            var victuzDB = _context.users.Include(u => u.Role);
            return View(await victuzDB.ToListAsync());
        }

        // GET: Users/Details/5
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            var roles = _context.role // of filter op RoleId: r.RoleId != adminRoleId
                .Select(r => new { r.RoleId, r.RoleName })
                .ToList();

            ViewBag.RoleId = new SelectList(roles, "RoleId", "RoleName");

            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,UserName,Password,ConfirmPassword, RoleId")] RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {
                // Check if the username already exists
                var existingUser = await _context.users
                    .FirstOrDefaultAsync(u => u.UserName == registerVM.UserName);

                if (existingUser != null)
                {
                    // Add a model state error if the username is taken
                    ModelState.AddModelError("UserName", "Deze gebruikersnaam bestaat al.");
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
                    return RedirectToAction("Index", "Home");
                }
            }
            // If we got this far, something failed; redisplay the form with the same model
            ViewData["RoleId"] = new SelectList(_context.role, "RoleId", "RoleName", registerVM.RoleId);
            return View(registerVM); // Pass the RegisterVM back to the view
        }

        // GET: Users/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.users
                .FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            user.Password = string.Empty;

            ViewData["RoleId"] = new SelectList(_context.role, "RoleId", "RoleName", user.RoleId);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,UserName,Password,RoleId")] User userVM)
        {
            if (id != userVM.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Check if another user already has the same username
                var existingUser = await _context.users
                    .FirstOrDefaultAsync(u => u.UserName == userVM.UserName && u.UserId != userVM.UserId);

                if (existingUser != null)
                {
                    // Add a model state error if the username is taken by another user
                    ModelState.AddModelError("UserName", "Deze gebruikersnaam bestaat al.");
                }
                else
                {
                    try
                    {
                        // Retrieve the user to edit from the database
                        var user = await _context.users.FindAsync(userVM.UserId);
                        if (user == null)
                        {
                            return NotFound();
                        }

                        // Update the user's information
                        user.UserName = userVM.UserName;

                        // Only update the password if it was modified (assuming empty means no change)
                        if (!string.IsNullOrEmpty(userVM.Password))
                        {
                            user.Password = _passwordHasher.HashPassword(user, userVM.Password);
                        }

                        user.RoleId = userVM.RoleId;

                        // Mark the user entity as modified
                        _context.Update(user);
                        await _context.SaveChangesAsync();

                        return RedirectToAction("Index", "Home");
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!_context.users.Any(u => u.UserId == userVM.UserId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }

            // If we got this far, something failed; redisplay the form with the same model
            ViewData["RoleId"] = new SelectList(_context.role, "RoleId", "RoleName", userVM.RoleId);
            return View(userVM); // Pass the model back to the view with any validation errors
        }


        // GET: Users/Delete/5
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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
                            new Claim(ClaimTypes.Role, roleName ?? "")
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, "Cookie");

                        await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(claimsIdentity));

                        if(roleName == "admin")
                        {
                            return RedirectToAction("Dashboard", "Home");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Naam of wachtwoord is incorrect.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Naam of wachtwoord is incorrect.");
                }
            }
            return View(model);
        }


        public IActionResult AccountReg()
        {
            var roles = _context.role
                .Where(r => r.RoleName != "Admin") // of filter op RoleId: r.RoleId != adminRoleId
                .Where(r => r.RoleName != "guest")
                .Select(r => new { r.RoleId, r.RoleName })
                .ToList();

            ViewBag.RoleId = new SelectList(roles, "RoleId", "RoleName");
            
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
                    ModelState.AddModelError("UserName", "Deze gebruikersnaam bestaat al.");
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
                    return RedirectToAction("Index", "Home");
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
