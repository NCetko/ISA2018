using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ISA.Data;
using ISA.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System;

namespace ISA.Controllers
{
    public class SeatDiscountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SeatDiscountController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: SeatDiscount
        public async Task<IActionResult> Index()
        {
            return NotFound();
        }

        // POST: SeatDiscount/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(int price, string flightName, string seatName, string airplaneName, string segmentName)
        {
            Flight flight = await _context.Flights.FindAsync(flightName);
            if (flight == null)
            {
                return NotFound();
            }
            _context.Entry(flight).Reference(f => f.Airplane).Load();
            _context.Entry(flight.Airplane).Reference(f => f.Airline).Load();
            SeatDiscount seatDiscount = new SeatDiscount
            {
                Seat = await _context.Seats.FindAsync(airplaneName, segmentName, seatName),
                Price = price,
                Flight = flight
            };
            if (ModelState.IsValid)
            {
                _context.Add(seatDiscount);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Airline", new { id = seatDiscount.Flight.Airplane.Airline.AirlineName });
            }
            return Forbid();
        }

        [HttpPost]
        public async Task<IActionResult> Reserve(int discountId)
        {
            var user = _context.Users.Find((await _userManager.GetUserAsync(HttpContext.User)).Id);
            var seatDiscount = await _context.SeatDiscounts
                .Include(s => s.Flight)
                .Include(s => s.Flight.Airplane)
                .Include(s => s.Flight.Airplane.Airline)
                .Where(s => s.DiscountId == discountId).FirstOrDefaultAsync();

            if (user == null || seatDiscount == null)
            {
                return Forbid();
            }

            Reservation reservation = new Reservation
            {
                Created = DateTime.Now,
                ApplicationUser = user,
                TotalPrice = seatDiscount.Price,
                Airline = seatDiscount.Flight.Airplane.Airline
            };

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(reservation);
                    await _context.SaveChangesAsync();

                    seatDiscount.Reservation = reservation;
                    _context.Update(seatDiscount);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Airline", new { id = seatDiscount.Flight.Airplane.Airline.AirlineName });
                }
                catch
                {
                    return BadRequest();
                }
            }
            return RedirectToAction("Details", "Airline", new { id = seatDiscount.Flight.Airplane.Airline.AirlineName });
        }

        // GET: SeatDiscount/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var seatDiscount = await _context.SeatDiscounts.Where(s => s.DiscountId == id).FirstOrDefaultAsync();
            await _context.Entry(seatDiscount).Reference(a => a.Seat).LoadAsync();
            await _context.Entry(seatDiscount).Reference(a => a.Flight).LoadAsync();

            if (seatDiscount == null)
            {
                return NotFound();
            }

            return View(seatDiscount);
        }

        // POST: SeatDiscount/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var seatDiscount = await _context.SeatDiscounts
                .Include(s => s.Flight)
                .Include(s => s.Flight.Airplane)
                .Include(s => s.Flight.Airplane.Airline)
                .Where(s => s.DiscountId == id).FirstOrDefaultAsync();
            _context.SeatDiscounts.Remove(seatDiscount);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Airline", new { id = seatDiscount.Flight.Airplane.Airline.AirlineName });
        }

        private bool SeatDiscountExists(string id)
        {
            return _context.SeatDiscounts.Any(e => e.FlightName == id);
        }
    }
}
