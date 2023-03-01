using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LicentaFinal.Data;
using LicentaFinal.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using static iTextSharp.text.pdf.AcroFields;

namespace LicentaFinal.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            return View(await _context.Order.Include(t=>t.Items).ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.Include(t => t.Items)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            var model = new Order();
            model.Items.Add(new OrderItem());
            return View(model);
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Creat,Serie,Numar,Moneda,Cumparator,Adresa,Iban,Banca,AdresaMail,Observatii,Items")] Order order)
        {
            if (ModelState.IsValid)
            {
                order.Creat = DateTime.UtcNow;
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddOrderItem([Bind("Items")] Order order)
        {
            order.Items.Add(new OrderItem());
            return PartialView("OrderItems", order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        public async Task<IActionResult> DownloadPDF(int? id)
        {

            var ord = await _context.Order.FindAsync(id);

            if (ord == null)
            {
                return NotFound();
            }

            var querry = _context.Order.Include(o => o.Items).FirstOrDefault(o => o.Id == id);


            if (ord == null)
            {
                return NotFound();
            }

            // creaza un fisier PDF si scrie continutul in el
            var document = new Document();
            var memoryStream = new MemoryStream();
            var writer = PdfWriter.GetInstance(document, memoryStream);
            document.Open();
            document.Add(new Paragraph($"Data factura: {ord.Creat}"));
            document.Add(new Paragraph($"Serie: {ord.Serie}"));
            document.Add(new Paragraph($"Numar: {ord.Numar}"));
            document.Add(new Paragraph($"Moneda: {ord.Moneda}"));
            document.Add(new Paragraph($"Cumparator: {ord.Cumparator}"));
            document.Add(new Paragraph($"Adresa: {ord.Adresa}"));
            document.Add(new Paragraph($"Iban: {ord.Iban}"));
            document.Add(new Paragraph($"Banca: {ord.Banca}"));
            document.Add(new Paragraph($"AdresaMail: {ord.AdresaMail}"));
            document.Add(new Paragraph($"Observatii: {ord.Observatii}"));

            foreach (var item in querry.Items)
            {
                document.Add(new Paragraph($"Produs: {item.NumeProdus}  Cantitate: {item.Cantitate}"));
            }

            document.Close();

            // seteaza header-ul si descarca fisierul PDF
            Response.Headers.Add("Content-Disposition", "attachment; filename=Factura.pdf");
            return File(memoryStream.ToArray(), "application/pdf");
        }


        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Creat,Serie,Numar,Moneda,Cumparator,Adresa,Iban,Banca,AdresaMail,Observatii")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.Include(t => t.Items)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Order.Include(t => t.Items).FirstOrDefaultAsync(t => t.Id == id);
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool OrderExists(int id)
        {
          return _context.Order.Any(e => e.Id == id);
        }
    }
}
