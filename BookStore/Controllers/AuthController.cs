using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.ViewModel;
using BookStore.Models;
using BookStore.Identity;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using System.Web.Helpers;

namespace BookStore.Controllers
{
    public class AuthController : Controller
    {
        BookStoreContext context = new BookStoreContext();
        //
        // GET: /Login/
        public ActionResult Login()
        {
            List<Product> products = context.Products.ToList();
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterVM vm)
        {
            if (ModelState.IsValid)
            {
                var appDbContext = new AppDBContext();
                var userStore = new AppUserStore(appDbContext);
                var userManager = new AppUserManager(userStore);
                string passwordHash = Crypto.HashPassword(vm.Password);
                var user = new AppUser();
                user.Name = vm.Name;
                user.PasswordHash = passwordHash;
                user.UserName = vm.Username;
                user.Address = vm.Address;
                user.PhoneNumber = vm.PhoneNumber;
                IdentityResult identityResult = userManager.Create(user);
                if (identityResult.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Customer");

                    var authenManager = HttpContext.GetOwinContext().Authentication;
                    var userIdentity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    authenManager.SignIn(new AuthenticationProperties(), userIdentity);
                    return RedirectToAction("Index", "Home");   
                }
                else
                {
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("Unacceptable", "Invalid Data");
                return View();
            }
        }
        [HttpPost]
        public ActionResult Login(LoginVM lvm)
        {
            if (ModelState.IsValid)
            {

                var appDbContext = new AppDBContext();
                var userStore = new AppUserStore(appDbContext);
                var userManager = new AppUserManager(userStore);
                var user = userManager.Find(lvm.Username, lvm.Password);
                if (user != null)
                {
                    var authenManager = HttpContext.GetOwinContext().Authentication;
                    var userIdentity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    authenManager.SignIn(new AuthenticationProperties(), userIdentity);
                    if (userManager.IsInRole(user.Id, "admin"))
                    {
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ViewBag.Error = "Tên tài khoản hoặc mật khẩu sai";
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("Unacceptable", "Invalid data");
                return View();
            }
        }

        [HttpPost]
        public ActionResult Logout()
        {
            var authenManager = HttpContext.GetOwinContext().Authentication;
            authenManager.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}