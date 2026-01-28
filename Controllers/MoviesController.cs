using System.Data;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        public ActionResult Random()
        {
            var movie = new Movie() { Name = "Shrek!"};
            var Customers = new List<Customer>
            {
                new Customer { Name = "Hem Raj"},
                new Customer { Name = "Hemant"}
            };
            var ViewModel = new RandomMovieViewModel
            {
                Movie =movie,
                Customers = Customers
            }; 
            ViewData["Movie"] = movie;
            // ViewBag.Movie = moviename;
            // return View(movie);
            // return RedirectToAction("Index","Home",new {page = 1, sortBy = "name"});
            // return Content("hello world");

            return View(ViewModel);
        }

        public ActionResult Edit(int id)
        {
            return Content("id=" + id);
        }

        public ActionResult Index(int? pageIndex, string sortBy)
        {
            if(!pageIndex.HasValue)
                pageIndex = 1;

            if(string.IsNullOrWhiteSpace(sortBy))
                sortBy = "Name";

            return Content(string.Format("pageIndex={0}&sortBy={1}", pageIndex,sortBy)); 
        }
        public ActionResult ByReleaseDate(int year, int month)
        {
            return Content(year + "/" + month);
        }
    }
}