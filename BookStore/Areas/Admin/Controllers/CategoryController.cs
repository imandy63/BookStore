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
    public class CategoryController : Controller
    {
        BookStoreContext context = new BookStoreContext();
        //
        // GET: /Admin/Category/
        public ActionResult Index()
        {
            List<Genre> genres = context.Genres.ToList();
            return View(genres);
        }

        [HttpPost]
        public ActionResult Create(string Name)
        {
            if (Name == "")
            {
                return View("Index");
            }
            else
            {
                Genre genre = new Genre();
                genre.Name = Name;
                context.Genres.Add(genre);
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Detail(int id = 0)
        {
            if (id == 0)
            {
                return RedirectToAction("Index");
            }
            Genre genre = context.Genres.Where(item => item.GenreId == id).FirstOrDefault();
            if (genre == null)
            {
                return RedirectToAction("Index");
            }
            return View(genre);
        }

        [HttpPost]
        public ActionResult Update(Genre genre)
        {
            Genre updateGenre = context.Genres.Where(item => item.GenreId == genre.GenreId).FirstOrDefault();
            updateGenre.Name = genre.Name;
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int Id)
        {
            Genre genre = context.Genres.Where(item => item.GenreId == Id).FirstOrDefault();
            context.Genres.Remove(genre);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
	}
}