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
    public class FurnizorisController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FurnizorisController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult AutocompleteNumeFurnizor(string term)
        {
            var results = _context.Furnizori
                            .Where(s => s.Furnizor.Contains(term))
                            .Select(s => s.Furnizor)
                            .Take(10)
                            .ToList();
            return Json(results);
        }

        public async Task<IActionResult> DownloadPDF(int? id)
        {
            var furn = _context.Furnizori.Find(id);

            if (furn == null)
            {
                return NotFound();
            }

            // creaza un fisier PDF si scrie continutul in el
            var document = new Document();
            var memoryStream = new MemoryStream();
            var writer = PdfWriter.GetInstance(document, memoryStream);
            document.Open();
            document.Add(new Paragraph($"Nume furnizor: {furn.Furnizor}"));
            document.Add(new Paragraph($"Codul de identificare fiscală – CIF: {furn.CIF}"));
            document.Add(new Paragraph($"Cod registru comert: {furn.CodRegistruComert}"));
            document.Add(new Paragraph($"Numar telefon: {furn.NumarTelefon}"));
            document.Add(new Paragraph($"Adresa furnizor: {furn.AdresaSediu}"));

            document.Close();

            // seteaza header-ul si descarca fisierul PDF
            Response.Headers.Add("Content-Disposition", "attachment; filename=stoc.pdf");
            return File(memoryStream.ToArray(), "application/pdf");
        }



        // GET: Furnizoris
        public async Task<IActionResult> Index()
        {
              return View(await _context.Furnizori.ToListAsync());
        }

        // GET: Furnizoris/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Furnizori == null)
            {
                return NotFound();
            }

            var furnizori = await _context.Furnizori
                .FirstOrDefaultAsync(m => m.Id == id);
            if (furnizori == null)
            {
                return NotFound();
            }

            return View(furnizori);
        }

        // GET: Furnizoris/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Furnizoris/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Furnizor,CIF,CodRegistruComert,NumarTelefon,AdresaSediu")] Furnizori furnizori)
        {
            if (ModelState.IsValid)
            {
                _context.Add(furnizori);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(furnizori);
        }

        // GET: Furnizoris/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Furnizori == null)
            {
                return NotFound();
            }

            var furnizori = await _context.Furnizori.FindAsync(id);
            if (furnizori == null)
            {
                return NotFound();
            }
            return View(furnizori);
        }

        // POST: Furnizoris/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Furnizor,CIF,CodRegistruComert,NumarTelefon,AdresaSediu")] Furnizori furnizori)
        {
            if (id != furnizori.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(furnizori);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FurnizoriExists(furnizori.Id))
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
            return View(furnizori);
        }

        // GET: Furnizoris/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Furnizori == null)
            {
                return NotFound();
            }

            var furnizori = await _context.Furnizori
                .FirstOrDefaultAsync(m => m.Id == id);
            if (furnizori == null)
            {
                return NotFound();
            }

            return View(furnizori);
        }

        // POST: Furnizoris/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Furnizori == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Furnizori'  is null.");
            }
            var furnizori = await _context.Furnizori.FindAsync(id);
            if (furnizori != null)
            {
                _context.Furnizori.Remove(furnizori);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FurnizoriExists(int id)
        {
          return _context.Furnizori.Any(e => e.Id == id);
        }
    }
}
