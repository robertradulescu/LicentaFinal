using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LicentaFinal.Data;
using LicentaFinal.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Identity;
using LicentaFinal.Areas.Identity.Data;

namespace LicentaFinal.Controllers
{
    public class OrderItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderItemsController(ApplicationDbContext context , UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
       

        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AutocompleteNumeProdus(string term)
        {
            var results = _context.OrderItem
                            .Where(s => s.NumeProdus.Contains(term))
                            .Select(s => s.NumeProdus)
                            .Take(10)
                            .ToList();
            return Json(results);
        }

        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            return View("Index",await _context.OrderItem.Where(j=>j.NumeProdus.Contains(SearchPhrase)).ToListAsync());
        }

        // GET: OrderItems
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var items = await _context.OrderItem
                .Where(item => item.Creator == currentUser.UserName)
                .ToListAsync();

            return View(items);
        }


        // GET: OrderItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderItem == null)
            {
                return NotFound();
            }

            return View(orderItem);
        }

        // GET: OrderItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OrderItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NumeProdus,Cantitate,Pret")] OrderItem orderItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(orderItem);
        }

        // GET: OrderItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItem.FindAsync(id);
            if (orderItem == null)
            {
                return NotFound();
            }
            return View(orderItem);
        }

        // POST: OrderItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NumeProdus,Cantitate,Pret,Creator")] OrderItem orderItem)
        {
            if (id != orderItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var oldOrderItem = await _context.OrderItem.AsNoTracking().FirstOrDefaultAsync(oi => oi.Id == orderItem.Id);

                    if (oldOrderItem.Cantitate != orderItem.Cantitate)
                    {
                        _context.OrderHistory.Add(new OrderHistory
                        {
                            DateChanged = DateTime.UtcNow,
                            OrderItemId = orderItem.Id,
                            OrderItem = orderItem,
                            OldQuantity = oldOrderItem.Cantitate,
                            NewQuantity = orderItem.Cantitate,
                            OldPrice = oldOrderItem.Pret,
                            NewPrice = orderItem.Pret
                        });
                    }
                    else if (oldOrderItem.Pret != orderItem.Pret)
                    {
                        _context.OrderHistory.Add(new OrderHistory
                        {
                            DateChanged = DateTime.UtcNow,
                            OrderItemId = orderItem.Id,
                            OrderItem = orderItem,
                            OldQuantity = oldOrderItem.Cantitate,
                            NewQuantity = orderItem.Cantitate,
                            OldPrice = oldOrderItem.Pret,
                            NewPrice = orderItem.Pret
                        });
                    }

                    _context.Update(orderItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderItemExists(orderItem.Id))
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

            return View(orderItem);
        }



        // GET: OrderItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderItem == null)
            {
                return NotFound();
            }

            return View(orderItem);
        }

        // POST: OrderItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderItem = await _context.OrderItem.FindAsync(id);
            _context.OrderItem.Remove(orderItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderItemExists(int id)
        {
          return _context.OrderItem.Any(e => e.Id == id);
        }
    }
}
