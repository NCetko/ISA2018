using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ISA.Data;
using ISA.Models.Entities;
using ISA.Models.FlightViewModels;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Identity;

namespace ISA.Controllers
{
    public class FlightController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FlightController(
            UserManager<ApplicationUser> userManager, 
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Flight
        public async Task<IActionResult> Index()
        {
            return View(await _context.Flights.ToListAsync());
        }

        // GET: Flight/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights
                .Include(f=>f.Airplane)
                .Include(f=>f.Airplane.Airline)
                .Include(f=>f.Airplane.Airline.Provider)
                .Include(f=>f.DepartureLocation)
                .Include(f=>f.DepartureLocation.Destination)
                .Include(f=>f.ArrivalLocation.Destination)
                .Include(f=>f.ArrivalLocation.Destination)
                .FirstOrDefaultAsync(m => m.FlightName == id);

            if (flight == null)
            {
                return NotFound();
            }

            var seats = _context.Seats.Where(s => s.AirplaneName == flight.Airplane.AirplaneName);

            var reservedSeats = await (
                from s in _context.Seats
                where _context.SeatReservations.Any(sr => sr.Seat == s && sr.Flight.Departure < DateTime.Now)
                || _context.SeatDiscounts.Any(sr => sr.Seat == s && sr.Flight.Departure < DateTime.Now)
                select s
                ).ToListAsync();

            var user = _context.Users.Find((await _userManager.GetUserAsync(HttpContext.User)).Id);

            var friends = await (
                from u in _context.Users
                where _context.Friendships.Any(
                    fs => (fs.Sender == user || fs.Receiver == user)
                    && (fs.Sender == u|| fs.Receiver == u))
                select u).Where(u => u != user)
                .ToListAsync(); 

            ViewBag.Friends = new SelectList(friends, "Id", "UserName");
            ViewBag.ReservedSeats = reservedSeats;

            ViewBag.Segments = _context.Segments.Where(s => s.AirplaneName == flight.Airplane.AirplaneName);
            ViewBag.Seats = seats;

            return View(flight);
        }

        // GET: Flight/Create/airlineName
        [HttpGet("/Flight/Create/{airlineName}")]
        public IActionResult Create(string airlineName)
        {
            if (string.IsNullOrEmpty(airlineName))
            {
                return NotFound();
            }

            var airline = _context.Airlines.Find(airlineName);
            if (airline == null)
            {
                return NotFound();
            }

            ViewBag.DuplicateKey = false;

            var airplanes = _context.Airplanes.Include(a => a.Airline).Where(a => a.Airline.AirlineName == airlineName).ToList();
            ViewBag.Airplanes = new SelectList(airplanes, "AirplaneName", "AirplaneName");

            var destinations = (from d in _context.Destinations
                                from ad in _context.AirlineDestinations
                                where ad.Destination == d
                                where ad.Airline.AirlineName == airlineName
                                select d).ToList();
            ViewBag.Destinations = new SelectList(destinations, "DestinationName", "DestinationName");

            var viewModel = new CreateViewModel
            {
                AirlineName = airlineName,
                Arrival = DateTime.Now,
                Departure = DateTime.Now,
                KM = 10,
                Price = 10
            };

            return View(viewModel);
        }

