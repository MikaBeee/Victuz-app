using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Victuz.Data;
using Victuz.Models.Businesslayer;
using Victuz.Models.Viewmodels;

namespace Victuz.Controllers
{
    public class GatheringsController : Controller
    {
        private readonly VictuzDB _context;

        public GatheringsController(VictuzDB context)
        {
            _context = context;
        }

        // GET: Gatherings      //View specific to the administrator
        public async Task<IActionResult> Index()
        {
            var victuzDB = _context.gathering.Include(g => g.Category).Include(g => g.Location);
            return View(await victuzDB.ToListAsync());
        }

        // GET: Gatherings/Details/5    //Possible for anyone
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gathering = await _context.gathering
                .Include(g => g.Category)
                .Include(g => g.Location)
                .FirstOrDefaultAsync(m => m.GatheringId == id);
            if (gathering == null)
            {
                return NotFound();
            }

            return View(gathering);
        }

        // GET: Gathering/AlLGatherings     //Possible for anyone
        public async Task<List<GatheringVM>> AllGatherings()
        {
            var victuzDB = _context.gathering
                .Include(g => g.Category)
                .Include(g => g.Location)
                .Select(g => new GatheringVM
                {
                    GatheringId = g.GatheringId,
                    GatheringTitle = g.GatheringTitle,
                    GatheringDescription = g.GatheringDescription,
                    MaxParticipants = g.MaxParticipants,
                    Category = g.Category,
                    CategoryId = g.CategoryId,
                    Location = g.Location,
                    LocationId = g.LocationId,              
                })
                .ToListAsync();

            return await victuzDB;    

        }

        // GET: Gatherings/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.categorie, "CatId", "CatName");
            ViewData["LocationId"] = new SelectList(_context.location, "LocId", "LocName");
            return View();
        }

        // POST: Gatherings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GatheringId,GatheringTitle,GatheringDescription,MaxParticipants,Date,LocationId,CategoryId")] Gathering gathering)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gathering);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.categorie, "CatId", "CatName", gathering.CategoryId);
            ViewData["LocationId"] = new SelectList(_context.location, "LocId", "LocName", gathering.LocationId);
            return View(gathering);
        }

        // GET: Gatherings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gathering = await _context.gathering.FindAsync(id);
            if (gathering == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.categorie, "CatId", "CatName", gathering.CategoryId);
            ViewData["LocationId"] = new SelectList(_context.location, "LocId", "LocName", gathering.LocationId);
            return View(gathering);
        }

        // POST: Gatherings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GatheringId,GatheringTitle,GatheringDescription,MaxParticipants,Date,LocationId,CategoryId")] Gathering gathering)
        {
            if (id != gathering.GatheringId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gathering);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GatheringExists(gathering.GatheringId))
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
            ViewData["CategoryId"] = new SelectList(_context.categorie, "CatId", "CatName", gathering.CategoryId);
            ViewData["LocationId"] = new SelectList(_context.location, "LocId", "LocName", gathering.LocationId);
            return View(gathering);
        }

        // GET: Gatherings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gathering = await _context.gathering
                .Include(g => g.Category)
                .Include(g => g.Location)
                .FirstOrDefaultAsync(m => m.GatheringId == id);
            if (gathering == null)
            {
                return NotFound();
            }

            return View(gathering);
        }

        // POST: Gatherings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gathering = await _context.gathering.FindAsync(id);
            if (gathering != null)
            {
                _context.gathering.Remove(gathering);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GatheringExists(int id)
        {
            return _context.gathering.Any(e => e.GatheringId == id);
        }
    }
}
