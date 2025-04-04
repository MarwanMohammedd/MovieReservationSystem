using Microsoft.AspNetCore.Mvc;
using MovieReservationSystem.DataAccess.Repository;
using MovieReservationSystem.Model.Models;

namespace MovieReservationSystem.Controllers
{
    public class ReviewsController : Controller
    {
        ReviewsRepository reviewsRepository;
        public ReviewsController(ReviewsRepository reviewsRepository)
        {
            this.reviewsRepository = reviewsRepository;
        }
        public IActionResult Index()
        {
            return Content("Reviews Controller");
        }

        public IActionResult New()
        {
            return View();
        }
        public IActionResult SaveNew(Review review)
        {
            return Content("Save Review");
        }

        public IActionResult Delete(int id)
        {
            return Content("Delete Review");
        }

        public IActionResult Update(int id)
        {
            return Content("Update Review");
        }
        public IActionResult SaveUpdate(int id)
        {
            return Content("Save Review");
        }

        public IActionResult Get(int id)
        {
            return Content($"Get Review {id}");
        }

        public IActionResult GetMovieReviewsById(int id) // Movie Id
        {
            return Content("Get Movie Reviews By Id");
        }
        
    }
}
