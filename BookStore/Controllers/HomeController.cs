using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;
using BookStore.ViewModel;
using BookStore.Identity;
using Microsoft.AspNet.Identity;
using System.Web.Helpers;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        BookStoreContext context = new BookStoreContext();
        AppDBContext appDbContext = new AppDBContext();
        //
        // GET: /Home/
        public ActionResult Index()
        {
            ViewBag.product1 = context.Products.Where(pro => pro.GenreId != 4).Take(4).ToList();
            ViewBag.product2 = context.Products.Where(pro => pro.GenreId == 4).Take(4).ToList();
            return View();
        }

        public ActionResult ProfileUpdate()
        {
            string id = User.Identity.GetUserId();
            var appDbContext = new AppDBContext();
            AppUser user = appDbContext.Users.Where(u => u.Id == id).FirstOrDefault();
            ViewBag.user = user;
            return View();
        }

        public ActionResult Password()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ProfileUpdate(ProfileModel profile)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            string id = User.Identity.GetUserId();
            AppUser user = appDbContext.Users.Where(u => u.Id == id).FirstOrDefault();
            user.Name = profile.Name;
            user.PhoneNumber = profile.PhoneNumber;
            user.Address = profile.Address;
            appDbContext.SaveChanges();
            return RedirectToAction("Success","Message");
        }

        public ActionResult ChangePassword(PasswordModel passwordModel){
            if (!ModelState.IsValid)
            {
                return View();
            }
            string id = User.Identity.GetUserId();
            AppUser user = appDbContext.Users.Where(u => u.Id == id).FirstOrDefault();
            string passwordHash = Crypto.HashPassword(passwordModel.Password);
            user.PasswordHash = passwordHash;
            appDbContext.SaveChanges();
            return RedirectToAction("Success","Message");
        }
	}
}