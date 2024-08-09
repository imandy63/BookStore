using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;
using BookStore.Filters;

namespace BookStore.Areas.Admin.Controllers
{
    [AdminAuthorization]
    public class OrderController : Controller
    {
        static int PageLimit = 10;
        BookStoreContext context = new BookStoreContext();
        //
        // GET: /Admin/Order/
        public ActionResult Index(int page = 1)
        {
            int total = context.Orders.Where(odr => odr.Status != "Mới").Count();
            List<Order> orders = context.Orders.Where(odr => odr.Status!="Mới").OrderBy(i => i.OrderId).Skip(PageLimit * (page-1)).Take(PageLimit).ToList();
            ViewBag.CurrentPage = page;
            ViewBag.MaxPage = total != 0 ? total / PageLimit + 1 : 0;
            return View(orders);
        }


        public ActionResult Detail(int Id = 0)
        {
            if (Id == 0)
            {
                return RedirectToAction("Index");
            }
            List<OrderDetail> orderDetails = context.OrderDetails.Where(ordl => ordl.OrderId == Id).ToList();
            ViewBag.Status = context.Orders.Where(order => order.OrderId == Id).FirstOrDefault().Status;
            return View(orderDetails);
        }


        [HttpPost]
        public ActionResult AcceptOrder(int Id)
        {
            if (Id == 0)
            {
                return RedirectToAction("Index");
            }
            Order order = context.Orders.Where(ordl => ordl.OrderId == Id).FirstOrDefault();
            order.Status = "Đã xác nhận";
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DoneOrder(int Id)
        {
            if (Id == 0)
            {
                return RedirectToAction("Index");
            }
            Order order = context.Orders.Where(ordl => ordl.OrderId == Id).FirstOrDefault();
            order.Status = "Đã nhận hàng";
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RejectOrder(int Id)
        {
            if (Id == 0)
            {
                return RedirectToAction("Index");
            }
            Order order = context.Orders.Where(ordl => ordl.OrderId == Id).FirstOrDefault();
            if (order.Status != "Chưa xác nhận")
            {
                return RedirectToAction("Index");
            }
            List<OrderDetail> orderDetails = context.OrderDetails.Where(ord => ord.OrderId == Id).ToList();
            foreach (OrderDetail ord in orderDetails)
            {
                ord.Product.Quantity += ord.Quantity;
            }
            order.Status = "Đã hủy";
            context.SaveChanges();
            return RedirectToAction("Index");
        }
	}
}