using Microsoft.AspNetCore.Mvc;
using MarketPlace.Models;
using System.Linq;
using System.Text.Json;

namespace MarketPlace.Controllers
{
    public class SalesController : Controller
    {
        private readonly MarketPlaceDbContext _context;

        public SalesController(MarketPlaceDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        [HttpPost]
        public IActionResult SaveReceipt(SalesHistory history)
        {
            
            if (history.GrandTotal > 0)
            {
                string rawWords = ConvertToWords((long)history.GrandTotal);
                history.AmountInWords = char.ToUpper(rawWords[0]) + rawWords.Substring(1) + " Naira Only";
            }
            else
            {
                history.AmountInWords = "Zero Naira Only";
            }

            
            if (string.IsNullOrEmpty(history.SalesItemsJson))
            {
                history.SalesItemsJson = "[]";
            }

           
            _context.SalesHistories.Add(history);
            _context.SaveChanges();

            // 4. Redirect to Customer List
            return RedirectToAction("Index", "Customer");
        }

        private string ConvertToWords(long number)
        {
            if (number == 0) return "zero";

            long[] divisors = { 1000000000, 1000000, 1000, 1 };
            string[] scales = { "", "million", "thousand", "" };
            string result = "";

            for (int i = 0; i < divisors.Length; i++)
            {
                if (number >= divisors[i])
                {
                    int chunk = (int)(number / divisors[i]);
                    result += ConvertChunk(chunk) + scales[i];
                    number %= divisors[i];
                }
            }
            return result.Trim();
        }

        private string ConvertChunk(int num)
        {
            string[] ones ={"","one","two","three","four","five","six","seven","eight","nine",
                           "ten","eleven","twelve","thirteen","fourteen","fifteen","sixteen",
                           "seventeen","eighteen","nineteen"};
            string[] tens ={"","","twenty","thirty","forty","fifty","sixty","seventy",
                            "eighty","ninety"};

            string result = "";

            if (num >= 100)
            {
                result += ones[num / 100] + " hundred ";
                num %= 100;
            }

            if (num >= 20)
            {
                result += tens[num / 10];
                if (num % 10 != 0)
                    result += " " + ones[num % 10];
            }
            else if (num > 0)
            {
                result += ones[num];
            }

            return result.Trim();
        }
    }
}