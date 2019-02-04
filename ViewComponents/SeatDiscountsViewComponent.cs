using ISA.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISA.ViewComponents
{
    public class SeatDiscountsViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public SeatDiscountsViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(
        string airlineName)
        {
            ViewBag.AirlineName = airlineName;
            if (string.IsNullOrEmpty(airlineName))
            {
                return(null);
            }
            var airline = _context.Airlines.Find(airlineName);
            _context.Entry(airline).Reference(a => a.Provider).Load();
            ViewBag.Provider = airline.Provider;

            return View(await _context.SeatDiscounts
                .Include(a => a.Flight)
                .Include(a => a.Seat)
                .Include(a => a.Flight.Airplane)
                .Include(a => a.Reservation)
                .Include(a => a.Flight.ArrivalLocation)
                .Include(a => a.Flight.ArrivalLocation.Destination)
                .Include(a => a.Flight.DepartureLocation)
                .Include(a => a.Flight.DepartureLocation.Destination)
                .Where(a => a.Flight.Airplane.Airline.AirlineName == airlineName)
                .Where(a => a.Reservation == null)
                .ToListAsync());
        }
    }
}
