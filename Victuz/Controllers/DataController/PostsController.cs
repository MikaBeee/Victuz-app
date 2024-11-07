using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Victuz.Data;
using Victuz.Models.Businesslayer;
using Victuz.Models.Viewmodels;

namespace Victuz.Controllers.DataController
{
    public class PostsController : Controller
    {
        private readonly VictuzDB _context;

        public PostsController(VictuzDB context)
        {
            _context = context;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            var victuzDB = _context.post.Include(p => p.Forum).Include(p => p.User);
            return View(await victuzDB.ToListAsync());
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.post
                .Include(p => p.Forum)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        public async Task<List<PostVM>> AllPosts()
        {
            var victuzDB = _context.post
                .Include(p => p.Forum)
                .Include(p => p.User)
                .Select(p => new PostVM
                {
                    PostId = p.PostId,
                    UserId = p.UserId,
                    User = p.User,
                    ForumId = p.ForumId,
                    Forum = p.Forum,
                    Content = p.Content,
                    PostedDate = p.PostedDate
                })
                .ToListAsync();

            return await victuzDB;
        }

        // GET: Posts/Create
        public IActionResult Create(int forumid)
        {
            ViewData["ForumId"] = new SelectList(_context.forum, "ForumId", "Title");
            ViewData["UserId"] = new SelectList(_context.users, "UserId", "UserName");
            return View(new Post { ForumId = forumid});
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,UserId,ForumId,Content,PostedDate")] Post post)
        {
            post.PostedDate = DateTime.Now;

            if (ModelState.IsValid)
            { 
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Forums", new { id = post.ForumId });
            }
            ViewData["ForumId"] = new SelectList(_context.forum, "ForumId", "Title", post.ForumId);
            ViewData["UserId"] = new SelectList(_context.users, "UserId", "UserName", post.UserId);

            ViewBag.InputValues = post;

            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["ForumId"] = new SelectList(_context.forum, "ForumId", "Title", post.ForumId);
            ViewData["UserId"] = new SelectList(_context.users, "UserId", "Password", post.UserId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,UserId,ForumId,Content,PostedDate")] Post post)
        {
            if (id != post.PostId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.PostId))
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
            ViewData["ForumId"] = new SelectList(_context.forum, "ForumId", "Title", post.ForumId);
            ViewData["UserId"] = new SelectList(_context.users, "UserId", "Password", post.UserId);
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.post
                .Include(p => p.Forum)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.post.FindAsync(id);
            if (post != null)
            {
                _context.post.Remove(post);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.post.Any(e => e.PostId == id);
        }
    }
}
