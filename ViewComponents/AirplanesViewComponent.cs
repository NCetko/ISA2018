using ISA.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISA.ViewComponents
{
    public class AirplanesViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public AirplanesViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(
        string airlineName)
        {
            ViewBag.AirlineName = airlineName;
            if (airlineName == null)
            {
                return(null);
            }

            return View(await _context.Airplanes.Where(a => a.Airline.AirlineName == airlineName).ToListAsync());
        }
    }
}
