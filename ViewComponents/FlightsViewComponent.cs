using ISA.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISA.ViewComponents
{
    public class FlightsViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public FlightsViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(
        string airlineName)
        {
            ViewBag.AirlineName = airlineName;
            if (string.IsNullOrEmpty(airlineName))
            {
                return (null);
            }
            var airline = _context.Airlines.Find(airlineName);
            _context.Entry(airline).Reference(a => a.Provider).Load();
            ViewBag.Provider = airline.Provider;

            return View(await _context.Flights
                .Include(a => a.Airplane)
                .Include(a => a.Airplane.Airline)
                .Include(a => a.Airplane.Airline.Provider)
                .Include(a => a.DepartureLocation)
                .Include(a => a.DepartureLocation.Destination)
                .Include(a => a.ArrivalLocation)
                .Include(a => a.ArrivalLocation.Destination)
                .Where(a => a.Airplane.Airline.AirlineName == airlineName).ToListAsync());
        }
    }
}
