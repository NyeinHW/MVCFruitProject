using MVCFruitStore.Database;
using MVCFruitStore.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCFruitStore.Controllers
{
    public class MyPurchaseController : Controller
    {
        protected CartContext dbcontext;

        public MyPurchaseController(CartContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public IActionResult ViewPurchase(string userId)
        {

            if (userId != null)
            {   
                
                var order3 = from od in dbcontext.OrderDetails
                             join o in dbcontext.Orders on od.OrderId equals o.Id
                             where o.UserId == userId
                             group od by od.ProductId into gp
                             select new
                             {
                                 productId = gp.Key,
                                 count = gp.Count(),
                             };
  
                List<AllItems> list = new List<AllItems>();
                foreach (var i in order3)
                {
                    var p = dbcontext.Products.Where(product => product.Id == i.productId).FirstOrDefault();
                    //Take the latest dateandtime as per product
                    var d = (from od in dbcontext.OrderDetails
                             join o in dbcontext.Orders on od.OrderId equals o.Id
                             where o.UserId == userId && od.ProductId == i.productId
                             orderby o.DateandTime descending
                             select new
                             {
                                 Date = o.DateandTime
                             }).First();

                    AllItems item = new AllItems();
                    item.ProductId = i.productId;
                    item.ProductQty = i.count;
                    item.ProductImage = p.Image;
                    item.ProductDesc = p.Description;
                    item.ProductName = p.Name;
                    item.ProductPrice = p.Price;
                    item.DateAndTime = d.Date;
                    list.Add(item);
                }

                ViewData["allItems"] = list;

                List<Order_Detail> orderDetails = new List<Order_Detail>();

                var orderList2 = from od in dbcontext.OrderDetails
                                     join o in dbcontext.Orders on od.OrderId equals o.Id
                                     where o.UserId == userId
                                     select od;

                    foreach (var o in orderList2)
                    {
                        Order_Detail odele = new Order_Detail();
                        odele.ActivationCode = o.ActivationCode;
                        odele.ProductId = o.ProductId;
                        orderDetails.Add(odele);
                    }

                ViewData["activationCode"] = orderDetails;
                TempData["userid"] = userId;
                return View();              

            }

            else return RedirectToAction("Login", "Home");
        }

    }
}