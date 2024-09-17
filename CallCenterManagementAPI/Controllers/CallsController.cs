using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CallCenterManagementAPI.Data;
using CallCenterManagementAPI.Model;

namespace CallCenterManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallsController : ControllerBase
    {
        private readonly CallCenterManagementAPIContext _context;

        public CallsController(CallCenterManagementAPIContext context)
        {
            _context = context;
        }

        // GET: api/Calls
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Call>>> GetCall()
        {
          if (_context.Call == null)
          {
              return NotFound();
          }
            return await _context.Call.ToListAsync();
        }

        // GET: api/Calls/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Call>> GetCall(int id)
        {
          if (_context.Call == null)
          {
              return NotFound();
          }
            var call = await _context.Call.FindAsync(id);

            if (call == null)
            {
                return NotFound();
            }

            return call;
        }

        // PUT: api/Calls/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCall(int id, Call call)
        {
            if (id != call.Id)
            {
                return BadRequest();
            }

            _context.Entry(call).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CallExists(id))
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

        // POST: api/Calls
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Call>> PostCall(Call call)
        {
          if (_context.Call == null)
          {
              return Problem("Entity set 'CallCenterManagementAPIContext.Call'  is null.");
          }
            _context.Call.Add(call);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCall", new { id = call.Id }, call);
        }

        // DELETE: api/Calls/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCall(int id)
        {
            if (_context.Call == null)
            {
                return NotFound();
            }
            var call = await _context.Call.FindAsync(id);
            if (call == null)
            {
                return NotFound();
            }

            _context.Call.Remove(call);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CallExists(int id)
        {
            return (_context.Call?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
