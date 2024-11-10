using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Victuz.Data;
using Victuz.Models.Businesslayer;
using Victuz.Models.Viewmodels;
using Victuz.Methods;

namespace Victuz.Controllers.DataController
{
    public class GatheringRegistrationsController : Controller
    {
        private readonly VictuzDB _context;

        public GatheringRegistrationsController(VictuzDB context)
        {
            _context = context;
        }

        // GET: TicketQR
        public IActionResult GetTicketImage(string guid)
        {

            var referrer = Request.Headers["Referer"].ToString();
            if (!(referrer.Contains("/GatheringRegistrations/Details") || referrer.Contains("/Home/TicketScanner")))
            {
                return RedirectToAction("Error", "Home", new { statusCode = 403 });
            }
            var image = ImageCache.GetOrAdd(guid, QrGeneration.GenerateQrFromGuid);
            return File(image, "image/png");
        }


        // GET: GatheringRegistrations/Details/5
        public async Task<IActionResult> Details(int? userid, int? gatheringid)
        {
            int checkid = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            // check if the users role is admin if so he can view any tickets available
            if (User.IsInRole("admin"))
            {
                goto actionpermitted;
            }
            // Check if the current user is authorized to view the details
            if (userid != checkid)
            {
                return RedirectToAction("Error", "Home", new { statusCode = 403 });
            }

            actionpermitted:
            if (userid == null || gatheringid == null)
            {
                return NotFound();
            }

            var gatheringRegistration = await _context.gatheringRegistration
                .Include(g => g.Gathering)
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.UserId == userid && m.GatheringId == gatheringid);

            if (gatheringRegistration == null)
            {
                return NotFound();
            }

            return View(gatheringRegistration);
        }


        [Authorize(Roles = "admin")]
        public async Task<List<GatheringRegistrationVM>> AllGatheringRegistration()
        {
            var victuzDB = _context.gatheringRegistration
                .Select(g => new GatheringRegistrationVM
                {
                    UserId = g.UserId,
                    User = g.User,
                    GatheringId = g.GatheringId,
                    Gathering = g.Gathering
                })
                .ToListAsync();

            return await victuzDB;
        }

