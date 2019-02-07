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
            return View(await _context.Airlines.Include(a => a.Provider).ToListAsync());
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
