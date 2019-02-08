using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ISA.Data;
using ISA.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using ISA.Models.AirlineViewModels;
using System.IO;

namespace ISA.Controllers
{
    [AllowAnonymous]
    public class AirlineController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AirlineController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Airline
        public async Task<IActionResult> Index()
        {
            var destinations = _context.Destinations.ToList();
            ViewBag.Destinations = new SelectList(destinations, "DestinationName", "DestinationName");

            return View(await _context.Airlines.Include(a => a.Provider).ToListAsync());
        }

        [HttpGet("/Airline/Sales/{id}/{range}")]
        public async Task<IActionResult> Sales(string id, int range)
        {
            if (!(range == 1 || range == 7 || range == 30))
            {
                return NotFound();
            }

            switch (range)
            {
                case 1:
                    {
                        ViewBag.Range = "Daily sales";
                        ViewBag.Labels = "labels: [newDate(-1),newDate(-0.75),newDate(-0.5),newDate(-0.25),newDate(0)],";
                        break;
                    }
                case 7:
                    {
                        ViewBag.Range = "Weekly sales";
                        ViewBag.Labels = "labels: [newDate(-7),newDate(-6),newDate(-5),newDate(-4),newDate(-3)," +
                                         "newDate(-2),newDate(-1),newDate(0)],";
                        break;
                    }
                case 30:
                    {
                        ViewBag.Range = "Monthly sales";
                        ViewBag.Labels = "labels: [newDate(-30),newDate(-25),newDate(-20),newDate(-15)," +
                                         "newDate(-10),newDate(-5),newDate(0)],";
                        break;
                    }
            }

            DateTime startDate = DateTime.Now.AddDays(-range).Date;
            DateTime endDate = DateTime.Now.Date;

            List<SalesViewModel> viewModel = new List<SalesViewModel>();

            List<Reservation> reserevations = await _context.Reservations
                .Where(r => r.Airline.AirlineName == id)
                .Where(r => r.Created.Date >= startDate && r.Created.Date <= endDate)
                .Include(r => r.SeatReservations)
                .ToListAsync();

            foreach (var reservation in reserevations)
            {
                int count = reservation.SeatReservations.Where(r=>r.Confirmed==true).Count();
                count += _context.SeatDiscounts.Where(d => d.Reservation == reservation).ToList().Count();

                viewModel.Add(
                    new SalesViewModel
                    {
                        Count = count,
                        Date = reservation.Created.ToString("MM/dd/yyyy") + " " + reservation.Created.ToString("HH:mm")
                    }
                    );
            }

            return View(viewModel);
        }

        [HttpGet("/Airline/Revenue/{id}/{startDate?}/{endDate?}")]
        public async Task<IActionResult> Revenue(string id, DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null || endDate == null)
            {
                ViewBag.StartDate = DateTime.Now;
                ViewBag.EndDate = DateTime.Now;
                ViewBag.Valid = false;
                ViewBag.AirlineName = id;
                return View();
            }
            ViewBag.Valid = true;
            double revenue;
            try
            {
                revenue = await _context.Reservations
                    .Where(r => r.Created > startDate && r.Created < endDate)
                    .Where(r => r.Airline.AirlineName == id)
                    .SumAsync(r => r.TotalPrice);

            }
            catch
            {
                revenue = 0;
            }

            ViewBag.Revenue = revenue;
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;
            return View();
        }

        // GET: Airline/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airline = await _context.Airlines
                .Include(a => a.Provider)
                .Include(a => a.Ratable)
                .FirstOrDefaultAsync(m => m.AirlineName == id);
            if (airline == null)
            {
                return NotFound();
            }

            try
            {
                var score = _context.Ratings
                .Where(r => r.Ratable == airline.Ratable)
                .Average(r => r.Value);
                ViewBag.Score = score;
            }
            catch { }

            return View(airline);
        }

        // GET: Airline/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Airline/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateViewModel viewModel)
        {
            Airline airline = new Airline
            {
                AirlineName = viewModel.AirlineName,
                Address = viewModel.Address,
                Description = viewModel.Description
            };

            if (viewModel.Image != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await viewModel.Image.CopyToAsync(memoryStream);
                    airline.Image = memoryStream.ToArray();
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(airline);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(airline);
        }

        // GET: Airline/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airline = await _context.Airlines.FindAsync(id);
            if (airline == null)
            {
                return NotFound();
            }
            return View(new EditViewModel(airline));
        }

        // POST: Airline/Edit
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditViewModel viewModel)
        {
            Airline airline = _context.Airlines.Find(viewModel.AirlineName);
            if (airline == null)
            {
                return NotFound();
            }

            airline.Address = viewModel.Address;
            airline.Description = viewModel.Description;

            if (viewModel.NewImage != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await viewModel.NewImage.CopyToAsync(memoryStream);
                    airline.Image = memoryStream.ToArray();
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(airline);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AirlineExists(airline.AirlineName))
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
            return View(airline);
        }

        // GET: Airline/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airline = await _context.Airlines
                .FirstOrDefaultAsync(m => m.AirlineName == id);
            if (airline == null)
            {
                return NotFound();
            }

            return View(airline);
        }

        // POST: Airline/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var airline = await _context.Airlines.FindAsync(id);
            _context.Airlines.Remove(airline);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AirlineExists(string id)
        {
            return _context.Airlines.Any(e => e.AirlineName == id);
        }
    }
}
