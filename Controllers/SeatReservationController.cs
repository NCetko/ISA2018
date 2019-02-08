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
using ISA.Models.SeatReservationViewModels;
using System.Net.Mail;
using System.Net;

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
            var user = _context.Users.Find((await _userManager.GetUserAsync(HttpContext.User)).Id);
            List<SeatReservation> seatReservations = await _context.SeatReservations.Where(s => s.ApplicationUser == user).ToListAsync();
            return View(seatReservations);
        }

        // POST: SeatReservation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(CreateViewModel viewModel)
        {
            var user = _context.Users.Find((await _userManager.GetUserAsync(HttpContext.User)).Id);
            Flight flight = _context.Flights.Find(viewModel.FlightName);
            _context.Entry(flight).Reference(f => f.Airplane).Load();
            _context.Entry(flight.Airplane).Reference(f => f.Airline).Load();

            if (flight == null)
            {
                return NotFound();
            };
            Seat seat = _context.Seats.Find(flight.Airplane.AirplaneName, viewModel.SegmentName, viewModel.SeatName);
            _context.Entry(seat).Reference(s => s.Segment).Load();
            if (seat == null)
            {
                return NotFound();
            }

            Reservation reservation = (
                    from r in _context.Reservations
                    where _context.SeatReservations.Any(
                            sr => sr.Flight.FlightName == viewModel.FlightName
                            && sr.Reservation == r
                        )
                    && r.ApplicationUser == user
                    select r
                ).FirstOrDefault();

            if (reservation == null)
            {
                reservation = new Reservation
                {
                    Created = DateTime.Now,
                    ApplicationUser = user,
                    TotalPrice = flight.Price,
                    Airline = flight.Airplane.Airline
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

            SeatReservation seatReservation = new SeatReservation
            {
                Flight = flight,
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                ApplicationUser = user,
                Confirmed = true,
                Passport = viewModel.Passport,
                Seat = seat,
                Reservation = reservation
            };

            if (ModelState.IsValid)
            {
                _context.Add(seatReservation);
                await _context.SaveChangesAsync();

                var smtpClient = new SmtpClient
                {
                    Host = "smtp.gmail.com", // set your SMTP server name here
                    Port = 587, // Port 
                    EnableSsl = true,
                    Credentials = new NetworkCredential("isa.ftn.2018@gmail.com", "~4;L2wTtt}c3h)Zw")
                };

                using (var message = new MailMessage("isa.ftn.2018@gmail.com", user.Email)
                {
                    Subject = "New Reservation",
                    Body = "You have reserved seat " + seat.SeatName + " segment " + seat.Segment.SegmentName + " flight "+flight.FlightName
                })
                {
                    await smtpClient.SendMailAsync(message);
                }

                return RedirectToAction("Details", "Flight", new { id = flight.FlightName });
            }
            return Forbid();
        }

        [HttpPost]
        public async Task<IActionResult> Invite(InviteViewModel viewModel)
        {
            var user = _context.Users.Find((await _userManager.GetUserAsync(HttpContext.User)).Id);
            var friend = _context.Users.Find(viewModel.FriendId);
            Flight flight = _context.Flights.Find(viewModel.FlightName);
            _context.Entry(flight).Reference(f => f.Airplane).Load();
            _context.Entry(flight.Airplane).Reference(f => f.Airline).Load();

            if (flight == null)
            {
                return NotFound();
            };
            Seat seat = _context.Seats.Find(flight.Airplane.AirplaneName, viewModel.SegmentName, viewModel.SeatName);
            if (seat == null)
            {
                return NotFound();
            }

            Reservation reservation = (
                    from r in _context.Reservations
                    where _context.SeatReservations.Any(
                            sr => sr.Flight.FlightName == viewModel.FlightName
                            && sr.Reservation == r
                        )
                    && r.ApplicationUser == user
                    select r
                ).FirstOrDefault();

            if (reservation == null)
            {
                reservation = new Reservation
                {
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

            SeatReservation seatReservation = new SeatReservation
            {
                Flight = flight,
                ApplicationUser = friend,
                Confirmed = false,
                Seat = seat,
                Reservation = reservation
            };

            if (ModelState.IsValid)
            {
                _context.Add(seatReservation);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Flight", new { id = flight.FlightName });
            }
            return Forbid();
        }

        [HttpPost]
        public async Task<IActionResult> Answer(AnswerViewModel viewModel)
        {
            var user = _context.Users.Find((await _userManager.GetUserAsync(HttpContext.User)).Id);
            SeatReservation seatReservation = _context.SeatReservations.Where(s => s.SeatReservationId == viewModel.SeatReservationId).FirstOrDefault();
            _context.Entry(seatReservation).Reference(s => s.Flight).Load();
            if (seatReservation == null)
            {
                return NotFound();
            }

            if (viewModel.Answer)
            {
                seatReservation.Confirmed = true;
                if (ModelState.IsValid)
                {
                    _context.Update(seatReservation);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    _context.SeatReservations.Remove(seatReservation);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return Forbid();
        }


        private bool SeatReservationExists(string id)
        {
            return _context.SeatReservations.Any(e => e.FlightName == id);
        }
    }
}
