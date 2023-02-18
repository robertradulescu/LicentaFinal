using LicentaFinal.Data;
using LicentaFinal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient.DataClassification;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace LicentaFinal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext Context { get; }



        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context )
        {
            _logger = logger;
            Context = context;
        }

        [HttpPost]
        public JsonResult AutoComplete(string prefix) 
        {
            var stocuri= (from Stocuri in Context.Stocuri
                          
                          where Stocuri.NumeProdus.StartsWith(prefix)
                          select new
                          {
                              label= Stocuri.NumeProdus,
                              val= Stocuri.Id

                          }).ToList();
                          
            return Json(stocuri); 
        }

        [HttpGet]
        public IActionResult AutocompleteNumeProdus(string term)
        {
            var results = Context.Stocuri
                            .Where(s => s.NumeProdus.Contains(term))
                            .Select(s => s.NumeProdus)
                            .Take(10)
                            .ToList();
            return Json(results);
        }


        [HttpPost]
        public IActionResult Index(string NumeProdus, string ProdusId)
        {
            ViewBag.Message = "Nume Produs: " + NumeProdus + " Id Produs: " + ProdusId;
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}