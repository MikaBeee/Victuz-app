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
                .Where(g => g.IsSuggested == false)
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
                .Include(g => g.Votes)
                .FirstOrDefaultAsync(m => m.GatheringId == id);

            if (gathering == null)
            {
                return NotFound();
            }

            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)); // Assuming you're using Claims for logged-in user
            
            var isUserRegistered = gathering.GatheringRegistrations
                .Any(gr => gr.UserId == userId); // Check if the user is registered for this gathering

            var existingVote = await _context.votes
                .FirstOrDefaultAsync(v => v.GatheringId == id && v.UserId == userId); // Check if the user has voted for this gathering

            if (existingVote == null)
            {
                ViewData["HasVoted"] = false; // User hasn't voted
            }
            else
            {
                ViewData["HasVoted"] = true; // User has voted
            }



            // Pass the registration status to the view
            ViewData["IsUserRegistered"] = isUserRegistered;

            return View(gathering);
        }

        // GET: Gathering/AlLGatherings     //Possible for anyone
        public async Task<IActionResult> AllGatherings()
        {
            var gatherings = await _context.gathering
                .Where(g => g.IsSuggested == false)
                .Include(g => g.Category)
                .Include(g => g.Location)
                .ToListAsync();

            return View(gatherings);
        }

        // GET: Gathering/IsSugested=true     //View specific to the administrator
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> SuggestiesIndex()
        {
            var gatherings = await _context.gathering
                .Where(g => g.IsSuggested == true)
                .Where(g => g.Votes.Count > 3)
                .Include(g => g.Category)
                .Include(g => g.Location)
                .Include(g => g.Votes)
                .ToListAsync();

            return View(gatherings);
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
        public async Task<IActionResult> Create([Bind("GatheringId,GatheringTitle,GatheringDescription,MaxParticipants,Date,LocationId,CategoryId,IsSuggested")] Gathering gathering, IFormFile Photo)

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

        [Authorize]
        public IActionResult CreateSuggestion()
        {
            ViewData["CategoryId"] = new SelectList(_context.categorie, "CatId", "CatName");
            ViewData["LocationId"] = new SelectList(_context.location, "LocId", "LocName");
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSuggestion([Bind("GatheringId,GatheringTitle,GatheringDescription,MaxParticipants,Date,LocationId,CategoryId")] Gathering gathering, IFormFile Photo)
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

                gathering.IsSuggested = true;

                _context.Add(gathering);
                await _context.SaveChangesAsync();
                return RedirectToAction("SuggestedActivity");
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

            ViewBag.IsSuggested = new List<SelectListItem>
            {
                new SelectListItem { Text = "Ja", Value = "true", Selected = Convert.ToBoolean(gathering.IsSuggested) },
                new SelectListItem { Text = "Nee", Value = "false", Selected = Convert.ToBoolean(!gathering.IsSuggested) }
            };

            return View(gathering);
        }

        // POST: Gatherings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GatheringId,GatheringTitle,GatheringDescription,MaxParticipants,Date,LocationId,CategoryId,IsSuggested")] Gathering gathering, IFormFile? Photo)
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
            ViewBag.IsSuggested = new List<SelectListItem>
            {
                new SelectListItem { Text = "Ja", Value = "true", Selected = Convert.ToBoolean(gathering.IsSuggested) },
                new SelectListItem { Text = "Nee", Value = "false", Selected = Convert.ToBoolean(!gathering.IsSuggested) }
            };
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VoteGathering(int gatheringId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Controleer of de gebruiker al heeft gestemd
            bool alreadyVoted = await _context.votes
                .AnyAsync(v => v.GatheringId == gatheringId && v.UserId == userId);

            if (alreadyVoted)
            {
                TempData["ErrorMessage"] = "U heeft al gestemd voor deze activiteit.";
                return RedirectToAction("Details", new { id = gatheringId });
            }

            // Voeg een nieuwe stem toe
            var vote = new Vote
            {
                GatheringId = gatheringId,
                UserId = userId
            };

            _context.votes.Add(vote);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Uw stem is geregistreerd!";
            return RedirectToAction("Details", new { id = gatheringId });
        }

        public async Task<IActionResult> SuggestedActivity()
        {
            var suggestedGatherings = await _context.gathering
                .Where(g => g.IsSuggested == true)
                .Include(g => g.Category)
                .Include(g => g.Location)
                .ToListAsync();

            return View(suggestedGatherings);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AccepteerSuggestie(int gatheringId)
        {
            var gathering = await _context.gathering
                .FirstOrDefaultAsync(g => g.GatheringId == gatheringId);

            if (gathering == null)
            {
                return NotFound();
            }

            gathering.IsSuggested = false;

            _context.Update(gathering);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(SuggestiesIndex));
        }
    }
}
