using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MovieReservationSystem.DataAccess.UnitOfWork;
using MovieReservationSystem.Model.Models;
using MovieReservationSystem.ViewModels;

namespace MovieReservationSystem.Controllers
{
    public class MovieSchedulesController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public MovieSchedulesController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var MovieSCList = await unitOfWork.MovieSchedle.ReadAllAsync("Movie");
            var MovieSCVMList = mapper.Map<IEnumerable<MovieSchedule>, IEnumerable<MovieScheduleVM>>(MovieSCList);
            return View(MovieSCVMList);
        }
        [HttpGet]
        public async Task<IActionResult> Add( )
        {
            MovieScheduleVM movieScheduleVM = new();
            movieScheduleVM.MovieList= await unitOfWork.Movie.ReadAllAsync();
            return View(movieScheduleVM);
        }
        [HttpPost]
        public async Task<IActionResult> Add(MovieScheduleVM movieScheduleVM)
        {
            if (ModelState.IsValid)
            {
                MovieSchedule movieSC =mapper.Map<MovieSchedule>(movieScheduleVM);
                try
                {
                   await unitOfWork.MovieSchedle.AddAsync(movieSC);
                    await unitOfWork.Save();
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
                }
            }
            movieScheduleVM.MovieList = await unitOfWork.Movie.ReadAllAsync();
            return View(movieScheduleVM);

        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {

            var movieSc = await unitOfWork.MovieSchedle.GetItemAsync(e=>e.ID==id);
            var movieScheduleVM = mapper.Map<MovieSchedule, MovieScheduleVM>(movieSc);
            movieScheduleVM.MovieList = await unitOfWork.Movie.ReadAllAsync();
            return View(movieScheduleVM);

        }
        [HttpPost]
        public async Task<IActionResult> Update(MovieScheduleVM movieScheduleVM)
        {
            if (ModelState.IsValid)
            {
              
                var movieSc = mapper.Map<MovieScheduleVM, MovieSchedule>(movieScheduleVM);

                try
                {
                   await unitOfWork.MovieSchedle.UpdateAsync(e =>e.ID== movieScheduleVM.ID,movieSc);
                   await unitOfWork.Save(); 
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
                }
            }
            movieScheduleVM.MovieList = await unitOfWork.Movie.ReadAllAsync();
            return View(movieScheduleVM);
           
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await unitOfWork.MovieSchedle.Delete(e => e.ID == id);
                await unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            catch
            {

                return NotFound();
            }
        }


    }
}
