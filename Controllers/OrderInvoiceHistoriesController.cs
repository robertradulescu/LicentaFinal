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
    public class OrderInvoiceHistoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderInvoiceHistoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: OrderInvoiceHistories
        public async Task<IActionResult> Index()
        {
            string currentUserName = User.Identity.Name;
            var orderHistories = await _context.OrderInvoiceHistory.Include(o => o.Order)
                                                .Where(o => o.Order.Creator == currentUserName)
                                                .OrderByDescending(o => o.DateChanged)
                                                .ToListAsync();
            return View(orderHistories);
        }


        public async Task<IActionResult> Cleanup()
        {
            _context.OrderInvoiceHistory.RemoveRange(_context.OrderInvoiceHistory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            return View("Index", await _context.OrderInvoiceHistory
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
        public IActionResult AutocompleteNumeProdus(string term)
        {
            var results = _context.OrderInvoiceHistory
                            .Where(s => s.NewCompanyName.Contains(term))
                            .Select(s => s.NewCompanyName)
                            .Take(10)
                            .ToList();
            return Json(results);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.OrderInvoiceHistory == null)
            {
                return NotFound();
            }

            var orderInvoiceHistory = await _context.OrderInvoiceHistory
                .Include(o => o.Order)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderInvoiceHistory == null)
            {
                return NotFound();
            }

            return View(orderInvoiceHistory);
        }

    }
}
