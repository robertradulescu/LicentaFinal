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
using Microsoft.AspNetCore.Http;


namespace LicentaFinal.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult GenerateReceipt(int id)
        {
            // Interogare catre baza de date pentru a extrage datele corespunzatoare elementului cu id-ul dat
            var querry = _context.Order.Find(id);
            if (querry == null)
            {
                return NotFound();
            }
            string Unitate = querry.NumeFirma;
            string sediu = querry.Adresa;
            System.DateTime data = querry.Creat;

            string Cumparator=querry.Cumparator;
            string AdresaCumparator=querry.AdresaCumparator;

            long CnpCumparator=querry.CnpCumparator;

            string moneda=querry.Moneda;

            var order = _context.Order.Include(o => o.Items).FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            double totalPrice = 0;

            foreach (var item in order.Items)
            {
                double productTotalPrice = item.Cantitate * item.Pret;
                totalPrice += productTotalPrice;
            }



            // Creaza un nou document PDF
            Document doc = new Document();
            MemoryStream ms = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc, ms);
            doc.Open();

            // Adauga modelul de chitanta
            Image img = Image.GetInstance("ModelChitanta.jpg");
            doc.Add(img);

            // Adauga datele pe chitanta
            Font font = new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.BOLD);
            Font font2 = new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD);
            Phrase phrase1 = new Phrase($"{Unitate}", font);
            Phrase phrase2 = new Phrase($"{sediu}", font);
            Phrase phrase3 = new Phrase($"{data}", font2);
            Phrase phrase4 = new Phrase($"{Cumparator}", font);
            Phrase phrase5 = new Phrase($"{AdresaCumparator}", font);
            Phrase phrase6 = new Phrase($"{CnpCumparator}", font);
            Phrase phrase7 = new Phrase($"{totalPrice}", font);
            Phrase phrase8 = new Phrase($"{moneda}", font);


            PdfContentByte cb = writer.DirectContent;
            ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, phrase1, 135, 750, 0);
            ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, phrase2, 130, 651, 0);
            ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, phrase3, 385, 650, 0);
            ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, phrase4, 180, 614, 0);
            ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, phrase5, 130, 588, 0);
            ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, phrase6, 160, 562, 0);
            ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, phrase7, 150, 535, 0);
            ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, phrase8, 175, 535, 0);

            // Inchide documentul si creeaza fisierul PDF
            doc.Close();
            byte[] bytes = ms.ToArray();
            return File(bytes, "application/pdf", "chitanta.pdf");
        }

        [HttpGet]
        public IActionResult AutocompleteNumeProdus(string term)
        {
            var results = _context.Order
                            .Where(s => s.Serie.Contains(term) || s.Cumparator.Contains(term))
                            .Select(s => s.Serie)
                            .Take(10)
                            .ToList();
            return Json(results);
        }



        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            return View("Index", await _context.Order
      .Where(j => j.Serie.Contains(SearchPhrase) || j.Cumparator.Contains(SearchPhrase))
      .ToListAsync());

        }
        public IActionResult ChartPie()
        {
            var orders = _context.OrderItem
                            .GroupBy(o => o.NumeProdus)
                            .Select(g => new OrderItem
                            {
                                NumeProdus = g.Key,
                                Cantitate = g.Sum(x => x.Cantitate)
                            })
                            .ToList();

            return View(orders);
        }

        public IActionResult ChartColumn()
        {
            var orders = _context.OrderItem
        .GroupBy(o => o.NumeProdus)
        .Select(g => new { NumeProdus = g.Key, ValoareStoc = g.Sum(x => (decimal)(x.Cantitate * x.Pret)) })
        .ToList();


            return View(orders);
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
        public async Task<IActionResult> Create([Bind("Creat,NumeFirma,Serie,Numar,Moneda,Cumparator,Adresa,Iban,Banca,AdresaMail,Observatii,Creator,AdresaCumparator,CnpCumparator,NrInregistrareComert,Items")] Order order)
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
                document.Add(new Paragraph($"Produs: {item.NumeProdus}  Cantitate: {item.Cantitate}  Pret: {item.Cantitate} "));
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Creat,NumeFirma,Serie,Numar,Moneda,Cumparator,Adresa,Iban,Banca,AdresaMail,Observatii,Creator,AdresaCumparator,CnpCumparator,NrInregistrareComert")] Order order)
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
