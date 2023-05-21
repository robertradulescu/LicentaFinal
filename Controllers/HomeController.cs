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



    }
}