using Microsoft.AspNetCore.Mvc;
using MarketPlace.Models;
using System.Linq;

namespace MarketPlace.Controllers
{
    public class CustomerController : Controller
    {
        private readonly MarketPlaceDbContext _db;

        public CustomerController(MarketPlaceDbContext db)
        {
            _db = db;
        }

       
        public IActionResult Index()
        {
            var customers = _db.SalesHistories
                .GroupBy(s => new { s.CustomerName, s.CustomerPhone })
                .Select(g => g.First())
                .ToList();

            return View(customers);
        }

        // Page showing saved receipts for one customer (with items table)
        public IActionResult Details(string phone)
        {
            var history = _db.SalesHistories
                .Where(s => s.CustomerPhone == phone)
                .OrderByDescending(s => s.DateTime)
                .ToList();

            return View(history);
        }

        // AJAX endpoint for autocomplete dropdown on sales page
        [HttpGet]
        public IActionResult GetUniqueCustomers()
        {
            var customers = _db.SalesHistories
                .Where(s => !string.IsNullOrEmpty(s.CustomerName))
                .Select(s => new
                {
                    Name = s.CustomerName,
                    Phone = s.CustomerPhone,
                    Address = s.CustomerAddress
                })
                .Distinct()
                .ToList();

            return Json(customers);
        }
    }
}