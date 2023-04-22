﻿using System;
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
    public class OrderHistoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderHistoriesController(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            string currentUserName = User.Identity.Name;
            var orderHistories = await _context.OrderHistory.Include(o => o.OrderItem)
                                                .Where(o => o.OrderItem.Creator == currentUserName)
                                                .OrderByDescending(o => o.DateChanged)
                                                .ToListAsync();
            return View(orderHistories);
        }

        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            return View("Index", await _context.OrderHistory
      .Where(j => j.OrderItem.NumeProdus.Contains(SearchPhrase)

      )
      .ToListAsync());

        }

        public async Task<IActionResult> Cleanup()
        {
            _context.OrderHistory.RemoveRange(_context.OrderHistory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult AutocompleteProductName(string term)
        {
            var results = _context.OrderHistory
                            .Where(s => s.OrderItem.NumeProdus.Contains(term))
                            .Select(s => s.OrderItem.NumeProdus)
                            .Take(10)
                            .ToList();
            return Json(results);
        }


    }
}
