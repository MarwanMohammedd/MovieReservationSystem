using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MovieReservationSystem.DataAccess.UnitOfWork;
using MovieReservationSystem.Hubs;
using MovieReservationSystem.Model.Models;
using MovieReservationSystem.ViewModels;
using System.Security.Claims;
using System.Threading.Tasks.Dataflow;

namespace MovieReservationSystem.Controllers
{
    public class ReviewController : Controller
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHubContext<CommentHub> hubContext;

        public ReviewController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IHubContext<CommentHub> hubContext)
        {

            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.hubContext = hubContext;
        }


        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] ReviewViewModel reviewViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var user = await userManager.FindByIdAsync(userid);
                    if (user is null)
                    {
                        return BadRequest("Error: The User does not exist.");
                    }


                    Movie? movie = await unitOfWork.Movie.GetItemAsync(elem => elem.ID == reviewViewModel.MovieId);
                    if (movie is not null)
                    {
                        Review review = new()
                        {
                            UserId = int.Parse(userid),
                            MovieId = reviewViewModel.MovieId,
                            Commnet = reviewViewModel.Comment,
                            Rate = reviewViewModel.Rate,
                        };
                        await unitOfWork.Review.AddAsync(review);
                        await unitOfWork.Save();

                        await hubContext.Clients.All.SendAsync("ReceiveComment", new
                        {
                            Commnet = reviewViewModel.Comment,
                            Rate = reviewViewModel.Rate,
                            Name = $"{user.FirstName} {user.LastName}",
                              UserId = int.Parse(userid),
                            MovieId = reviewViewModel.MovieId,

                        });

                        return RedirectToAction("MovieDetailPage", "Movie", new { id = reviewViewModel.MovieId });
                    }
                    return Json(false);
                }
                catch
                {
                    return Json(true);
                }
            }
            return Json(true);
        }





        [HttpPost]
        public async Task<IActionResult> DeleteComment([FromBody] ReviewViewModel reviewViewModel)
        {
            try
            {
                var userid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (User.Identity.IsAuthenticated && (User.IsInRole("Admin") ||
                   userid == reviewViewModel.UserId))
                {
                    await unitOfWork.Review.Delete(review => review.UserId == reviewViewModel.UserId && review.MovieId == reviewViewModel.MovieId);
                    await unitOfWork.Save();
                    await hubContext.Clients.All.SendAsync("DeleteComment", new
                    {
                        UserId = reviewViewModel.UserId,
                        flag = true
                    });
                }
                else
                {
                    await hubContext.Clients.All.SendAsync("DeleteComment", new
                    {

                        UserId = reviewViewModel.UserId,
                        flag = false
                    });
                }
                return RedirectToAction("MovieDetailPage", "Movie", new { id = reviewViewModel.MovieId });

            }
            catch
            {
                return View("Error");
            }
        }













        //[HttpGet]
        //public IActionResult EditComment()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> EditComment(ReviewViewModel reviewViewModel)
        //{
        //  //  Console.WriteLine($"ReviewID Received: {reviewViewModel.ReviewID}");

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {

        //            var existingReview = await unitOfWork.Review.GetItemAsync(review => review.UserId == reviewViewModel.UserId && review.MovieId == reviewViewModel.MovieId);
        //            if (existingReview == null)
        //            {
        //                return BadRequest("Error: The Review does not exist.");
        //            }


        //            existingReview.Commnet = reviewViewModel.Comment;

        //            await unitOfWork.Review.UpdateAsync(review => review.UserId == reviewViewModel.UserId &&
        //            review.MovieId == reviewViewModel.MovieId, existingReview);
        //            await unitOfWork.Save();
        //            return RedirectToAction("DisplayComment");
        //        }
        //        catch
        //        {
        //            return View("Error");
        //        }
        //    }
        //    return View(reviewViewModel);
        //}

        //// in the movie conttroller 
        //public async Task<IActionResult> TopRated()
        //{
        //    IEnumerable<Movie> movies = await unitOfWork.Movie.GetAllAsync("Review");
        //    IEnumerable<Movie> rankedMovies = movies
        //        .OrderByDescending(movie => movie.Reviews!.Any() ? movie.Reviews!.Average(review => review.Rate) : 0);
        //    return View(rankedMovies);
        //}





    }
}
