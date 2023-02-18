using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LicentaFinal.Data;
using LicentaFinal.Models;
using Microsoft.AspNetCore.Authorization;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace LicentaFinal.Controllers
{
    public class StocurisController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StocurisController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult AutocompleteNumeProdus(string term)
        {
            var results = _context.Stocuri
                            .Where(s => s.NumeProdus.Contains(term))
                            .Select(s => s.NumeProdus)
                            .Take(10)
                            .ToList();
            return Json(results);
        }

        public async Task<IActionResult> DownloadPDF(int? id)
        {
            var stoc = _context.Stocuri.Find(id);

            if (stoc == null)
            {
                return NotFound();
            }

            // creaza un fisier PDF si scrie continutul in el
            var document = new Document();
            var memoryStream = new MemoryStream();
            var writer = PdfWriter.GetInstance(document, memoryStream);
            document.Open();
            document.Add(new Paragraph($"Detalii produs: {stoc.NumeProdus}"));
            document.Add(new Paragraph($"Preț: {stoc.Pret}"));
            document.Add(new Paragraph($"Cantitate: {stoc.Cantitate}"));
            document.Close();

            // seteaza header-ul si descarca fisierul PDF
            Response.Headers.Add("Content-Disposition", "attachment; filename=stoc.pdf");
            return File(memoryStream.ToArray(), "application/pdf");
        }



        // GET: Stocuris

        [Authorize]
        public async Task<IActionResult> Index()
        {
              return View(await _context.Stocuri.ToListAsync());
        }

        // GET: Stocuris/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Stocuri == null)
            {
                return NotFound();
            }

            var stocuri = await _context.Stocuri
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stocuri == null)
            {
                return NotFound();
            }

            return View(stocuri);
        }

        // GET: Stocuris/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stocuris/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,NumeProdus,Cantitate,Pret")] Stocuri stocuri)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stocuri);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stocuri);
        }

        // GET: Stocuris/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Stocuri == null)
            {
                return NotFound();
            }

            var stocuri = await _context.Stocuri.FindAsync(id);
            if (stocuri == null)
            {
                return NotFound();
            }
            return View(stocuri);
        }

        // POST: Stocuris/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NumeProdus,Cantitate,Pret")] Stocuri stocuri)
        {
            if (id != stocuri.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stocuri);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StocuriExists(stocuri.Id))
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
            return View(stocuri);
        }

        // GET: Stocuris/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Stocuri == null)
            {
                return NotFound();
            }

            var stocuri = await _context.Stocuri
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stocuri == null)
            {
                return NotFound();
            }

            return View(stocuri);
        }

        // POST: Stocuris/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Stocuri == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Stocuri'  is null.");
            }
            var stocuri = await _context.Stocuri.FindAsync(id);
            if (stocuri != null)
            {
                _context.Stocuri.Remove(stocuri);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StocuriExists(int id)
        {
          return _context.Stocuri.Any(e => e.Id == id);
        }
    }
}
