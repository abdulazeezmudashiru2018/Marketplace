using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MarketPlace.Models;

namespace MarketPlace.Controllers
{
    public class OrderController : Controller
    {
        private const decimal SHIPPING_FEE = 9.99m;
        private const decimal TAX_RATE = 0.08m;

        // Your Paystack SECRET key
        private const string PAYSTACK_SECRET_KEY = "sk_test_27ecd376aa31b4e6ff380d0418b60e558708cd7d";

        private readonly MarketPlaceDbContext _db;
        public OrderController(MarketPlaceDbContext db)
        {
            _db = db;
        }

        // GET: /Order
        public IActionResult Index()
        {
            var products = _db.Products.ToList();
            return View(products);
        }

        // POST: /Order/PlaceOrder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder([FromBody] Order order)
        {
            if (order == null || order.Items == null || !order.Items.Any())
                return BadRequest(new { ok = false, message = "Your cart is empty." });

            if (!ModelState.IsValid)
                return BadRequest(new { ok = false, errors = ModelState });

            foreach (var item in order.Items)
            {
                var product = _db.Products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product == null)
                    return BadRequest(new { ok = false, message = $"Unknown product {item.ProductId}." });

                item.Name = product.ProductName;
                item.Price = product.ProductPrice;
                if (item.Qty < 1) item.Qty = 1;
            }

            order.Subtotal = order.Items.Sum(i => i.LineTotal);
            order.Shipping = order.Items.Any() ? SHIPPING_FEE : 0m;
            order.Tax = order.Subtotal * TAX_RATE;
            order.Total = order.Subtotal + order.Shipping + order.Tax;

            if (order.Payment == "Credit Card")
            {
                if (string.IsNullOrWhiteSpace(order.PaymentReference))
                    return BadRequest(new { ok = false, message = "Missing payment reference." });

                var (verified, paidAmount) = await VerifyPaystackAsync(order.PaymentReference);
                if (!verified)
                    return BadRequest(new { ok = false, message = "Payment could not be verified." });

                var expectedKobo = (long)System.Math.Round(order.Total * 100m);
                if (paidAmount < expectedKobo)
                    return BadRequest(new { ok = false, message = "Paid amount does not match order total." });

                order.IsPaid = true;
            }
            else if (order.Payment == "Bank Transfer")
            {
                order.IsPaid = false;
            }

            _db.Orders.Add(order);
            _db.SaveChanges();

            return Json(new { ok = true, orderId = order.Id, total = "₦" + order.Total.ToString("N2") });
        }

        // ====== ADMIN: PAYMENTS ======
        // GET: /Order/Payments
        public IActionResult Payments()
        {
            var orders = _db.Orders.OrderByDescending(o => o.CreatedAt).ToList();
            return View(orders);
        }

        // GET: /Order/PaymentDetails/5
        public IActionResult PaymentDetails(int id)
        {
            var order = _db.Orders.Include(o => o.Items).FirstOrDefault(o => o.Id == id);
            if (order == null) return NotFound();
            return View(order);
        }

        // ====== ADMIN: NOTIFICATIONS (ORDERS) ======
        // GET: /Order/Notifications
        public IActionResult Notifications()
        {
            var orders = _db.Orders.OrderByDescending(o => o.CreatedAt).ToList();
            return View(orders);
        }

        // GET: /Order/OrderDetails/5
        public IActionResult OrderDetails(int id)
        {
            var order = _db.Orders.Include(o => o.Items).FirstOrDefault(o => o.Id == id);
            if (order == null) return NotFound();
            return View(order);
        }

        // POST: /Order/MarkPaid/5
        [HttpPost]
        public IActionResult MarkPaid(int id)
        {
            var order = _db.Orders.FirstOrDefault(o => o.Id == id);
            if (order == null) return NotFound();
            order.IsPaid = true;
            _db.SaveChanges();
            return RedirectToAction("PaymentDetails", new { id });
        }

        // Calls Paystack's verify endpoint with your secret key.
        private static async Task<(bool ok, long amount)> VerifyPaystackAsync(string reference)
        {
            using var http = new HttpClient();
            http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", PAYSTACK_SECRET_KEY);

            var response = await http.GetAsync(
                $"https://api.paystack.co/transaction/verify/{reference}");

            if (!response.IsSuccessStatusCode)
                return (false, 0);

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (!root.TryGetProperty("data", out var data))
                return (false, 0);

            var status = data.TryGetProperty("status", out var s) ? s.GetString() : null;
            var amount = data.TryGetProperty("amount", out var a) ? a.GetInt64() : 0;

            return (status == "success", amount);
        }
    }
}
