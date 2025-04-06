using Microsoft.AspNetCore.Mvc;

namespace MovieReservationSystem.Controllers
{
    public class TheaterController : Controller
    {
        private readonly ITheaterRepository _theaterRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TheaterController(ITheaterRepository theaterRepository, IUnitOfWork unitOfWork)
        {
            _theaterRepository = theaterRepository;
            _unitOfWork = unitOfWork;
        }

      
        public async Task<IActionResult> Index()
        {
            var theaters = await _theaterRepository.GetAllAsync();
            return View(theaters);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Theater theater)
        {
            if (!ModelState.IsValid)
                return View(theater);

            await _theaterRepository.AddAsync(theater);
            await _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var theater = await _theaterRepository.GetByIdAsync(id);
            if (theater == null)
                return NotFound();

            return View(theater);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Theater theater)
        {
            if (!ModelState.IsValid)
                return View(theater);

            await _theaterRepository.UpdateAsync(th=>th.ID==theater.ID,theater);
            await _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var theater = await _theaterRepository.GetByIdAsync(id);
            if (theater == null)
                return NotFound();

            return View(theater);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var theater = await _theaterRepository.GetByIdAsync(id);
            if (theater == null)
                return NotFound();

            await _theaterRepository.Delete(th=>th.ID==theater.ID);
            await _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var theater = await _theaterRepository.GetByIdAsync(id);
            if (theater == null)
                return NotFound();

            return View(theater);
        }
    }
}
