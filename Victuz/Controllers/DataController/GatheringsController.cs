using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Victuz.Data;
using Victuz.Migrations;
using Victuz.Models.Businesslayer;
using Victuz.Models.Viewmodels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Victuz.Controllers.DataController
{
    public class GatheringsController : Controller
    {
        private readonly VictuzDB _context;

        public GatheringsController(VictuzDB context)
        {
            _context = context;
        }

        // GET: Gatherings      //View specific to the administrator
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index()
        {
            var victuzDB = _context.gathering
                .Include(g => g.Category)
                .Include(g => g.Location);
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
                .Include(g => g.GatheringRegistrations)
                .FirstOrDefaultAsync(m => m.GatheringId == id);
            if (gathering == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Assuming you're using Claims for logged-in user
            var isUserRegistered = gathering.GatheringRegistrations
                .Any(gr => gr.UserId.ToString() == userId); // Check if the user is registered for this gathering

            // Pass the registration status to the view
            ViewData["IsUserRegistered"] = isUserRegistered;

            return View(gathering);
        }

        // GET: Gathering/AlLGatherings     //Possible for anyone
        public async Task<IActionResult> AllGatherings()
        {
            var victuzDB = _context.gathering
                .Include(g => g.Category)
                .Include(g => g.Location);
            return View(await victuzDB.ToListAsync());
        }


        public async Task<IActionResult> MyEvents()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // This is where the UserId is usually stored

            // Set the UserId value into ViewData to be used in the view
            ViewData["UserId"] = userId;

            var registeredGatherings = _context.gathering
                .Include(g => g.Category)
                .Include(g => g.Location)
                .Where(g => g.GatheringRegistrations.Any(gr => gr.UserId == Convert.ToInt32(userId)))
                .ToListAsync();

            return View(await registeredGatherings);
        }

        // GET: Gatherings/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.categorie, "CatId", "CatName");
            ViewData["LocationId"] = new SelectList(_context.location, "LocId", "LocName");
            return View();
        }

        // POST: Gatherings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GatheringId,GatheringTitle,GatheringDescription,MaxParticipants,Date,LocationId,CategoryId")] Gathering gathering, IFormFile Photo)

        {
            ViewData["CategoryId"] = new SelectList(_context.categorie, "CatId", "CatName", gathering.CategoryId);
            ViewData["LocationId"] = new SelectList(_context.location, "LocId", "LocName", gathering.LocationId);
            if (ModelState.IsValid)
            {
                if (Photo != null && Photo.Length > 0)
                {

                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }


                    var fileName = Path.GetFileName(Photo.FileName);


                    var filePath = Path.Combine(uploadsFolder, fileName);


                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Photo.CopyToAsync(stream);
                    }
                    gathering.Photopath = "/images/" + fileName;

                }
                _context.Add(gathering);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));



            }
            return View(gathering);
        }

        // GET: Gatherings/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gathering = await _context.gathering
                .FindAsync(id);
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
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GatheringId,GatheringTitle,GatheringDescription,MaxParticipants,Date,LocationId,CategoryId")] Gathering gathering, IFormFile? Photo)
        {
            if (id != gathering.GatheringId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var originalGathering = await _context.gathering.AsNoTracking().FirstOrDefaultAsync(g => g.GatheringId == id);
                    if (originalGathering == null)
                    {
                        return NotFound();
                    }
                    if (Photo == null)
                    {

                        gathering.Photopath = originalGathering.Photopath;
                    }
                    else
                    {
                        string deletethis = "wwwroot" + originalGathering.Photopath;
                        try
                        {
                            System.IO.File.Delete(deletethis);
                        }
                        catch (IOException ex)
                        {
                            Console.WriteLine($"Error deleting file: {ex.Message}");
                        }

                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }


                        var fileName = Path.GetFileName(Photo.FileName);


                        var filePath = Path.Combine(uploadsFolder, fileName);


                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await Photo.CopyToAsync(stream);
                        }

                        gathering.Photopath = "/images/" + fileName;
                    }
                        _context.Entry(gathering).State = EntityState.Modified;
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
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var originalGathering = await _context.gathering.AsNoTracking().FirstOrDefaultAsync(g => g.GatheringId == id);
            if (originalGathering == null)
            {
                return NotFound();
            }
            string deletethis = "wwwroot" + originalGathering.Photopath;
            try
            {
                System.IO.File.Delete(deletethis);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error deleting file: {ex.Message}");
            }
            var gathering = await _context.gathering.FindAsync(id);
            if (gathering != null)
            {
                _context.gathering.Remove(gathering);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        //Get: Gatheringparticipants
        [Authorize(Roles = "admin")]

        private bool GatheringExists(int id)
        {
            return _context.gathering.Any(e => e.GatheringId == id);
        }
    }
}
