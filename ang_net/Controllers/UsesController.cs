using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ang_net.Models;
using Microsoft.AspNetCore.Cors;

namespace ang_net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CrosPolicy")]
    public class UsesController : ControllerBase
    {
        private readonly ang_netContext _context;

        public UsesController(ang_netContext context)
        {
            _context = context;
        }

        // GET: api/Uses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Use>>> GetUse()
        {
            return await _context.Use.ToListAsync();
        }

        // GET: api/Uses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Use>> GetUse(int id)
        {
            
            var use = await _context.Use.FindAsync(id);

            if (use == null)
            {
                return NotFound();
            }

            return use;
        }

        // PUT: api/Uses/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        
        [HttpPut("{id}")]
        
        public async Task<IActionResult> PutUse(int id, Use use)
        {
            if (id != use.Id)
            {
                return BadRequest("id est "+id);
            }

            _context.Entry(use).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UseExists(id))
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

        // POST: api/Uses
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Use>> PostUse(Use use)
        {
            _context.Use.Add(use);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUse", new { id = use.Id }, use);
        }

        // DELETE: api/Uses/5
        [HttpDelete("{id}")]
        
        public async Task<ActionResult<Use>> DeleteUse(int id)
        {
            var use = await _context.Use.FindAsync(id);
            if (use == null)
            {
                return NotFound();
            }

            _context.Use.Remove(use);
            await _context.SaveChangesAsync();

            return use;
        }

        private bool UseExists(int id)
        {
            return _context.Use.Any(e => e.Id == id);
        }
    }
}
