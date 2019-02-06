using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ISA.Data;
using ISA.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using ISA.Models.AirplaneViewModels;

namespace ISA.Controllers
{
    [AllowAnonymous]
    public class AirplaneController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AirplaneController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("Airplane/{airlineName}")]
        public async Task<IActionResult> Index(string airlineName)
        {
            if(airlineName == null)
            {
                return NotFound();
            }
            return View(await _context.Airplanes.Where(a => a.Airline.AirlineName == airlineName).ToListAsync());
        }

        // GET: Airplane/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airplane = await _context.Airplanes.Include(a=>a.Airline).Include(a=>a.Airline.Provider)
                .FirstOrDefaultAsync(m => m.AirplaneName == id);
            if (airplane == null)
            {
                return NotFound();
            }

            var seats = _context.Seats.Where(s => s.AirplaneName == id);
            var reservedSeats = await (
                from s in _context.Seats
                where _context.SeatReservations.Any(sr => sr.Seat == s && sr.Flight.Departure < DateTime.Now)
                || _context.SeatDiscounts.Any(sr => sr.Seat == s && sr.Flight.Departure < DateTime.Now)
                select s
                ).ToListAsync();

            ViewBag.Segments = _context.Segments.Where(s => s.AirplaneName == id);
            ViewBag.Seats = seats;
            ViewBag.ReservedSeats = reservedSeats;

            return View(airplane);
        }

        [HttpGet("/Airplane/Create/{AirlineName}")]
        // GET: Airplane/Create
        public IActionResult Create(string AirlineName)
        {
            CreateViewModel viewModel = new CreateViewModel {
                AirlineName = AirlineName
            };
            return View(viewModel);
        }

        // POST: Airplane/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateViewModel viewModel)
        {
            Airplane airplane = new Airplane {
                AirplaneName = viewModel.AirplaneName,
                Airline = _context.Airlines.Find(viewModel.AirlineName)
            };
            if (ModelState.IsValid)
            {
                _context.Add(airplane);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Airplane", new { id = viewModel.AirplaneName});
            }
            return View(airplane);
        }

        // GET: Airplane/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airplane = await _context.Airplanes.Include(a=>a.Airline)
                .FirstOrDefaultAsync(m => m.AirplaneName == id);
            if (airplane == null)
            {
                return NotFound();
            }

            return View(airplane);
        }

        // POST: Airplane/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var airplane = await _context.Airplanes.FindAsync(id);
            _context.Entry(airplane).Reference(a => a.Airline).Load();
            string airlineName = airplane.Airline.AirlineName;
            _context.Airplanes.Remove(airplane);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Airline", new { id = airlineName });
        }

        private bool AirplaneExists(string id)
        {
            return _context.Airplanes.Any(e => e.AirplaneName == id);
        }
    }
}
