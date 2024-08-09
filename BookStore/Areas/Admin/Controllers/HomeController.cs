using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;
using BookStore.Identity;
using BookStore.Filters;

namespace BookStore.Areas.Admin.Controllers
{
    [AdminAuthorization]
    public class HomeController : Controller
    {
        BookStoreContext context = new BookStoreContext();
        AppDBContext dbContext = new AppDBContext();
        // GET: /Admin/Home/
        public ActionResult Index()
        {
            int countProduct = context.Products.Count();
            int countUser = dbContext.Users.Count();
            int countOrder = context.Orders.Count();
            ViewBag.totalUser = countUser;
            ViewBag.totalProduct = countProduct;
            ViewBag.TotalOrder = countOrder;
            return View();
        }
	}
}