using ISA.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISA.ViewComponents
{
    public class DestinationsViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public DestinationsViewComponent(ApplicationDbContext context)
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

            var airlineDestinations = _context.AirlineDestinations.Include(a => a.Destination).Include(a => a.Airline).Include(a => a.Airline.Provider).Where(a => a.Removed == false).Where(a => a.Airline.AirlineName == airlineName);

            return View(airlineDestinations);
        }
    }
}
