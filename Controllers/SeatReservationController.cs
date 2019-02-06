using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ISA.Data;
using ISA.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace ISA.Controllers
{
    public class SeatReservationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SeatReservationController(
            UserManager<ApplicationUser> userManager, 
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: SeatReservation
        public async Task<IActionResult> Index()
        {
            return NotFound();
        }

        // POST: SeatReservation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(string passport, string firstName, string lastName, string flightName, string seatName, string segmentName)
        {
            var user = _context.Users.Find((await _userManager.GetUserAsync(HttpContext.User)).Id);
            Flight flight = _context.Flights.Find(flightName);
            _context.Entry(flight).Reference(f=>f.Airplane).Load();
            _context.Entry(flight.Airplane).Reference(f=>f.Airline).Load();

            if (flight == null) {
                return NotFound();
            };
            Seat seat = _context.Seats.Find(flight.Airplane.AirplaneName, segmentName, seatName);
            if(seat == null)
            {
                return NotFound();
            }

            Reservation reservation = (
                    from r in _context.Reservations
                    where _context.SeatReservations.Any(
                            sr => sr.Flight.FlightName == flightName
                        )
                    select r
                ).FirstOrDefault();

            if (reservation == null)
            {
                reservation = new Reservation {
                    Created = DateTime.Now,
                    ApplicationUser = user,
                    TotalPrice = flight.Price
                };

                if (ModelState.IsValid)
                {
                    _context.Add(reservation);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                reservation.TotalPrice += flight.Price;
            }

            SeatReservation seatReservation = new SeatReservation {
                Flight = flight,
                FirstName = firstName,
                LastName = lastName,
                ApplicationUser = user,
                Confirmed = true,
                Passport = passport,
                Seat = seat,
                Reservation = reservation
            };
            
            if (ModelState.IsValid)
            {
                _context.Add(seatReservation);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Airline", new { id = flight.Airplane.Airline.AirlineName });
            }
            return Forbid();
        }

        private bool SeatReservationExists(string id)
        {
            return _context.SeatReservations.Any(e => e.FlightName == id);
        }
    }
}
