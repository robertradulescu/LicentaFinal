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
    public class InvoiceHistoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InvoiceHistoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: InvoiceHistories
        public async Task<IActionResult> Index()
        {
            string currentUserName = User.Identity.Name;
            var invoiceHistories = await _context.InvoiceHistory.Include(o => o.Invoice)
                                                .Where(o => o.Invoice.Creator == currentUserName)
                                                .OrderByDescending(o => o.DateChanged)
                                                .ToListAsync();
            return View(invoiceHistories);
        }


        public async Task<IActionResult> Cleanup()
        {
            _context.InvoiceHistory.RemoveRange(_context.InvoiceHistory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            return View("Index", await _context.InvoiceHistory
      .Where(j => j.NewTradeRegistrationNumber.Contains(SearchPhrase) 
            
            || j.OldTradeRegistrationNumber.Contains(SearchPhrase)
            || j.NewSeries.Contains(SearchPhrase)
            || j.OldSeries.Contains(SearchPhrase)
            || j.NewCompanyName.Contains(SearchPhrase)
            || j.OldCompanyName.Contains(SearchPhrase)


      )
      .ToListAsync());

        }

        [HttpGet]
        public IActionResult AutocompleteProductName(string term)
        {
            var results = _context.InvoiceHistory
                            .Where(s => s.NewCompanyName.Contains(term))
                            .Select(s => s.NewCompanyName)
                            .Take(10)
                            .ToList();
            return Json(results);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.InvoiceHistory == null)
            {
                return NotFound();
            }

            var invoiceHistory = await _context.InvoiceHistory
                .Include(o => o.Invoice)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoiceHistory == null)
            {
                return NotFound();
            }

            return View(invoiceHistory);
        }

    }
}