        // GET: GatheringRegistrations/Create
        public IActionResult Create(int Id)
        {
            ViewData["GatheringId"] = new SelectList(_context.gathering, "GatheringId", "GatheringTitle");

            bool isLoggedIn = User.Identity.IsAuthenticated;
            if(isLoggedIn)
            {
                ViewData["UserID"] = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            else
            {
                ViewData["UserID"] = null;
            }

            ViewData["IsLoggedIn"] = isLoggedIn;

            var registration = new GatheringRegistration
            {
                GatheringId = Id,
                RegistrationDate = DateTime.Now
            };

            return View(registration);
        }

        // POST: GatheringRegistrations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, string name, [Bind("RegistrationDate")] GatheringRegistration gatheringRegistration)
        {
            string redirectToCon;
            string redirectToPage;

            gatheringRegistration.RegistrationDate = DateTime.Now;
            gatheringRegistration.GatheringId = id;

            if (User.Identity.IsAuthenticated)
            {
                gatheringRegistration.UserId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                redirectToCon = "Gatherings";
                redirectToPage = "MyEvents";
            }
            else
            {
                var newUser = new User
                {
                    UserName = name,
                    Password = "defaultPassword" + name,
                    RoleId = 3
                };

                _context.users.Add(newUser);
                await _context.SaveChangesAsync();

                gatheringRegistration.UserId = newUser.UserId;

                redirectToCon = "Home";
                redirectToPage = "Index";
            }

            // Check if the gathering exists and has tickets left
            var gathering = await _context.gathering
                .Include(g =>  g.GatheringRegistrations)
                .FirstOrDefaultAsync(g => g.GatheringId == id);

            if (gathering == null)
            {
                return NotFound(); // Gathering not found
            }

            int ticketsleft = gathering.MaxParticipants - (gathering.GatheringRegistrations?.Count ?? 0);

            if (ticketsleft < 1)
            {
                TempData["ErrorMessage"] = "Er zijn geen tickets meer beschikbaar voor deze activiteit.";
                return RedirectToAction("Create", new { gatheringRegistration.GatheringId });
            }

            // Check if the user is already registered
            bool ticketExists = await _context.gatheringRegistration
                .AnyAsync(gr => gr.UserId == gatheringRegistration.UserId && gr.GatheringId == gatheringRegistration.GatheringId);

            if (ticketExists)
            {
                TempData["ErrorMessage"] = "U heeft al een ticket voor deze activiteit";
                return RedirectToAction("Create", new { gatheringRegistration.GatheringId });
            }

            // Proceed with registration
            if (ModelState.IsValid)
            {
                _context.Add(gatheringRegistration);

                await _context.SaveChangesAsync();

                return RedirectToAction(redirectToPage, redirectToCon);
            }

            ViewData["GatheringId"] = new SelectList(_context.gathering, "GatheringId", "GatheringTitle", gatheringRegistration.GatheringId);
            return View(gatheringRegistration);
        }

        // GET: GatheringRegistrations/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? userid, int? gatheringid)
        {
            if (userid == null || gatheringid == null)
            {
                return NotFound();
            }

            var gatheringRegistration = await _context.gatheringRegistration.FirstOrDefaultAsync(m => m.UserId == userid && m.GatheringId == gatheringid); ;
            if (gatheringRegistration == null)
            {
                return NotFound();
            }
            ViewData["GatheringId"] = new SelectList(_context.gathering, "GatheringId", "GatheringTitle", gatheringRegistration.GatheringId);
            ViewData["UserId"] = new SelectList(_context.users, "UserId", "Password", gatheringRegistration.UserId);
            return View(gatheringRegistration);
        }

        // POST: GatheringRegistrations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,GatheringId,RegistrationDate")] GatheringRegistration gatheringRegistration)
        {
            if (id != gatheringRegistration.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gatheringRegistration);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GatheringRegistrationExists(gatheringRegistration.UserId))
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
            ViewData["GatheringId"] = new SelectList(_context.gathering, "GatheringId", "GatheringTitle", gatheringRegistration.GatheringId);
            ViewData["UserId"] = new SelectList(_context.users, "UserId", "Password", gatheringRegistration.UserId);
            return View(gatheringRegistration);
        }

        // GET: GatheringRegistrations/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? userid, int? gatheringid)
        {
            if (userid == null || gatheringid == null)
            {
                return NotFound();
            }

            var gatheringRegistration = await _context.gatheringRegistration
                .Include(g => g.Gathering)
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.UserId == userid && m.GatheringId == gatheringid);
            if (gatheringRegistration == null)
            {
                return NotFound();
            }

            return View(gatheringRegistration);
        }

        // POST: GatheringRegistrations/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int userid, int? gatheringid)
        {
            var gatheringRegistration = await _context.gatheringRegistration
                .FindAsync(userid, gatheringid); // Pass both key values
            if (gatheringRegistration != null)
            {
                _context.gatheringRegistration.Remove(gatheringRegistration);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
        //Get: All participants of id
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GatheringParticipants(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gathering = await _context.gathering
                .FirstOrDefaultAsync(g => g.GatheringId == id);

            if (gathering == null)
            {
                return NotFound();
            }

            var gatheringRegistration = await _context.gatheringRegistration
                .Include(g => g.Gathering)
                .Include(g => g.User)
                .Where(g => g.GatheringId == id)
                .ToListAsync();

            var gatheringregvm = gatheringRegistration
                .Select(g => new GatheringRegistrationVM
                {
                    Gathering = g.Gathering,
                    User = g.User
                }).ToList();

            ViewData["GatheringTitle"] = gathering.GatheringTitle;

            return View(gatheringregvm);
        }


        private bool GatheringRegistrationExists(int id)
        {
            return _context.gatheringRegistration.Any(e => e.UserId == id);
        }
    }
}
