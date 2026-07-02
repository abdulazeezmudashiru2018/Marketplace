using Microsoft.AspNetCore.Mvc;
using MarketPlace.Models;

namespace MarketPlace.Controllers
{
    public class AddGoodsController : Controller
    {
        private readonly MarketPlaceDbContext _context;

        public AddGoodsController(
            MarketPlaceDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var products =
                _context.Products.ToList();

            return View(products);
        }

        [HttpPost]
        public IActionResult SaveProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);

            if (product != null)
            {
                _context.Products.Remove(product);

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EditProduct(int Id, string ProductName, decimal ProductPrice)
        {
            var product = _context.Products.Find(Id);

            if (product != null)
            {
                product.ProductName = ProductName;
                product.ProductPrice = ProductPrice;

               
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }


    }
}

