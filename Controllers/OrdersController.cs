﻿using System;
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
using Org.BouncyCastle.Asn1.X509;
using Microsoft.AspNetCore.Identity;
using LicentaFinal.Areas.Identity.Data;

namespace LicentaFinal.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

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
        public IActionResult AutocompleteProductName(string term)
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
            var currentUser = await _userManager.GetUserAsync(User);
            var orders = await _context.Order
                .Where(o => o.Creator == currentUser.UserName)
                .Include(t => t.Items)
                .ToListAsync();
            return View(orders);
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

        public async Task<IActionResult> DownloadInvoice(int? id)
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
            // Creare document PDF
            var document = new Document();
            var memoryStream = new MemoryStream();
            var writer = PdfWriter.GetInstance(document, memoryStream);
            document.Open();

            // Adăugare logo și titlu
            var logo = Image.GetInstance("Logo.png");
            logo.ScalePercent(30f);
            logo.Alignment = Element.ALIGN_CENTER;
            document.Add(logo);
            document.Add(new Paragraph("Factura", new Font(Font.FontFamily.HELVETICA, 22, Font.BOLD)));
            document.Add(new Chunk("\n\n"));

            // Adăugare detalii factură
            var facturaTable = new PdfPTable(2);
            facturaTable.WidthPercentage = 100;
            facturaTable.SpacingAfter = 20;

            // Stilizare celule tabel
            var cellStyle = new PdfPCell();
            cellStyle.Padding = 8;
            cellStyle.Border = 0;
            cellStyle.BorderWidthBottom = 1;

            // Adăugare celule tabel
            facturaTable.AddCell(new PdfPCell(new Phrase("Data factura:", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD))) { Border = 0, Padding = 8 });
            facturaTable.AddCell(new PdfPCell(new Phrase($"{ord.Creat.ToShortDateString()}", new Font(Font.FontFamily.HELVETICA, 10))) { Border = 0, Padding = 8 });

            facturaTable.AddCell(cellStyle);
            facturaTable.AddCell(cellStyle);

            facturaTable.AddCell(new PdfPCell(new Phrase("Serie:", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD))) { Border = 0, Padding = 8 });
            facturaTable.AddCell(new PdfPCell(new Phrase($"{ord.Serie}", new Font(Font.FontFamily.HELVETICA, 10))) { Border = 0, Padding = 8 });

            facturaTable.AddCell(new PdfPCell(new Phrase("Numar:", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD))) { Border = 0, Padding = 8 });
            facturaTable.AddCell(new PdfPCell(new Phrase($"{ord.Numar}", new Font(Font.FontFamily.HELVETICA, 10))) { Border = 0, Padding = 8 });

            facturaTable.AddCell(new PdfPCell(new Phrase("Moneda:", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD))) { Border = 0, Padding = 8 });
            facturaTable.AddCell(new PdfPCell(new Phrase($"{ord.Moneda}", new Font(Font.FontFamily.HELVETICA, 10))) { Border = 0, Padding = 8 });

            facturaTable.AddCell(new PdfPCell(new Phrase("Cumparator:", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD))) { Border = 0, Padding = 8 });
            facturaTable.AddCell(new PdfPCell(new Phrase($"{ord.Cumparator}", new Font(Font.FontFamily.HELVETICA, 10))) { Border = 0, Padding = 8 });

            facturaTable.AddCell(new PdfPCell(new Phrase("Adresa:", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD))) { Border = 0, Padding = 8 });
            facturaTable.AddCell(new PdfPCell(new Phrase($"{ord.Adresa}", new Font(Font.FontFamily.HELVETICA, 10))) { Border = 0, Padding = 8 });

            facturaTable.AddCell(new PdfPCell(new Phrase("IBAN:", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD))) { Border = 0, Padding = 8 });
            facturaTable.AddCell(new PdfPCell(new Phrase($"{ord.Iban}", new Font(Font.FontFamily.HELVETICA, 10))) { Border = 0, Padding = 8 });         
            
            facturaTable.AddCell(new PdfPCell(new Phrase("Banca:", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD))) { Border = 0, Padding = 8 });
            facturaTable.AddCell(new PdfPCell(new Phrase($"{ord.Banca}", new Font(Font.FontFamily.HELVETICA, 10))) { Border = 0, Padding = 8 });

            document.Add(facturaTable); ;

            // Adăugare tabel produse
            var productsTable = new PdfPTable(4);
            productsTable.WidthPercentage = 100;
            productsTable.SpacingAfter = 20;
            productsTable.SetWidths(new float[] { 2f, 1f, 1f, 1f });

            // Add table headers
            var produsHeader = new PdfPCell(new Phrase("Produs", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
            produsHeader.HorizontalAlignment = Element.ALIGN_CENTER;
            produsHeader.PaddingBottom = 10;
            produsHeader.BackgroundColor = new BaseColor(230, 230, 230);
            productsTable.AddCell(produsHeader);

            var pretHeader = new PdfPCell(new Phrase("Pret unitar", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
            pretHeader.HorizontalAlignment = Element.ALIGN_CENTER;
            pretHeader.PaddingBottom = 10;
            pretHeader.BackgroundColor = new BaseColor(230, 230, 230);
            productsTable.AddCell(pretHeader);

            var cantitateHeader = new PdfPCell(new Phrase("Cantitate", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
            cantitateHeader.HorizontalAlignment = Element.ALIGN_CENTER;
            cantitateHeader.PaddingBottom = 10;
            cantitateHeader.BackgroundColor = new BaseColor(230, 230, 230);
            productsTable.AddCell(cantitateHeader);

            var totalHeader = new PdfPCell(new Phrase("Total", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
            totalHeader.HorizontalAlignment = Element.ALIGN_CENTER;
            totalHeader.PaddingBottom = 10;
            totalHeader.BackgroundColor = new BaseColor(230, 230, 230);
            productsTable.AddCell(totalHeader);

            // Add table rows
            foreach (var item in querry.Items)
            {
                var produsCell = new PdfPCell(new Phrase(item.NumeProdus, new Font(Font.FontFamily.HELVETICA, 10)));
                produsCell.HorizontalAlignment = Element.ALIGN_LEFT;
                productsTable.AddCell(produsCell);

                var pretCell = new PdfPCell(new Phrase(item.Pret.ToString("0.00"), new Font(Font.FontFamily.HELVETICA, 10)));
                pretCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                productsTable.AddCell(pretCell);

                var cantitateCell = new PdfPCell(new Phrase(item.Cantitate.ToString(), new Font(Font.FontFamily.HELVETICA, 10)));
                cantitateCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                productsTable.AddCell(cantitateCell);

                var totalValue = item.ValoareStoc.ToString("0.00"); // convertim valoarea numerica la un șir de caractere
                var currency = ord.Moneda.ToString(); // convertim valoarea variabilei ord.Moneda la un șir de caractere

                var totalCell = new PdfPCell(new Phrase(totalValue + " " + currency, new Font(Font.FontFamily.HELVETICA, 10)));
                totalCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                productsTable.AddCell(totalCell);

            }
            // Apply Bootstrap styling to table
            productsTable.DefaultCell.Border = Rectangle.NO_BORDER;
            productsTable.DefaultCell.PaddingTop = 10;
            productsTable.DefaultCell.PaddingBottom = 10;
            productsTable.DefaultCell.PaddingLeft = 5;
            productsTable.DefaultCell.PaddingRight = 5;


            document.Add(productsTable);
   

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
                    // Salvăm o copie a stării anterioare a modelului Order înainte de a fi actualizat
                    var oldOrder = await _context.Order.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id);

                    // Actualizăm modelul Order
                    _context.Update(order);

                    // Creăm o nouă instanță a clasei OrderInvoiceHistory
                    var orderInvoiceHistory = new OrderInvoiceHistory()
                    {
                        DateChanged = DateTime.Now,
                        OrderId = order.Id,
                        Order = order,
                        OldCompanyName = oldOrder.NumeFirma,
                        NewCompanyName = order.NumeFirma,
                        OldSeries = oldOrder.Serie,
                        NewSeries = order.Serie,
                        OldNumber = oldOrder.Numar,
                        NewNumber = order.Numar,
                        OldCurrency = oldOrder.Moneda,
                        NewCurrency = order.Moneda,
                        OldAdress = oldOrder.Adresa,
                        NewAdress = order.Adresa,
                        OldIban = oldOrder.Iban,
                        NewIban = order.Iban,
                        OldBank = oldOrder.Banca,
                        NewBank = order.Banca,
                        OldAddressMail = oldOrder.AdresaMail,
                        NewAddressMail = order.AdresaMail,
                        OldObservation = oldOrder.Observatii,
                        NewObservation = order.Observatii,
                        OldCreator = oldOrder.Creator,
                        NewCreator = order.Creator,
                        OldBuyerAddress = oldOrder.AdresaCumparator,
                        NewBuyerAddress = order.AdresaCumparator,
                        OldCnpBuyer = oldOrder.CnpCumparator,
                        NewCnpBuyer = order.CnpCumparator,
                        OldTradeRegistrationNumber = oldOrder.NrInregistrareComert,
                        NewTradeRegistrationNumber = order.NrInregistrareComert
                    };

                    // Adăugăm istoricul în baza de date
                    _context.Add(orderInvoiceHistory);

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
