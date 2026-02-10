using System.Data;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;
using Vidly.Data;
using Vidly.Dtos;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public ActionResult Random()
        {
            var movie = new Movie() { Name = "Shrek!"};
            var Customers = new List<Customer>
            {
                new Customer { Name = "Hem Raj" },
                new Customer { Name = "Hemant"}
            };
            var ViewModel = new RandomMovieViewModel
            {
                Movie =movie,
                Customers = Customers
            }; 
            ViewData["Movie"] = movie;

            return View(ViewModel);
        }

        public ViewResult Index()
        {
            return View();
        }
        public ActionResult ByReleaseDate(int year, int month)
        {
            return Content(year + "/" + month);
        }

        public ActionResult New()
        {
            var viewModel = new MovieFormViewModel
            {
                Genres = _context.Genre.ToList(),           // Load all genres for dropdown
                Movie = new Movie()                           // Empty movie for form
            };

            return View("MovieForm", viewModel);             // Reuse form view (or create separate if preferred)
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                // ← put breakpoint here or log
                Console.WriteLine("ModelState invalid: " + string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                // or better: use ILogger
                var viewModel = new MovieFormViewModel { Movie = movie, Genres = _context.Genre.ToList() };
                return View("MovieForm", viewModel);
            }

            Console.WriteLine($"Saving movie: {movie.Name}, GenreId={movie.GenreId}"); // ← see this?

            movie.DateAdded = DateTime.Now;
            _context.Movies.Add(movie);

            try
            {
                _context.SaveChanges();
                Console.WriteLine("SaveChanges succeeded");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Save error: " + ex.Message);
                ModelState.AddModelError("", "Database error: " + ex.Message);
                return View("MovieForm", new MovieFormViewModel { Movie = movie, Genres = _context.Genre.ToList() });
            }

            return RedirectToAction("Index", "Movies");
        }
    }
}