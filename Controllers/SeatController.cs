using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ISA.Data;
using ISA.Models.Entities;

namespace ISA.Controllers
{
    public class SeatController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SeatController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Seat
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Seats.Include(s => s.Segment);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Seat/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seat = await _context.Seats
                .Include(s => s.Segment)
                .FirstOrDefaultAsync(m => m.AirplaneName == id);
            if (seat == null)
            {
                return NotFound();
            }

            return View(seat);
        }

        // GET: Seat/Create
        public IActionResult Create()
        {
            ViewData["AirplaneName"] = new SelectList(_context.Segments, "AirplaneName", "AirplaneName");
            return View();
        }

        // POST: Seat/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SeatName,SegmentName,AirplaneName,X,Y")] Seat seat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(seat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AirplaneName"] = new SelectList(_context.Segments, "AirplaneName", "AirplaneName", seat.AirplaneName);
            return View(seat);
        }

        // GET: Seat/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seat = await _context.Seats.FindAsync(id);
            if (seat == null)
            {
                return NotFound();
            }
            ViewData["AirplaneName"] = new SelectList(_context.Segments, "AirplaneName", "AirplaneName", seat.AirplaneName);
            return View(seat);
        }

        // POST: Seat/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("SeatName,SegmentName,AirplaneName,X,Y")] Seat seat)
        {
            if (id != seat.AirplaneName)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(seat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SeatExists(seat.AirplaneName))
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
            ViewData["AirplaneName"] = new SelectList(_context.Segments, "AirplaneName", "AirplaneName", seat.AirplaneName);
            return View(seat);
        }

        // GET: Seat/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seat = await _context.Seats
                .Include(s => s.Segment)
                .FirstOrDefaultAsync(m => m.AirplaneName == id);
            if (seat == null)
            {
                return NotFound();
            }

            return View(seat);
        }

        // POST: Seat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var seat = await _context.Seats.FindAsync(id);
            _context.Seats.Remove(seat);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SeatExists(string id)
        {
            return _context.Seats.Any(e => e.AirplaneName == id);
        }
    }
}
