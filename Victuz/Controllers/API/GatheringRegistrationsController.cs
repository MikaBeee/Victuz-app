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
    public class GatheringRegistrationsController : ControllerBase
    {
        private readonly VictuzDB _context;

        public GatheringRegistrationsController(VictuzDB context)
        {
            _context = context;
        }

        // GET: api/GatheringRegistrations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GatheringRegistration>>> GetgatheringRegistration()
        {
            return await _context.gatheringRegistration.ToListAsync();
        }

        // GET: api/GatheringRegistrations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GatheringRegistration>> GetGatheringRegistration(int id)
        {
            var gatheringRegistration = await _context.gatheringRegistration.FindAsync(id);

            if (gatheringRegistration == null)
            {
                return NotFound();
            }

            return gatheringRegistration;
        }

        // PUT: api/GatheringRegistrations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGatheringRegistration(int id, GatheringRegistration gatheringRegistration)
        {
            if (id != gatheringRegistration.UserId)
            {
                return BadRequest();
            }

            _context.Entry(gatheringRegistration).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GatheringRegistrationExists(id))
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

        // POST: api/GatheringRegistrations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GatheringRegistration>> PostGatheringRegistration(GatheringRegistration gatheringRegistration)
        {
            _context.gatheringRegistration.Add(gatheringRegistration);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (GatheringRegistrationExists(gatheringRegistration.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetGatheringRegistration", new { id = gatheringRegistration.UserId }, gatheringRegistration);
        }

        // DELETE: api/GatheringRegistrations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGatheringRegistration(int id)
        {
            var gatheringRegistration = await _context.gatheringRegistration.FindAsync(id);
            if (gatheringRegistration == null)
            {
                return NotFound();
            }

            _context.gatheringRegistration.Remove(gatheringRegistration);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GatheringRegistrationExists(int id)
        {
            return _context.gatheringRegistration.Any(e => e.UserId == id);
        }
    }
}
