using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MovieReservationSystem.DataAccess.Data;
using MovieReservationSystem.DataAccess.UnitOfWork;
using MovieReservationSystem.Model.Models;
using MovieReservationSystem.ViewModels;
using System.Diagnostics;
using System.Security.Claims;

namespace MovieReservationSystem.Hubs
{
    public class CommentHub : Hub
    {
        
        //private readonly IUnitOfWork unitOfWork;
        //private readonly UserManager<ApplicationUser> userManager;
        //private readonly ApplicationDBContext context;

        //public CommentHub(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, ApplicationDBContext context)
        //{
        //    this.unitOfWork = unitOfWork;
        //    this.userManager = userManager;
        //    this.context = context;
        //}
        //public override Task OnDisconnectedAsync(Exception? exception)
        //{
        //    Debug.WriteLine(exception?.InnerException?.Message);
        //    return base.OnDisconnectedAsync(exception);
        //}


        //public async void AddComment(int MovieId,string Comment, int Rate)
        //{
        //    try
        //    {
        //        var userid = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        //        var user = await userManager.FindByIdAsync(userid);

        //        if (user is null)
        //        {
        //            // ("Error: The User does not exist.");
        //        }
        //        Movie? movie =await context.Movies.FirstOrDefaultAsync(e => e.ID == MovieId);
        //       // Movie? movie = await unitOfWork.Movie.GetItemAsync(elem => elem.ID == MovieId);
        //        if (movie is not null)
        //        {
        //            Review review = new()
        //            {
        //                UserId = int.Parse(userid),
        //                MovieId = MovieId,
        //                Commnet =Comment,
        //                Rate = Rate,
        //            };
        //            await  context.Reviews.AddAsync(review);
        //            await context.SaveChangesAsync();
        //         //   await unitOfWork.Review.AddAsync(review);
        //           // await unitOfWork.Save();
        //            try
        //            {
        //                await Clients.All.SendAsync("ReceiveComment", new
        //                {
        //                    UserId = int.Parse(userid),
        //                    MovieId = MovieId,
        //                    Commnet = Comment,
        //                    Rate = Rate
        //                });
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine($"Hub error: {ex.Message}");
        //                throw; // Re-throw to propagate to the client
        //            }
        //            //return RedirectToAction("MovieDetailPage", "Movie", new { id = reviewViewModel.MovieId });
        //            // return Json("comment has been added");
        //        }
        //        // return Json("The Movie is not exist");
        //    }
        //    catch
        //    {
        //        //return View("Error");
        //        //  return Json(true);

        //    }

        //}


        ////public async void AddComment(ReviewViewModel reviewViewModel)
        ////{

        ////    try
        ////    {
        ////        var userid = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        ////        var user = await userManager.FindByIdAsync(userid);

        ////        if (user is null)
        ////        {
        ////            // ("Error: The User does not exist.");
        ////        }


        ////        Movie? movie = await unitOfWork.Movie.GetItemAsync(elem => elem.ID == reviewViewModel.MovieId);
        ////        if (movie is not null)
        ////        {
        ////            Review review = new()
        ////            {
        ////                UserId = int.Parse(userid),
        ////                MovieId = reviewViewModel.MovieId,
        ////                Commnet = reviewViewModel.Comment,
        ////                Rate = reviewViewModel.Rate,
        ////            };
        ////            await unitOfWork.Review.AddAsync(review);
        ////            await unitOfWork.Save();
        ////            try
        ////            {
        ////                await Clients.All.SendAsync("ReceiveComment", new
        ////                {
        ////                    UserId = int.Parse(userid),
        ////                    MovieId = reviewViewModel.MovieId,
        ////                    Commnet = reviewViewModel.Comment,
        ////                    Rate = reviewViewModel.Rate
        ////                });
        ////            }
        ////            catch (Exception ex)
        ////            {
        ////                Console.WriteLine($"Hub error: {ex.Message}");
        ////                throw; // Re-throw to propagate to the client
        ////            }
        ////            //return RedirectToAction("MovieDetailPage", "Movie", new { id = reviewViewModel.MovieId });
        ////            // return Json("comment has been added");
        ////        }
        ////        // return Json("The Movie is not exist");
        ////    }
        ////    catch
        ////    {
        ////        //return View("Error");
        ////        //  return Json(true);

        ////    }

        ////}
    }
}
