using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LicentaFinal.Data;
using LicentaFinal.Models;

namespace LicentaFinal.Controllers
{
    public class FurnizorisController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FurnizorisController(ApplicationDbContext context)
        {
            _context = context;
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
