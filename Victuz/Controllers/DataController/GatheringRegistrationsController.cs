using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Victuz.Data;
using Victuz.Models.Businesslayer;
using Victuz.Models.Viewmodels;

namespace Victuz.Controllers.DataController
{
    public class GatheringRegistrationsController : Controller
    {
        private readonly VictuzDB _context;

        public GatheringRegistrationsController(VictuzDB context)
        {
            _context = context;
        }

        // GET: GatheringRegistrations
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index()
        {
            var victuzDB = _context.gatheringRegistration.Include(g => g.Gathering).Include(g => g.User);
            return View(await victuzDB.ToListAsync());
        }

        // GET: GatheringRegistrations/Details/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Details(int? userid, int? gatheringid)
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
        [Authorize]
        public IActionResult Create(int Id)
        {
            ViewData["GatheringId"] = new SelectList(_context.gathering, "GatheringId", "GatheringTitle");
            var registration = new GatheringRegistration
            {
                GatheringId = Id
            };

            return View();
        }

        // POST: GatheringRegistrations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,RegistrationDate")] int Id, GatheringRegistration gatheringRegistration)
        {
            if (ModelState.IsValid)
            {
                _context
                    .Add(gatheringRegistration);
                await _context
                    .SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["UserId"] = new SelectList(_context.users, "UserId", "Password", gatheringRegistration.UserId);
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
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gatheringRegistration = await _context.gatheringRegistration.FindAsync(id);
            if (gatheringRegistration != null)
            {
                _context.gatheringRegistration.Remove(gatheringRegistration);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GatheringRegistrationExists(int id)
        {
            return _context.gatheringRegistration.Any(e => e.UserId == id);
        }
    }
}
