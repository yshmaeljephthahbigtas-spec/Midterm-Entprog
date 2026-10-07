using Microsoft.AspNetCore.Mvc;
using Midterm_Entprog.Data;
using Midterm_Entprog.Models;

namespace Midterm_Entprog.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CartController(ApplicationDbContext db)
        {
            _db = db;
        }

        // SHOW CART
        public IActionResult Index()
        {
            var cartItems = _db.CartItems.ToList();

            ViewBag.Total = cartItems.Sum(item =>
                item.Price * item.Quantity);

            return View(cartItems);
        }

        // ADD TO CART
        public IActionResult AddToCart(int id)
        {
            var product = _db.Products.Find(id);

            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }

            var existingItem = _db.CartItems
                .FirstOrDefault(c => c.ProductId == id);

            if (existingItem != null)
            {
                existingItem.Quantity++;
                _db.CartItems.Update(existingItem);
            }
            else
            {
                var cartItem = new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = 1
                };

                _db.CartItems.Add(cartItem);
            }

            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        // UPDATE QUANTITY
        [HttpPost]
        public IActionResult UpdateQuantity(int id, int quantity)
        {
            var item = _db.CartItems.Find(id);

            if (item != null)
            {
                if (quantity < 1)
                {
                    quantity = 1;
                }

                item.Quantity = quantity;

                _db.CartItems.Update(item);
                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // REMOVE ITEM
        public IActionResult Remove(int id)
        {
            var item = _db.CartItems.Find(id);

            if (item != null)
            {
                _db.CartItems.Remove(item);
                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}