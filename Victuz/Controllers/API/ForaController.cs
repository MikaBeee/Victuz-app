using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Victuz.Data;
using Victuz.Models.Businesslayer;

namespace Victuz.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForaController : ControllerBase
    {
        private readonly VictuzDB _context;

        public ForaController(VictuzDB context)
        {
            _context = context;
        }

        // GET: api/Fora
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Forum>>> Getforum()
        {
            return await _context.forum.ToListAsync();
        }

        // GET: api/Fora/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Forum>> GetForum(int id)
        {
            var forum = await _context.forum.FindAsync(id);

            if (forum == null)
            {
                return NotFound();
            }

            return forum;
        }

        // PUT: api/Fora/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutForum(int id, Forum forum)
        {
            if (id != forum.ForumId)
            {
                return BadRequest();
            }

            _context.Entry(forum).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ForumExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Fora
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Forum>> PostForum(Forum forum)
        {
            _context.forum.Add(forum);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetForum", new { id = forum.ForumId }, forum);
        }

        // DELETE: api/Fora/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteForum(int id)
        {
            var forum = await _context.forum.FindAsync(id);
            if (forum == null)
            {
                return NotFound();
            }

            _context.forum.Remove(forum);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ForumExists(int id)
        {
            return _context.forum.Any(e => e.ForumId == id);
        }
    }
}
