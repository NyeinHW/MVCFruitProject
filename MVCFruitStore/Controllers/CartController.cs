using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVCFruitStore.Database;
using MVCFruitStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace MVCFruitStore.Controllers
{
    public class CartController : Controller
    {
        protected CartContext dbcontext;
        public CartController(CartContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        public IActionResult Checkout(string userid)
        {
            string sessionId = HttpContext.Session.GetString("sessionid");
            var items = (from x in dbcontext.Carts
                         where x.UserID == userid || x.SessionId == sessionId
                         select new
                         {
                             ProductId = x.ProductId,
                             Qty = x.Qty
                         });
            try
            {
                if (items == null)
                {
                    throw new Exception("There is no items in the cart");
                }
                if (userid == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                //Adding new orders
                Order o = new Order();
                string oid = Guid.NewGuid().ToString();
                o.Id = oid;
                o.UserId = userid;
                o.DateandTime = DateTime.Now;
                dbcontext.Add(o);

                foreach (var item in items)
                {
                    int x = item.Qty;
                    while (x > 0)
                    {
                        Order_Detail od = new Order_Detail();
                        od.OrderId = oid;
                        od.ActivationCode = Guid.NewGuid().ToString();
                        od.ProductId = item.ProductId;
                        dbcontext.Add(od);
                        x--;
                    }
                }
                //End of updating orders & order_details
                //remove items from cart upon sucessful transaction of checkout
                List<Cart> itr = dbcontext.Carts.Where(x => x.UserID == userid).ToList();

                foreach (Cart item in itr)
                {
                    dbcontext.Carts.Remove(item);
                }
                dbcontext.SaveChanges();
            }
            catch (Exception)
            {
                if (userid == null)
                {
                    RedirectToAction("ViewProductsWithoutLogin", "Product", new { sessionid = sessionId });
                }
                RedirectToAction("ViewProducts", "Product", new { userId = userid });
            }
            return RedirectToAction("ViewPurchase", "Mypurchase", new { userId = userid });
        }

        public IActionResult TransferCart(string userId, string sessionId)
        {
            //replace items in non-user to user in cart
            //string sessionid = HttpContext.Session.GetString("sessionid");
            var item = dbcontext.Carts.Where(x => x.SessionId == sessionId).ToList();
            foreach (var itr in item)
            {
                Cart c = new Cart();
                c.ProductId = itr.ProductId;
                c.UserID = userId;
                c.Qty = itr.Qty;
                c.CartId = Guid.NewGuid().ToString();
                dbcontext.Add(c);
                dbcontext.SaveChanges();
            }

            foreach (Cart itr in item)
            {
                dbcontext.Carts.Remove(itr);
            }
            dbcontext.SaveChanges();
            HttpContext.Session.Remove("sessionid");
            HttpContext.Session.SetString("userid", userId);
            return RedirectToAction("ViewCart", "Cart", new { userid = userId });
        }

        public IActionResult ViewCart(string? userid, string? sessionid)
        {
            var carts = (from c in dbcontext.Carts
                         where c.UserID == userid && c.SessionId == sessionid
                         select new
                         {
                             ProductId = c.CartProduct.Id,
                             ProductDesc = c.CartProduct.Description,
                             ProductImage = c.CartProduct.Image,
                             ProductName = c.CartProduct.Name,
                             ProductPrice = c.CartProduct.Price,
                             ProductQty = c.Qty
                         }
                        );

            List<Item> items = new List<Item>();
            foreach (var c in carts)
            {
                Item i = new Item();
                i.ProductName = c.ProductName;
                i.ProductId = c.ProductId;
                i.ProductDesc = c.ProductDesc;
                i.ProductImage = c.ProductImage;
                i.ProductPrice = c.ProductPrice;
                i.ProductQty = c.ProductQty;
                items.Add(i);

            }
            string route = "";

            if (userid != null)
            {
                string uname = HttpContext.Session.GetString("uname");
                route = "Product/ViewProducts?name=" + uname + "&" + "userid=" + userid;
            }
            else
            {
                route = "?SessionId=" + sessionid;
            }
            //string name,string userid
            TempData["userid"] = userid;
            ViewData["Route"] = route;
            ViewData["items"] = items;
            return View();
        }

        public string userAuthentication()
        {
            string userid = HttpContext.Session.GetString("userid");           
            //To check if there is existing userid stored in the cart
            if (userid == null)
            {
                string sessionid = HttpContext.Session.GetString("sessionid");
                //To check if there is sessionid store in HttpContext.Session ["sessionid"]
                //if not exist, create new sessionid

                if (sessionid != null) return sessionid;
                //return existing created sessionid
                else
                {
                    sessionid = Guid.NewGuid().ToString();
                    HttpContext.Session.SetString("sessionid", sessionid);
                    return sessionid;
                    //return new sessionid 
                }
            }
            else return userid;
        }
        public IActionResult AddToCart(string productid, string _userid)
        {
            string useraud = userAuthentication();
            if (_userid != null)
            {
                var User = dbcontext.Users.Where(x => x.Id == _userid).FirstOrDefault();
                AddItem(_userid, null, productid);
                return RedirectToAction("ViewProducts", "Product", new { name = User.Name, userid = _userid } /* new { name = User.Name, userid = _userid, sessionId = User.SessionId }*/);              
            }

            else
            {
                string nonuser = HttpContext.Session.GetString("sessionid");
                AddItem(null, nonuser, productid);
                return RedirectToAction("ViewProductsWithoutLogin", "Product", new { sessionId = nonuser });
            }   
        }

        public void AddItem(string? userid, string? sessionid, string productid)
        {
            var cartItem = dbcontext.Carts
                            .Where(x => x.UserID == userid && x.SessionId == sessionid && x.ProductId == productid)
                            .FirstOrDefault();
            //if there is no cart, create new cart
            if (cartItem == null)
            {
                cartItem = new Cart()
                {
                    CartId = Guid.NewGuid().ToString(),
                    UserID = userid,
                    SessionId = sessionid,
                    ProductId = productid,
                    CartProduct = dbcontext.Products.SingleOrDefault(p => p.Id == productid),
                    Qty = 1
                };
                dbcontext.Carts.Add(cartItem);
            }
            //if there is existing item in the cart, add quantity
            else cartItem.Qty++;
            dbcontext.SaveChanges();
        }

        public IActionResult UpdateQty(IFormCollection quantityform)
        {
            int newqty = Convert.ToInt32(quantityform["newQty"]);
            string productid = quantityform["productid"];
            string user = HttpContext.Session.GetString("userid");
            string nonuser = HttpContext.Session.GetString("sessionid");
            
            var cartItem = dbcontext.Carts
                .Where(x => x.UserID == user && x.SessionId == nonuser && x.ProductId == productid)
                .FirstOrDefault();

            if (cartItem != null)
            {
                if (newqty > 0)
                {
                    cartItem.Qty = newqty;
                }
                else
                {
                    RemoveItem(user, nonuser, productid);
                }
            }
            dbcontext.SaveChanges();

            //nonuser
            if (nonuser != null)
            {
                return RedirectToAction("ViewCart", "Cart", new { sessionid = nonuser });
            }
            //user
            return RedirectToAction("ViewCart", "Cart", new { userid = user });
            
        }
        public void RemoveItem(string userid, string sessionid, string productid)
        {
            var cartItem = dbcontext.Carts
                .Where(x => x.UserID == userid && x.SessionId == sessionid && x.ProductId == productid)
                .FirstOrDefault();
            if (cartItem != null)
            {
                dbcontext.Carts.Remove(cartItem);
            }
        }
    }
}


