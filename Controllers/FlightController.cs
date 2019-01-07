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

namespace ISA.Controllers
{
    public class FlightController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FlightController(ApplicationDbContext context)
        {
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
                .FirstOrDefaultAsync(m => m.FlightName == id);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        // GET: Flight/Create/airlineName
        [HttpGet("/Flight/Create/{airlineName}")]
        public IActionResult Create(string airlineName)
        {
            if(string.IsNullOrEmpty(airlineName))
            {
                return NotFound();
            }

            var airline = _context.Airlines.Find(airlineName);
            if(airline == null)
            {
                return NotFound();
            }

            var airplanes = _context.Airplanes.Include(a=>a.Airline).Where(a=>a.Airline.AirlineName == airlineName).ToList();
            ViewBag.Airplanes = new SelectList(airplanes, "AirplaneName", "AirplaneName");

            var destinations = ( from d in _context.Destinations
                                 from ad in _context.AirlineDestinations
                                 where ad.Destination == d
                                 where ad.Airline.AirlineName == airlineName
                                 select d).ToList();
            ViewBag.Destinations = new SelectList(destinations, "DestinationName", "DestinationName");

            var flights = _context.Flights
                .Include(a => a.Airplane)
                .Include(a=>a.Airplane.Airline)
                .Where(a=>a.Airplane.Airline.AirlineName == airlineName).ToList();
            ViewBag.Flights = new SelectList(flights, "FlightName", "FlightName");

            var viewModel = new CreateViewModel
            {
                AirlineName = airlineName,
                Arrival = DateTime.Now,
                Departure = DateTime.Now
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
            var flight = new Flight {
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
                Services = viewModel.Services
            };

            if (string.IsNullOrEmpty(viewModel.PrecedingFlightName))
            {
                flight.DepartureLocation = _context.AirlineDestinations.Find(airlineName, viewModel.DepartureLocationName);
            }
            else
            {
                var precedingFlight = _context.Flights.Find(viewModel.PrecedingFlightName);
                if(precedingFlight == null)
                {
                    NotFound();
                }
                _context.Entry(precedingFlight).Reference(f => f.DepartureLocation).Load();
                flight.PrecedingFlight = precedingFlight;
                flight.DepartureLocation = precedingFlight.DepartureLocation;
            }

            if (ModelState.IsValid)
            {
                _context.Add(flight);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Airline", new { id = airlineName });
            }
            return View(flight);
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
            return View(flight);
        }

        // POST: Flight/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Departure,Arrival,FlightName,Price,KM,CarryOnBag,CheckedBag,BaggageDetails,Services")] Flight flight)
        {
            if (id != flight.FlightName)
            {
                return NotFound();
            }

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
                return RedirectToAction(nameof(Index));
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
            return RedirectToAction(nameof(Index));
        }

        private bool FlightExists(string id)
        {
            return _context.Flights.Any(e => e.FlightName == id);
        }
    }
}
