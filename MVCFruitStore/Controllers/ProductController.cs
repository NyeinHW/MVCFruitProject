using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVCFruitStore.Database;
using MVCFruitStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;


namespace MVCFruitStore.Controllers
{
    public class ProductController : Controller
    {
        protected CartContext dbcontext;
        public ProductController(CartContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ViewProducts(string name, string userid)
        {
            ViewData["name"] = name;
            TempData["userid"] = userid;
            
            var user = dbcontext.Users.Where(x => x.Id == userid).FirstOrDefault();
            
            if (user != null) ViewData["name"] = user.Name;
            else ViewData["name"] = "Guest";

            List<Product> products = new List<Product>();
            products = dbcontext.Products.ToList();
            ViewData["qty"] = dbcontext.Carts.Where(x => x.UserID == userid).Sum(x => x.Qty);
            ViewData["products"] = products;
            return View();
        }
        public IActionResult ViewProductsWithoutLogin(string sessionid)
        {
            List<Product> products = new List<Product>();
            products = dbcontext.Products.ToList();
            ViewData["products"] = products;
            ViewData["qty"] = dbcontext.Carts.Where(x => x.UserID == null).Sum(x => x.Qty);
            ViewData["sessionid"] = sessionid;
            return View();
        }
        [HttpPost]
        public IActionResult Search(string searchString, string userName, string userid)
        {
            List<Product> products = new List<Product>();

            if (!String.IsNullOrEmpty(searchString))
            {
                products = dbcontext.Products.Where(s => s.Name.Contains(searchString) || s.Description.Contains(searchString)).ToList();
            }
            ViewData["products"] = products;
            if (userName != null)
            {
                ViewData["qty"] = dbcontext.Carts.Where(x => x.UserID == userid).Count();
                ViewData["name"] = userName;
                TempData["userid"] = userid;
                return View("ViewProducts");
            }
            ViewData["qty"] = dbcontext.Carts.Where(x => x.UserID == null).Count();

            return View("ViewProductsWithoutLogin");
        }

        public IActionResult ViewReviews(string productId)
        {
            var revproduct = dbcontext.Products.Where(p => p.Id == productId).FirstOrDefault();
            ViewBag.revproduct = revproduct;

            List<Review> reviews = new List<Review>();
            reviews = dbcontext.Reviews.Where(r => r.ProductId == productId).ToList();
            if(reviews!= null) { ViewData["reviews"] = reviews; } 
            else { ViewData["empty"] = "Sorry, no reviews yet"; }

            return View();
        }

        public IActionResult WriteReview(IFormCollection form)
        {
            var ReviewerId = TempData["userid"];
            Review r = new Review();
            r.Id = Guid.NewGuid().ToString();
            r.ProductId = form["ProductId"];
            r.UserId = form["ReviewerId"];
            r.ReviewText = form["ReviewText"];

            dbcontext.Add(r);
            dbcontext.SaveChanges();

            return RedirectToAction("ViewPurchase", "MyPurchase", new { userId = r.UserId } );
        }
    }
}