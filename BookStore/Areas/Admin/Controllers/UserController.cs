using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Identity;
using BookStore.Filters;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using BookStore.Areas.Admin.Viewmodel;
using System.Web.Helpers;

namespace BookStore.Areas.Admin.Controllers
{
    [AdminAuthorization]
    public class UserController : Controller
    {
        AppDBContext appContext = new AppDBContext();
        // GET: /Admin/User/
        public ActionResult Index()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new AppDBContext()));
            var appUserStore = new AppUserStore(appContext);
            var userManager = new AppUserManager(appUserStore);
            List<IdentityRole> roles = roleManager.Roles.ToList();
            var users = appContext.Users.SelectMany(u => u.Roles, (user, role) => new { user.UserName, user.Name, user.Address, role.RoleId, user.Id, user.PhoneNumber }).ToList();
            var allRoles = roleManager.Roles.ToList();
            ViewBag.Roles = allRoles;

            return View(users);
        }

        public ActionResult Create()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new AppDBContext()));
            var allRoles = roleManager.Roles.OrderByDescending(role => role.Name).ToList();
            ViewBag.Roles = allRoles;
            return View();
        }

        public ActionResult Detail(string Id = "")
        {
            if (Id == "")
            {
                return RedirectToAction("Index");
            }
            var users = appContext.Users.Where(user => user.Id == Id).SelectMany(u => u.Roles, (user, role) => new { user.Id, user.Name, user.Address, role.RoleId, user.PhoneNumber }).FirstOrDefault();
            if (users == null)
            {
                return RedirectToAction("Index");
            }
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new AppDBContext()));
            var allRoles = roleManager.Roles.OrderByDescending(role => role.Name).ToList();
            ViewBag.Roles = allRoles;
            return View(users);
        }

        [HttpPost]
        public ActionResult Create(UserViewModel viewUser)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new AppDBContext()));
            var allRoles = roleManager.Roles.OrderByDescending(role => role.Name).ToList();
            ViewBag.Roles = allRoles;
            if (!roleManager.RoleExists(viewUser.Role))
            {
                ModelState.AddModelError("Role", "Lỗi role");
                return View();
            }
            var appUserStore = new AppUserStore(appContext);
            var userManager = new AppUserManager(appUserStore);
            string passwordHash = Crypto.HashPassword(viewUser.Password);
            AppUser user = new AppUser();
            user.Name = viewUser.Name;
            user.PasswordHash = passwordHash;
            user.UserName = viewUser.Username;
            user.Address = viewUser.Address;
            user.PhoneNumber = viewUser.PhoneNumber;
            IdentityResult identityResult = userManager.Create(user);
            userManager.AddToRole(user.Id, viewUser.Role);
            return RedirectToAction("Index");
        }

        public ActionResult UpdatePassword(string Id = "")
        {
            if (Id == "")
            {
                return RedirectToAction("Index");
            }
            var appUserStore = new AppUserStore(appContext);
            var userManager = new AppUserManager(appUserStore);
            AppUser appUser = appContext.Users.Where(user => user.Id == Id).FirstOrDefault();
            ViewBag.user = appUser;
            return View();
        }

        [HttpPost]
        public ActionResult UpdateUser(string UserId, string Name, string Address, string Role, string PhoneNumber)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new AppDBContext()));
            var allRoles = roleManager.Roles.OrderByDescending(role => role.Name).ToList();
            ViewBag.Roles = allRoles;
            if (!roleManager.RoleExists(Role))
            {
                ModelState.AddModelError("Role", "Lỗi role");
                return View();
            }
            var appUserStore = new AppUserStore(appContext);
            var userManager = new AppUserManager(appUserStore);
            AppUser user = appContext.Users.Where(u => u.Id == UserId).FirstOrDefault();
            user.Name = Name;
            user.Address = Address;
            user.PhoneNumber = PhoneNumber;
            foreach (var role in roleManager.Roles.ToList())
            {
                userManager.RemoveFromRoles(UserId, role.Name);

            }
            userManager.AddToRole(UserId, Role);
            appContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(string Id = "")
        {
            if (Id == "")
            {
                return RedirectToAction("Index");
            }
            var appUserStore = new AppUserStore(appContext);
            var userManager = new AppUserManager(appUserStore);
            AppUser appUser = appContext.Users.Where(user => user.Id == Id).FirstOrDefault();
            userManager.Delete(appUser);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdatePassword(string Password, string Id)
        {
            var appUserStore = new AppUserStore(appContext);
            var userManager = new AppUserManager(appUserStore);
            AppUser appUser = appContext.Users.Where(user => user.Id == Id).FirstOrDefault();
            return RedirectToAction("Index");
        }
    }
}