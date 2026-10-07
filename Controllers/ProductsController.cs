using Microsoft.AspNetCore.Mvc;
using Midterm_Entprog.Data;
using Midterm_Entprog.Models;

namespace Midterm_Entprog.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductsController(ApplicationDbContext db)
        {
            _db = db;
        }

        // READ + SEARCH
        public IActionResult Index(string searchString)
        {
            var products = _db.Products.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p =>
                    p.Name.ToLower().Contains(searchString.ToLower()));
            }

            ViewData["searchString"] = searchString;

            return View(products.ToList());
        }

        // CREATE - show form
        public IActionResult Create()
        {
            return View();
        }

        // CREATE - save product
        [HttpPost]
        public IActionResult Create(Product product)
        {
            _db.Products.Add(product);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        // EDIT - show form
        public IActionResult Edit(int id)
        {
            var product = _db.Products.Find(id);

            if (product == null)
            {
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // EDIT - save changes
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            _db.Products.Update(product);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        // DELETE
        public IActionResult Delete(int id)
        {
            var product = _db.Products.Find(id);

            if (product != null)
            {
                _db.Products.Remove(product);
                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}