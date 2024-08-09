using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;

namespace BookStore.Controllers
{
    public class ProductController : Controller
    {
        BookStoreContext context = new BookStoreContext();
        static int pageLimit = 12;

        //
        // GET: /Product/
        public ActionResult Index(int page = 1, string order = "", string search = "")
        {
            List<Product> products;
            int total = context.Products.Where(product => product.Name.Contains(search)).Count();
            ViewBag.MaxPage = total / pageLimit + 1;
            if (page > ViewBag.MaxPage)
            {
                page = 1;
            }
            switch (order)
            {
                case "new":
                    products = context.Products.Where(product => product.Name.Contains(search)).OrderByDescending(product => product.Id).Skip((page - 1) * pageLimit).Take(pageLimit).ToList();
                    break;
                case "priceDesc":
                    products = context.Products.Where(product => product.Name.Contains(search)).OrderByDescending(product => product.Price).Skip((page - 1) * pageLimit).Take(pageLimit).ToList();
                    break;
                case "priceAsc":
                    products = context.Products.Where(product => product.Name.Contains(search)).OrderBy(product => product.Price).Skip((page - 1) * pageLimit).Take(pageLimit).ToList();
                    break;
                default:
                    products = context.Products.Where(product => product.Name.Contains(search)).OrderBy(product => product.Id).Skip((page - 1) * pageLimit).Take(pageLimit).ToList();
                    break;
            }
            
            ViewBag.CurrentPage = page;

            List<Product> mostQuantityProduct = context.Products.OrderByDescending(product => product.Quantity).Take(5).ToList();
            ViewBag.Suggestions = mostQuantityProduct;
            ViewBag.Search = search;
            ViewBag.Order = order;
            ViewBag.Total = total;
            return View(products);
        }

        public ActionResult Book(int page = 1, string order = "", int genreId = 0)
        {
            List<Product> products;
            int total;
            if (page > ViewBag.MaxPage)
            {
                page = 1;
            }
            if (genreId == 4)
            {
                return RedirectToAction("Accessory");
            }
            if(genreId==0){
                total = context.Products.Where(product => product.GenreId != 4).Count();
            }
            else
            {
                total = context.Products.Where(product => product.GenreId == genreId).Count();
            }
            ViewBag.MaxPage = total / pageLimit + 1;
            
            if (genreId == 0)
            {
                switch (order)
                {
                    case "new":
                        products = context.Products.Where(product => product.GenreId != 4).OrderByDescending(product => product.Id).Skip((page - 1) * pageLimit).Take(pageLimit).ToList();
                        break;
                    case "priceDesc":
                        products = context.Products.Where(product => product.GenreId != 4).OrderByDescending(product => product.Price).Skip((page - 1) * pageLimit).Take(pageLimit).ToList();
                        break;
                    case "priceAsc":
                        products = context.Products.Where(product => product.GenreId != 4).OrderBy(product => product.Price).Skip((page - 1) * pageLimit).Take(pageLimit).ToList();
                        break;
                    default:
                        products = context.Products.Where(product => product.GenreId != 4).OrderBy(product => product.Id).Skip((page - 1) * pageLimit).Take(pageLimit).ToList();
                        break;
                }
            }
            else
            {
                switch (order)
                {
                    case "priceDesc":
                        products = context.Products.Where(product => product.GenreId == genreId).OrderByDescending(product => product.Price).Skip((page - 1) * pageLimit).Take(pageLimit).ToList();
                        break;
                    case "priceAsc":
                        products = context.Products.Where(product => product.GenreId == genreId).OrderBy(product => product.Price).Skip((page - 1) * pageLimit).Take(pageLimit).ToList();
                        break;
                    default:
                        products = context.Products.Where(product => product.GenreId == genreId).OrderBy(product => product.Id).Skip((page - 1) * pageLimit).Take(pageLimit).ToList();
                        break;
                }
            }
            List<Genre> genres = context.Genres.Where(genre => genre.GenreId != 4).ToList();
            ViewBag.Genres = genres;
            ViewBag.CurrentGenre = genreId;
            ViewBag.CurrentPage = page;
            ViewBag.Order = order;
            ViewBag.Total = total;
            return View(products);
        }

        public ActionResult Accessory(int page = 1, string order = "")
        {
            List<Product> products;
            int total = context.Products.Where(product => product.GenreId == 4).Count();
            ViewBag.MaxPage = total / pageLimit + 1;
            if (page > ViewBag.MaxPage)
            {
                page = 1;
            }
            switch (order)
            {
                case "priceDesc":
                    products = context.Products.Where(product => product.GenreId == 4).OrderByDescending(product => product.Price).Skip((page - 1) * pageLimit).Take(pageLimit).ToList();
                    break;
                case "priceAsc":
                    products = context.Products.Where(product => product.GenreId == 4).OrderBy(product => product.Price).Skip((page - 1) * pageLimit).Take(pageLimit).ToList();
                    break;
                default:
                    products = context.Products.Where(product => product.GenreId == 4).OrderBy(product => product.Id).Skip((page - 1) * pageLimit).Take(pageLimit).ToList();
                    break;
            }

            ViewBag.CurrentPage = page;
            ViewBag.Order = order;
            ViewBag.Total = total;
            return View(products);
        }

        public ActionResult Detail(int id = 0)
        {
            if (id == 0)
            {
                return RedirectToAction("Index");
            }
            Product product = context.Products.Where(pro => pro.Id == id).FirstOrDefault();
            if (product == null)
            {
                return RedirectToAction("Index");
            }
            return View(product);
        }
	}
}