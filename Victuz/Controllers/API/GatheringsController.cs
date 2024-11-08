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
    public class GatheringsController : ControllerBase
    {
        private readonly VictuzDB _context;

        public GatheringsController(VictuzDB context)
        {
            _context = context;
        }

        // GET: api/Gatherings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Gathering>>> Getgathering()
        {
            return await _context.gathering.ToListAsync();
        }

        // GET: api/Gatherings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Gathering>> GetGathering(int id)
        {
            var gathering = await _context.gathering.FindAsync(id);

            if (gathering == null)
            {
                return NotFound();
            }

            return gathering;
        }

        // PUT: api/Gatherings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGathering(int id, Gathering gathering)
        {
            if (id != gathering.GatheringId)
            {
                return BadRequest();
            }

            _context.Entry(gathering).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GatheringExists(id))
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

        // POST: api/Gatherings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Gathering>> PostGathering(Gathering gathering)
        {
            _context.gathering.Add(gathering);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGathering", new { id = gathering.GatheringId }, gathering);
        }

        // DELETE: api/Gatherings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGathering(int id)
        {
            var gathering = await _context.gathering.FindAsync(id);
            if (gathering == null)
            {
                return NotFound();
            }

            _context.gathering.Remove(gathering);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GatheringExists(int id)
        {
            return _context.gathering.Any(e => e.GatheringId == id);
        }
    }
}
