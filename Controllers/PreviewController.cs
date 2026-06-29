using Microsoft.AspNetCore.Mvc;
using MarketPlace.Models;

namespace MarketPlace.Controllers
{
    public class PreviewController : Controller
    {
        [HttpPost]
        public IActionResult Index(SalesReceipt receipt)
        {
            // Safety check: If SalesItemsJson is null or empty, default to an empty array string
            if (string.IsNullOrEmpty(receipt.SalesItemsJson))
            {
                receipt.SalesItemsJson = "[]";
            }

            return View(receipt);
        }
    }
}