using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using MVCFruitStore.Models;
using MVCFruitStore.Database;
using Microsoft.AspNetCore.Http;
using System.Data;

namespace MVCFruitStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        protected CartContext dbcontext;

        public HomeController(ILogger<HomeController> logger, CartContext dbcontext)
        {
            _logger = logger;
            this.dbcontext = dbcontext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Login(string Name, string Password)
        {

            if (Name == null)
                return View();
            var user = dbcontext.Users
                                .Where(x => x.Name == Name)
                                .FirstOrDefault();
            if (user == null)
            {
                ViewData["login_error"] = "The username cannot be found.";

                return View();
            }

            using (MD5 md5Hash = MD5.Create())
            {
                string hashPwd = MD5Hash.GetMd5Hash(md5Hash, Password);


                if (user.Password != hashPwd)
                {
                    ViewData["login_error"] = "Password is wrong. Please try again.";
                    return View();
                }
            }
            //From /cart/checkout/~without login
            string sessionid = HttpContext.Session.GetString("sessionid");
            if (sessionid != null)
            {
                return RedirectToAction("TransferCart", "Cart", new { userId = user.Id, sessionId = sessionid });
            }
            HttpContext.Session.SetString("uname", user.Name);
            HttpContext.Session.SetString("userid", user.Id);
            return RedirectToAction("ViewProducts", "Product", new { name = Name, userid = user.Id });
        }

        public IActionResult Logout()
        {
            //save user cart into cart entities

            HttpContext.Session.Clear();
            dbcontext.SaveChanges();
            return RedirectToAction("ViewProductsWithoutLogin", "Product");
        }

    }
}

