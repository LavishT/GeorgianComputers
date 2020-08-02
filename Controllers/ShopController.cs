using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeorgianConnects.Models;
using Microsoft.AspNetCore.Mvc;

namespace GeorgianConnects.Controllers
{
    public class ShopController : Controller
    {
        private readonly LavishComputersContext _context;

        public ShopController(LavishComputersContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var categories = _context.Category.OrderBy(c => c.Name).ToList();
            return View(categories);
        }

        public IActionResult Browse(string category)
        {
            ViewBag.Category = category;

            var products = _context.Product.Where(p => p.Category.Name == category).OrderBy(p => p.Name).ToList();
            return View(products);

        }

        public IActionResult ProductDetails(string product)
        {
            var selectedproduct = _context.Product.SingleOrDefault(p => p.Name == product);
            return View(selectedproduct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(int Quantity, int ProductId)
        {
            var product = _context.Product.SingleOrDefault(p => p.ProductId == ProductId);
            var price = product.Price;
            var Cart = new Cart
            {
                ProductId = ProductId,
                Quantity = Quantity,
                Price = price,
                Username = "tempuser"
            };

            _context.Cart.Add(Cart);
            _context.SaveChanges();

            return RedirectToAction("Cart");
                
        }
    }
}
