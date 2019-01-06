using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ISA.Data;
using ISA.Models.Entities;
using ISA.Models.AirlineDestinationViewModels;

namespace ISA.Controllers
{
    public class AirlineDestinationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AirlineDestinationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AirlineDestination
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.AirlineDestinations.Include(a => a.Airline).Include(a => a.Destination);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AirlineDestination/Create
        public IActionResult Create(string id)
        {
            if(string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var model = new CreateViewModel {
                AirlineName = id
            };

            var availableDestinations = (
                from d in _context.Destinations
                where !_context.AirlineDestinations.Any(ad => ad.Removed == false && ad.AirlineName == id && ad.DestinationName == d.DestinationName)
                select d
            );
            ViewData["DestinationName"] = new SelectList(availableDestinations, "DestinationName", "DestinationName");
            return View(model);
        }

        // POST: AirlineDestination/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateViewModel viewModel)
        {
            var existingAD = _context.AirlineDestinations.Find(viewModel.AirlineName, viewModel.DestinationName);
            if(existingAD!= null)
            {
                existingAD.Removed = false;

                if (ModelState.IsValid)
                {
                    _context.Update(existingAD);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Airline", new { id = viewModel.AirlineName });
                }
            }

            if (ModelState.IsValid)
            {
                var ad = new AirlineDestination {
                    Destination = _context.Destinations.Find(viewModel.DestinationName),
                    Airline = _context.Airlines.Find(viewModel.AirlineName)
                };
                _context.Add(ad);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Airline", new { id = viewModel.AirlineName });
            }

            var availableDestinations = (
                from d in _context.Destinations
                where !_context.AirlineDestinations.Any(ad => ad.Removed == false && ad.AirlineName == viewModel.AirlineName && ad.DestinationName == d.DestinationName)
                select d
            );
            ViewData["DestinationName"] = new SelectList(availableDestinations, "DestinationName", "DestinationName");

            return View(viewModel);
        }

        [HttpGet("/AirlineDestination/Delete/{airlineName}/{destinationName}")]
        public async Task<IActionResult> Delete(string airlineName, string destinationName)
        {
            if (string.IsNullOrEmpty(airlineName) || string.IsNullOrEmpty(destinationName))
            {
                return NotFound();
            }

            var airlineDestination = await _context.AirlineDestinations.FindAsync(airlineName, destinationName);

            if (airlineDestination == null)
            {
                return NotFound();
            }
            await _context.Entry(airlineDestination).Reference(a => a.Airline).LoadAsync();
            await _context.Entry(airlineDestination).Reference(a => a.Destination).LoadAsync();

            return View(airlineDestination);
        }

        // POST: AirlineDestination/Delete/5
        [HttpPost("/AirlineDestination/Delete/{airlineName}/{destinationName}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string airlineName, string destinationName)
        {
            var airlineDestination = await _context.AirlineDestinations.FindAsync(airlineName, destinationName);
            airlineDestination.Removed = true;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(airlineDestination);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AirlineDestinationExists(airlineDestination.AirlineName))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return RedirectToAction("Details", "Airline", new { id = airlineName });
        }

        private bool AirlineDestinationExists(string id)
        {
            return _context.AirlineDestinations.Any(e => e.AirlineName == id);
        }
    }
}
