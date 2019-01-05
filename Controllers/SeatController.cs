using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ISA.Data;
using ISA.Models.Entities;
using ISA.Models.SeatViewModels;

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

        //[HttpGet("/Segment/Edit/{airplaneName}/{segmentName}")]
        //public async Task<IActionResult> Edit(string airplaneName, string segmentName)

        // GET: Seat/Create/SegmentName

        [HttpGet("/Seat/Create/{airplaneName}/{segmentName}")]
        public async Task<IActionResult> Create(string airplaneName, string segmentName)
        {
            if (string.IsNullOrEmpty(airplaneName) || string.IsNullOrEmpty(segmentName))
            {
                return BadRequest();
            }

            var airplane = await _context.Airplanes.Include(a => a.Airline).Include(a => a.Airline.Provider)
                .FirstOrDefaultAsync(m => m.AirplaneName == airplaneName);

            if (airplane == null)
            {
                return NotFound();
            }

            ViewBag.Segments = _context.Segments.Where(s => s.AirplaneName == airplaneName);
            ViewBag.Seats = _context.Seats.Where(s => s.AirplaneName == airplaneName);


            var model = new CreateViewModel
            {
                X = 50,
                Y = 50
            };

            return View(model);
        }

        // POST: Seat/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("/Seat/Create/{airplaneName}/{segmentName}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string airplaneName, string segmentName, CreateViewModel viewModel)
        {
            Seat seat = new Seat
            {
                Segment = _context.Segments.Find(airplaneName, segmentName),
                SeatName = viewModel.SeatName,
                X = viewModel.X,
                Y = viewModel.Y
            };

            if (ModelState.IsValid)
            {
                _context.Add(seat);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Airplane", new { id = airplaneName });
            }
            ViewData["AirplaneName"] = new SelectList(_context.Segments, "AirplaneName", "AirplaneName", seat.AirplaneName);
            return View(seat);
        }

        [HttpGet("/Seat/Edit/{airplaneName}/{segmentName}/{seatName}")]
        public async Task<IActionResult> Edit(string airplaneName, string segmentName, string seatName)
        {
            if (string.IsNullOrEmpty(airplaneName) || string.IsNullOrEmpty(segmentName) || string.IsNullOrEmpty(seatName))
            {
                return NotFound();
            }

            var seat = await _context.Seats.FindAsync(airplaneName, segmentName, seatName);
            if (seat == null)
            {
                return NotFound();
            }

            var viewModel = new EditViewModel
            {
                SeatName = seat.SeatName,
                AirplaneName = seat.AirplaneName,
                SegmentName = seat.SegmentName,
                X = seat.X,
                Y = seat.Y
            };

            var airplane = await _context.Airplanes
                .Include(a => a.Airline)
                .Include(a => a.Airline.Provider)
                .FirstOrDefaultAsync(m => m.AirplaneName == airplaneName);

            if (airplane == null)
            {
                return NotFound();
            }
            
            ViewBag.Segments = _context.Segments.Where(s => s.AirplaneName == airplaneName);
            ViewBag.Seats = _context.Seats.Where(s => s.AirplaneName == airplaneName);

            return View(viewModel);
        }

        // POST: Seat/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("/Seat/Edit/{airplaneId}/{segmentId}/{seatId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string airplaneId, string segmentId, string seatId, EditViewModel viewModel)
        {
            if (string.IsNullOrEmpty(airplaneId) || string.IsNullOrEmpty(segmentId) || string.IsNullOrEmpty(seatId))
            {
                return NotFound();
            }

            Seat seat = await _context.Seats.FindAsync(airplaneId, segmentId, seatId);
            if(seat == null)
            {
                return NotFound();
            }
            await _context.Entry(seat).Reference(p => p.Segment).LoadAsync();

            seat.X = viewModel.X;
            seat.Y = viewModel.Y;

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
                return RedirectToAction("Details", "Airplane", new { id = airplaneId });
            }

            var airplane = await _context.Airplanes
                .Include(a => a.Airline)
                .Include(a => a.Airline.Provider)
                .FirstOrDefaultAsync(m => m.AirplaneName == airplaneId);

            if (airplane == null)
            {
                return NotFound();
            }

            ViewBag.Segments = _context.Segments.Where(s => s.AirplaneName == airplaneId);
            ViewBag.Seats = _context.Seats.Where(s => s.AirplaneName == airplaneId);

            return View(viewModel);
        }

        [HttpGet("/Seat/Delete/{airplaneName}/{segmentName}/{seatName}")]
        public async Task<IActionResult> Delete(string airplaneName, string segmentName, string seatName)
        {
            if (string.IsNullOrEmpty(airplaneName) || string.IsNullOrEmpty(segmentName) || string.IsNullOrEmpty(seatName))
            {
                return NotFound();
            }

            var seat = await _context.Seats.FindAsync(airplaneName,segmentName,seatName);
            if (seat == null)
            {
                return NotFound();
            }

            var viewModel = new EditViewModel
            {
                SeatName = seat.SeatName,
                AirplaneName = seat.AirplaneName,
                SegmentName = seat.SegmentName,
                X = seat.X,
                Y = seat.Y
            };

            return View(viewModel);
        }

        // POST: Seat/Delete/5
        [HttpPost("/Seat/Delete/{airplaneName}/{segmentName}/{seatName}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string airplaneName, string segmentName, string seatName)
        {
            var seat = await _context.Seats.FindAsync(airplaneName, segmentName, seatName);
            _context.Seats.Remove(seat);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Airplane", new { id = airplaneName });
        }

        private bool SeatExists(string id)
        {
            return _context.Seats.Any(e => e.AirplaneName == id);
        }
    }
}
