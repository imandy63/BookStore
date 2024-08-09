using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;
using System.IO;
using BookStore.Filters;

namespace BookStore.Areas.Admin.Controllers
{
    [AdminAuthorization]
    public class ProductController : Controller
    {
        static int pageLimit = 10;
        BookStoreContext context = new BookStoreContext();
        //
        // GET: /Admin/Product/
        public ActionResult Index(int page = 1)
        {
            List<Product> books = context.Products.OrderBy(product => product.Id).Skip((page-1)*pageLimit).Take(pageLimit).ToList();
            ViewBag.CurrentPage = page;
            ViewBag.MaxPage = context.Products.Count() != 0 ? (int)(context.Products.Count() / pageLimit) + 1 : 0;
            return View(books);
        }

        public ActionResult Create()
        {
            List<Genre> genres = context.Genres.ToList();
            ViewBag.Genres = genres;
            return View();
        }

        public ActionResult Detail(int id = 0)
        {
            if (id == 0)
            {
                return RedirectToAction("Index");
            }
            Product product = context.Products.Where(item => item.Id == id).FirstOrDefault();
            List<Genre> genres = context.Genres.ToList();
            ViewBag.Genres = genres;
            return View(product);
        }

        [HttpPost]
        public ActionResult Create(Product product, HttpPostedFileBase imageUpload)
        {
            List<Genre> genres = context.Genres.ToList();
            ViewBag.Genres = genres;
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (product.Quantity > 0)
            {
                product.IsAvailable = true;
            }
            else
            {
                product.IsAvailable = false;
            }
            if (imageUpload != null && imageUpload.ContentLength > 0)
            {
                // Kiểm tra kích thước
                if (imageUpload.ContentLength > 20000000)
                {
                    ModelState.AddModelError("Image", "Kích thước file không được lớn hơn 20MB.");
                    return View();
                }
                var allowedExtensions = new[] { ".jpg", ".png", ".jpeg" };
                var fileEx = Path.GetExtension(imageUpload.FileName).ToLower();
                if (!allowedExtensions.Contains(fileEx))
                {
                    ModelState.AddModelError("Image", "Chỉ chấp nhận hình ảnh dạng JPG, JPEG hoặc PNG.");
                    return View();
                }

                product.ImageURL = "";
                context.Products.Add(product);
                context.SaveChanges();

                Product pro = context.Products.ToList().Last();
                var fileName = pro.Id.ToString() + fileEx;
                var path = Path.Combine(Server.MapPath("~/img/products"), fileName);
                imageUpload.SaveAs(path);

                pro.ImageURL = fileName;
                context.SaveChanges();
            }
            else
            {
                product.ImageURL = "";
                context.Products.Add(product);
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Update(Product product, HttpPostedFileBase imageUpload)
        {
            List<Genre> genres = context.Genres.ToList();
            ViewBag.Genres = genres;
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (product.Quantity > 0)
            {
                product.IsAvailable = true;
            }
            else
            {
                product.IsAvailable = false;
            }
            Product updateProduct = context.Products.Where(item => item.Id == product.Id).FirstOrDefault();

            if (imageUpload != null && imageUpload.ContentLength > 0)
            {
                // Kiểm tra kích thước
                if (imageUpload.ContentLength > 20000000)
                {
                    ModelState.AddModelError("Image", "Kích thước file không được lớn hơn 20MB.");
                    return View();
                }
                var allowedExtensions = new[] { ".jpg", ".png", ".jpeg" };
                var fileEx = Path.GetExtension(imageUpload.FileName).ToLower();
                if (!allowedExtensions.Contains(fileEx))
                {
                    ModelState.AddModelError("Image", "Chỉ chấp nhận hình ảnh dạng JPG, JPEG hoặc PNG.");
                    return View();
                }

                DeleteImageFromSystem(updateProduct.ImageURL);

                var fileName = updateProduct.Id.ToString() + fileEx;
                var path = Path.Combine(Server.MapPath("~/img/products"), fileName);
                imageUpload.SaveAs(path);

                updateProduct.ImageURL = fileName;
                updateProduct.GenreId = product.GenreId;
                updateProduct.AuthorName = product.AuthorName;
                updateProduct.IsAvailable = product.IsAvailable;
                updateProduct.Price = product.Price;
                updateProduct.Quantity = product.Quantity;
                updateProduct.Name = product.Name;
                context.SaveChanges();
            }
            else
            {
                updateProduct.GenreId = product.GenreId;
                updateProduct.AuthorName = product.AuthorName;
                updateProduct.IsAvailable = product.IsAvailable;
                updateProduct.Price = product.Price;
                updateProduct.Quantity = product.Quantity;
                updateProduct.Name = product.Name;
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public void DeleteImageFromSystem(string ImageURL)
        {
            string imagePath = Server.MapPath("~/img/products/" + ImageURL);

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }

        public ActionResult Delete(int Id)
        {
            Product product = context.Products.Where(item => item.Id == Id).FirstOrDefault();

            DeleteImageFromSystem(product.ImageURL);

            string imagePath = Server.MapPath("~/img/products/" + product.ImageURL);

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            context.Products.Remove(product);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult DeleteImage(int Id)
        {
            Product product = context.Products.Where(item => item.Id == Id).FirstOrDefault();

            DeleteImageFromSystem(product.ImageURL);

            product.ImageURL = "";
            context.SaveChanges();

            List<Genre> genres = context.Genres.ToList();
            ViewBag.Genres = genres;

            return View("Detail", product);
        }
	}
}