        // POST: Flight/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("/Flight/Create/{airlineName}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string airlineName, CreateViewModel viewModel)
        {
            if (FlightExists(viewModel.FlightName))
            {
                ViewBag.DuplicateKey = true;
                return View(viewModel);
            }
            var flight = new Flight
            {
                Airplane = _context.Airplanes.Find(viewModel.AirplaneName),
                Arrival = viewModel.Arrival,
                ArrivalLocation = _context.AirlineDestinations.Find(airlineName, viewModel.ArrivalLocationName),
                BaggageDetails = viewModel.BaggageDetails,
                CarryOnBag = viewModel.CarryOnBag,
                CheckedBag = viewModel.CheckedBag,
                Departure = viewModel.Departure,
                FlightName = viewModel.FlightName,
                KM = viewModel.KM,
                Price = viewModel.Price,
                Services = viewModel.Services,
                DepartureLocation = _context.AirlineDestinations.Find(airlineName, viewModel.DepartureLocationName)
            };


            if (ModelState.IsValid)
            {
                try{
                    _context.Add(flight);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Airline", new { id = airlineName });
                }
                catch (SqlException)
                {
                    if (FlightExists(flight.FlightName))
                    {
                        ViewBag.DuplicateKey = true;
                        return View(viewModel);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(viewModel);
        }

        // GET: Flight/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights.FindAsync(id);
            if (flight == null)
            {
                return NotFound();
            }
            _context.Entry(flight).Reference(f => f.Airplane).Load();
            _context.Entry(flight.Airplane).Reference(f => f.Airline).Load();
            _context.Entry(flight).Reference(f => f.DepartureLocation).Load();
            _context.Entry(flight.DepartureLocation).Reference(f => f.Destination).Load();
            _context.Entry(flight).Reference(f => f.ArrivalLocation).Load();
            _context.Entry(flight.ArrivalLocation).Reference(f => f.Destination).Load();

            var airplanes = _context.Airplanes.Include(a => a.Airline).Where(a => a.Airline.AirlineName == flight.Airplane.Airline.AirlineName).ToList();
            ViewBag.Airplanes = new SelectList(airplanes, "AirplaneName", "AirplaneName");

            var destinations = (from d in _context.Destinations
                                from ad in _context.AirlineDestinations
                                where ad.Destination == d
                                where ad.Airline.AirlineName == flight.Airplane.Airline.AirlineName
                                select d).ToList();
            ViewBag.Destinations = new SelectList(destinations, "DestinationName", "DestinationName");

            var viewModel = new CreateViewModel {
                AirlineName = flight.Airplane.Airline.AirlineName,
                AirplaneName = flight.Airplane.AirplaneName,
                Arrival = flight.Arrival,
                ArrivalLocationName = flight.ArrivalLocation.Destination.DestinationName,
                BaggageDetails = flight.BaggageDetails,
                CarryOnBag = flight.CarryOnBag,
                CheckedBag = flight.CheckedBag,
                Departure = flight.Departure,
                DepartureLocationName = flight.DepartureLocation.Destination.DestinationName,
                FlightName = flight.FlightName,
                KM = flight.KM,
                Price = flight.Price,
                Services = flight.Services
            }; 

            return View(viewModel);
        }

        // POST: Flight/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, CreateViewModel viewModel)
        {
            if (id != viewModel.FlightName)
            {
                return NotFound();
            }

            var flight = _context.Flights.Find(id);
            flight.Airplane = _context.Airplanes.Find(viewModel.AirplaneName);
            flight.Arrival = viewModel.Arrival;
            flight.ArrivalLocation = _context.AirlineDestinations.Find(viewModel.AirlineName, viewModel.ArrivalLocationName);
            flight.BaggageDetails = viewModel.BaggageDetails;
            flight.CarryOnBag = viewModel.CarryOnBag;
            flight.CheckedBag = viewModel.CheckedBag;
            flight.Departure = viewModel.Departure;
            flight.KM = viewModel.KM;
            flight.Price = viewModel.Price;
            flight.Services = viewModel.Services;
            flight.DepartureLocation = _context.AirlineDestinations.Find(viewModel.AirlineName, viewModel.DepartureLocationName);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(flight);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlightExists(flight.FlightName))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Airline", new { id = viewModel.AirlineName });
            }
            return View(flight);
        }

        // GET: Flight/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights
                .FirstOrDefaultAsync(m => m.FlightName == id);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        // POST: Flight/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var flight = await _context.Flights.FindAsync(id);
            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Airline", new { id = flight.Airplane.Airline.AirlineName});
        }

        private bool FlightExists(string id)
        {
            return _context.Flights.Any(e => e.FlightName == id);
        }
    }
}
