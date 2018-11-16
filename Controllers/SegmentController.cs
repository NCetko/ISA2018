using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ISA.Data;
using ISA.Models.Entities;
using ISA.Models.SegmentViewModels;
using Microsoft.AspNetCore.Authorization;

namespace ISA.Controllers
{
    [AllowAnonymous]
    public class SegmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SegmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Segment
        public IActionResult Index()
        {
            return Content("Segment controller index");
        }

        [HttpGet("/Segment/Create/{airplaneName}")]
        // GET: Segment/Create
        public IActionResult Create(string airplaneName)
        {
            ViewBag.AirplaneName = airplaneName;
            return View();
        }

        // POST: Segment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("/Segment/Create/{airplaneName}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string airplaneName, CreateViewModel viewModel)
        {
            Segment segment = new Segment
            {
                Airplane = _context.Airplanes.Find(airplaneName),
                SegmentName = viewModel.SegmentName,
                Color = viewModel.Color
            };

            if (ModelState.IsValid)
            {
                _context.Add(segment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Airplane", new { id = airplaneName });
            }

            ViewBag.AirplaneName = airplaneName;

            return View(segment);
        }

        [HttpGet("/Segment/Edit/{airplaneName}/{segmentName}")]
        public async Task<IActionResult> Edit(string airplaneName, string segmentName)
        {
            if (airplaneName == null || segmentName == null)
            {
                return NotFound();
            }
            var segment = await _context.Segments.FindAsync(airplaneName, segmentName);
            if (segment == null)
            {
                return NotFound();
            }
            ViewBag.AirplaneName = airplaneName;
            return View(new EditViewModel(segment));
        }

        // POST: Segment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("/Segment/Edit/{airplaneName}/{segmentName}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string airplaneName, string segmentName, EditViewModel viewModel)
        {
            Segment segment = _context.Segments.Find(airplaneName, segmentName);
            segment.Color = viewModel.Color;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(segment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SegmentExists(segment.AirplaneName))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Airplane", new { id = airplaneName });
            }
            ViewBag.AirplaneName = airplaneName;
            return View(new EditViewModel(segment));
        }

        // GET: Segment/Delete/5
        [HttpGet("/Segment/Delete/{airplaneName}/{segmentName}")]
        public async Task<IActionResult> Delete(string airplaneName, string segmentName)
        {
            if (airplaneName == null || segmentName == null)
            {
                return NotFound();
            }
            var segment = await _context.Segments.FindAsync(airplaneName, segmentName);
            if (segment == null)
            {
                return NotFound();
            }
            _context.Entry(segment).Reference(x => x.Airplane).Load();
            return View(segment);
        }

        // POST: Segment/Delete/5
        [HttpPost("/Segment/Delete/{airplaneName}/{segmentName}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string airplaneName, string segmentName)
        {
            var segment = await _context.Segments.FindAsync(airplaneName, segmentName);
            _context.Segments.Remove(segment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details","Airplane", new { id = airplaneName });
        }

        private bool SegmentExists(string id)
        {
            return _context.Segments.Any(e => e.AirplaneName == id);
        }
    }
}
