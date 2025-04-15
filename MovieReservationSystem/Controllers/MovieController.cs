using System.Drawing;
using System.Drawing.Printing;
using System.Security.Policy;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MovieReservationSystem.DataAccess.UnitOfWork;
using MovieReservationSystem.Model.Models;
using MovieReservationSystem.Services;
using MovieReservationSystem.ViewModels;

namespace MovieReservationSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MovieController : Controller
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment host;
        private readonly IMapper autoMapper;

        public MovieController(IUnitOfWork unitOfWork, IWebHostEnvironment host, IMapper autoMapper)
        {
            this.unitOfWork = unitOfWork;
            this.host = host;
            this.autoMapper = autoMapper;
        }

        [AllowAnonymous]
       public async Task<IActionResult> ShowAll()
    {
        try
        {
            IEnumerable<Movie> movies = await unitOfWork.Movie.ReadAllAsync();
            movies = movies.OrderByDescending(e=>e.ID).Take(4);
            IEnumerable<MovieAllVM> movieDetailPageVMs = movies.Select(e => new MovieAllVM
            {

                ID = e.ID,
                Title = e.Title,
                ImagePath = e.ImagePath,
                Description = e.Description,
                RateAvg = unitOfWork.Review.ReadAllAsync().Result.Any(re => re.MovieId == e.ID) ? (int)unitOfWork.Review.ReadAllAsync().Result.Where(review => review.MovieId == e.ID).Average(e => e.Rate) : 0,
                TotalCount = unitOfWork.Review.ReadAllAsync().Result.Where(review => review.MovieId == e.ID).Count()

            });
            return View(movieDetailPageVMs);
        }
        catch
        {
            //Change later to home page
            return BadRequest("Error 404");
        }
    }


        [HttpGet]

        public async Task<IActionResult> Index()
        {

            var movies = await unitOfWork.Movie.GetAllAsync();
            if (movies == null)
            {
                return NotFound();
            }
            return View(movies);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Detials(int id)
        {

            Movie? movie = await unitOfWork.Movie.GetItemAsync(el => el.ID == id);
            if (movie == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        [HttpGet]
        public IActionResult Add()
        {
            MovieViewModel movieViewModel = new();
            return View(movieViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(MovieViewModel movieViewModel)
        {
            if (ModelState.IsValid)
            {

                if (movieViewModel.ImageFile != null)
                {
                    movieViewModel.ImagePath = await FilesUpload.UploadImage(host, movieViewModel.ImageFile);

                }
                Movie movie = autoMapper.Map<Movie>(movieViewModel);
                try
                {

                    await unitOfWork.Movie.AddAsync(movie);
                    await unitOfWork.Save();
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);

                }

            }
            return View(movieViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {

            var movie = await unitOfWork.Movie.GetItemAsync(e => e.ID == id);
            MovieViewModel MoviewVM = autoMapper.Map<MovieViewModel>(movie);
            return View(MoviewVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(MovieViewModel movieViewModel)
        {
            if (ModelState.IsValid)
            {


                var movie = await unitOfWork.Movie.GetItemAsync(e => e.ID == movieViewModel.ID);
                if (movie == null) return NotFound();


                if (movieViewModel.ImageFile != null)
                {
                    var filePath = Path.Combine(Path.Combine(host.WebRootPath, "Images"), movie.ImagePath);

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    movieViewModel.ImagePath = await FilesUpload.UploadImage(host, movieViewModel.ImageFile);

                }
                else
                {
                    movieViewModel.ImagePath = movie.ImagePath;
                }
                movie = autoMapper.Map<Movie>(movieViewModel);
                try
                {

                    await unitOfWork.Movie.UpdateAsync(
                        e => e.ID == movieViewModel.ID, movie);


                    await unitOfWork.Save();
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
                }
            }

            return View(movieViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {


            var movie = await unitOfWork.Movie.GetItemAsync(e => e.ID == id);

            var filePath = Path.Combine(Path.Combine(host.WebRootPath, "Images"), movie.ImagePath);

            if (!string.IsNullOrEmpty(movie.ImagePath) && System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            try
            {
                await unitOfWork.Movie.Delete(e => e.ID == id);
                await unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            catch
            {

                return NotFound();
            }
        }
        [AllowAnonymous]
        public async Task<IActionResult> MovieDetailPage(int id)
        {
            try
            {
                Movie? movie = await unitOfWork.Movie.GetItemAsync(el => el.ID == id);
                MovieDetailPageVM movieDetailPageVM = autoMapper.Map<MovieDetailPageVM>(movie);

                // Getting The Movie's Theater
                Theater? theater = await unitOfWork.Theater.GetTheaterByMovieId(id);


                

                var movies = await unitOfWork.MovieSchedle.GetAllAsync();
                movieDetailPageVM.MovieScheduleList = movies
                    .Where(el => el.MovieId == id)
                    .Select(el => el.StartTime)
                    .ToList();
                var reviews = await unitOfWork.Review.ReadAllAsync("User");
                var reviewss = reviews.Where(review => review.MovieId == id).ToList();

                movieDetailPageVM.reviews = reviewss;

                // Adding The Theater data to the View Model
                movieDetailPageVM.TheaterId = theater.ID;
                movieDetailPageVM.TheaterName = theater.Name;
                movieDetailPageVM.TheaterSeats = theater.Seats;
                movieDetailPageVM.TheaterSeatsCount = theater.SeatsCount;

                return View(movieDetailPageVM);
            }
            catch
            {

                //Change later to home page
                return RedirectToAction(nameof(ShowAll));
            }

        }




        ///----------------------------------------------------------------------------------
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> MoviePagenation(string type="MovieLastest", string? movieName = "#*&", int page = 1, int pageSize = 8)
        {
            IEnumerable<Movie>? movies = null;
            var movies1 = await unitOfWork.Movie.ReadAllAsync();
            var total = movies1.Count();
            if (type == "MovieLastest")
            {
                movies = await unitOfWork.Movie.MoviePagenationOrderBy(page, pageSize, e => e.ID);
            }
            else if (type == "MovieCommingSoon")
            {
                DateOnly date = DateOnly.FromDateTime(DateTime.Now);
                movies = await unitOfWork.Movie.MoviePagenationOrderBy(page, pageSize, e => e.ProductionYear,
                    e => e.ProductionYear > date);
                total = movies.Count();
            }
            else if (type == "MovieRealsed")
            {
                movies = await unitOfWork.Movie.MoviePagenationOrderBy(page, pageSize, e => e.ProductionYear);

            }
            else if (type == "MovieSearch")
            {
                movies = await unitOfWork.Movie.MoviePagenationOrderBy(page, pageSize, e => e.Title,
                    e => e.Title.ToLower().Contains(movieName.ToLower()));
                total = movies.Count();

            }

            int totalPages = (int)Math.Ceiling((double)total / pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = totalPages;
            ViewBag.PageSize = pageSize;
            ViewBag.Type = type;


            var re = await unitOfWork.Review.ReadAllAsync();

            IEnumerable<MovieAllVM> movieAllVMs = movies.Select(el => new MovieAllVM
            {
                ID = el.ID,
                Title = el.Title,
                ImagePath = el.ImagePath,
                RateAvg = re.Any(re => re.MovieId == el.ID) ? (int)re.Where(review => review.MovieId == el.ID).Average(e => e.Rate) : 0,
                TotalCount = re.Where(review => review.MovieId == el.ID).Count()
            });

            return View(movieAllVMs);
        }

        [AllowAnonymous]
        public async Task<IActionResult> MovieTopRated(int page = 1, int pageSize = 8)
        {
            var movies = await unitOfWork.Movie.ReadAllAsync();
            var re = await unitOfWork.Review.ReadAllAsync();

            IEnumerable<MovieAllVM> movieAllVMs = movies.Select(el => new MovieAllVM
            {
                ID = el.ID,
                Title = el.Title,
                ImagePath = el.ImagePath,
                RateAvg = re.Any(re => re.MovieId == el.ID) ? (int)re.Where(review => review.MovieId == el.ID).Average(e => e.Rate) : 0,
                TotalCount = re.Where(review => review.MovieId == el.ID).Count()
            });

            movieAllVMs = movieAllVMs.OrderByDescending(e => e.RateAvg).Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var total = movies.Count();
            int totalPages = (int)Math.Ceiling((double)total / pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = totalPages;
            ViewBag.PageSize = pageSize;
            ViewBag.Type = "MovieTopRated";

            return View("MoviePagenation", movieAllVMs);

        }
    }
}
