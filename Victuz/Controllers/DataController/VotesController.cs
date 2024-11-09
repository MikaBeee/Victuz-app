using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class VotesController : Controller
    {
        private readonly VictuzDB _context;

        public VotesController(VictuzDB context)
        {
            _context = context;
        }

        // GET: Votes
        public async Task<IActionResult> Index()
        {
            var victuzDB = _context.votes.Include(v => v.Gathering).Include(v => v.User);
            return View(await victuzDB.ToListAsync());
        }

        // GET: Votes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vote = await _context.votes
                .Include(v => v.Gathering)
                .Include(v => v.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (vote == null)
            {
                return NotFound();
            }

            return View(vote);
        }

        // GET: Votes/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.gathering, "GatheringId", "GatheringTitle");
            ViewData["UserId"] = new SelectList(_context.users, "UserId", "Password");
            return View();
        }

        // POST: Votes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GatheringId,UserId")] Vote vote)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.gathering, "GatheringId", "GatheringTitle", vote.UserId);
            ViewData["UserId"] = new SelectList(_context.users, "UserId", "Password", vote.UserId);
            return View(vote);
        }

        // GET: Votes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vote = await _context.votes.FindAsync(id);
            if (vote == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.gathering, "GatheringId", "GatheringTitle", vote.UserId);
            ViewData["UserId"] = new SelectList(_context.users, "UserId", "Password", vote.UserId);
            return View(vote);
        }

        // POST: Votes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GatheringId,UserId")] Vote vote)
        {
            if (id != vote.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vote);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoteExists(vote.UserId))
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
            ViewData["UserId"] = new SelectList(_context.gathering, "GatheringId", "GatheringTitle", vote.UserId);
            ViewData["UserId"] = new SelectList(_context.users, "UserId", "Password", vote.UserId);
            return View(vote);
        }

        // GET: Votes/Delete/5
        public async Task<IActionResult> Delete(int? gatheringId)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (gatheringId == null)
            {
                return NotFound();
            }

            var vote = await _context.votes
                .Include(v => v.Gathering)
                .Include(v => v.User)
                .FirstOrDefaultAsync(v => v.UserId == userId && v.GatheringId == gatheringId);
            
            if (vote == null)
            {
                return NotFound();
            }

            return View(vote);
        }

        // POST: Votes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int gatheringId)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var vote = await _context.votes
                .FirstOrDefaultAsync(v => v.GatheringId == gatheringId && v.UserId == userId);

            if (vote != null)
            {
                _context.votes.Remove(vote);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Gatherings", new { id = gatheringId });
        }

        private bool VoteExists(int id)
        {
            return _context.votes.Any(e => e.UserId == id);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Vote(int gatheringId)
        {
            // Controleer of de gebruiker al heeft gestemd op deze activiteit
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var existingVote = await _context.votes
                .FirstOrDefaultAsync(v => v.GatheringId == gatheringId && v.UserId == userId);

            if (existingVote == null)
            {
                // Voeg de stem van de gebruiker toe
                var vote = new Vote
                {
                    GatheringId = gatheringId,
                    UserId = userId
                };

                _context.Add(vote);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", "Gatherings", new { id = gatheringId });
        }
    }
}
