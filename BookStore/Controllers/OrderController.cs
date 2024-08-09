using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Filters;
using BookStore.Identity;
using Microsoft.AspNet.Identity;
using BookStore.Models;

namespace BookStore.Controllers
{
    [AuthenFilter]
    public class OrderController : Controller
    {
        BookStoreContext context = new BookStoreContext();
        AppDBContext appContext = new AppDBContext();
        //
        // GET: /Cart/
        public ActionResult Index(int Quantity = -1)
        {
            string id = User.Identity.GetUserId();
            List<Order> orders = appContext.Users.Where(user => user.Id == id).SelectMany(user => user.Orders).ToList();
            ViewBag.quantity = Quantity;
            return View(orders);
        }

        public ActionResult Detail(int id = 0)
        {
            if (id == 0)
            {
                return RedirectToAction("Index","Home");
            }
            string userId = User.Identity.GetUserId();
            Order order = appContext.Users.Where(user => user.Id == userId).SelectMany(user => user.Orders).FirstOrDefault(ord => ord.OrderId == id);
            if (order == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Id = order.OrderId;
            ViewBag.Status = order.Status;
            List<OrderDetail> orderDetails;
            orderDetails = context.OrderDetails.Where(detail => detail.OrderId == order.OrderId).ToList();
            if (orderDetails == null)
            {
                return RedirectToAction("Index","Home");
            }
            return View(orderDetails);
        }

        public ActionResult History()
        {
            string userId = User.Identity.GetUserId();
            List<Order> orders = appContext.Users.Where(user => user.Id == userId).SelectMany(user => user.Orders).Where(ord => ord.Status != "Mới").ToList();
            List<OrderDetail> orderDetails = context.OrderDetails.ToList();
            ViewBag.OrderDetails = orderDetails;
            return View(orders);
        }

        public ActionResult Cart()
        {
            string userId = User.Identity.GetUserId();
            Order order = appContext.Users.Where(user => user.Id == userId).SelectMany(user => user.Orders).FirstOrDefault(odr => odr.Status == "Mới");
            if (order == null)
            {
                return View(new List<OrderDetail>());
            }
            List<OrderDetail> orderDetails;
            orderDetails = context.OrderDetails.Where(detail => detail.OrderId == order.OrderId).ToList();
            if (orderDetails == null)
            {
                orderDetails = new List<OrderDetail>();
            }
            return View(orderDetails);
        }

        [HttpPost]
        public ActionResult AddProduct(int Id, int Quantity)
        {
            string userId = User.Identity.GetUserId();
            Order order = appContext.Users.Where(user => user.Id == userId).SelectMany(user => user.Orders).FirstOrDefault(odr => odr.Status == "Mới");
            if (order == null)
            {
                AppUser currentUser = appContext.Users.Where(user => user.Id == userId).FirstOrDefault();
                if (currentUser != null)
                {
                    Order newOrder = new Order();
                    newOrder.Status = "Mới";
                    newOrder.OrderDate = DateTime.Now;
                    currentUser.Orders.Add(newOrder);
                    appContext.SaveChanges();
                }
                order = context.Orders.Where(ord => ord.Status == "Mới").ToList().Last();
            }
            OrderDetail od = context.OrderDetails.Where(ord => ord.OrderId == order.OrderId && ord.ProductId == Id).FirstOrDefault();
            if (od == null)
            {
                OrderDetail detail = new OrderDetail();
                detail.OrderId = order.OrderId;
                detail.ProductId = Id;
                detail.Quantity = Quantity;
                context.OrderDetails.Add(detail);
            }
            else
            {
                od.Quantity += Quantity;
            }

            context.SaveChanges();
            OrderDetail ordetl= context.OrderDetails.ToList().Last();
            Product pro = context.Products.Where(product => product.Id == ordetl.ProductId).FirstOrDefault();
            if (pro != null)
            {
                pro.Quantity -= Quantity;
                if (pro.Quantity == 0)
                {
                    pro.IsAvailable = false;
                }
            }
            context.SaveChanges();
            return RedirectToAction("Index", "Product");
        }

        [HttpPost]
        public ActionResult ConfirmOrder()
        {
            string userId = User.Identity.GetUserId();
            Order order = appContext.Users.Where(user => user.Id == userId).SelectMany(user => user.Orders).FirstOrDefault(odr => odr.Status == "Mới");
            order.Status = "Chưa xác nhận";
            appContext.SaveChanges();
            return RedirectToAction("Success", "Message");
        }

        [HttpPost]
        public ActionResult CancelOrder(int id)
        {
            string userId = User.Identity.GetUserId();
            Order order = appContext.Users.Where(user => user.Id == userId).SelectMany(user => user.Orders).FirstOrDefault(odr => odr.OrderId == id);
            if (order == null)
            {
                return RedirectToAction("Fail", "Message");
            }
            List<OrderDetail> orderDetails = context.OrderDetails.Where(ord => ord.OrderId == id).ToList();
            foreach (OrderDetail ord in orderDetails)
            {
                ord.Product.Quantity += ord.Quantity;
            }
            context.SaveChanges();
            order.Status = "Đã hủy";
            appContext.SaveChanges();
            return RedirectToAction("Success", "Message");
        }

        [HttpPost]
        public ActionResult DeleteOrderDetail(int Id)
        {
            OrderDetail orderDetail = context.OrderDetails.Where(od => od.Id == Id).FirstOrDefault();
            orderDetail.Product.Quantity += orderDetail.Quantity;
            context.OrderDetails.Remove(orderDetail);
            context.SaveChanges();
            return RedirectToAction("Detail");
        }
    }
}