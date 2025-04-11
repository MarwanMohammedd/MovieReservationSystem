using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieReservationSystem.DataAccess.Repository;
using MovieReservationSystem.DataAccess.Repository.TheaterRepository;
using MovieReservationSystem.DataAccess.Repository.TicketRepository;
using MovieReservationSystem.DataAccess.UnitOfWork;
using MovieReservationSystem.DTO;
using MovieReservationSystem.Model;
using MovieReservationSystem.Model.Models;
using MovieReservationSystem.Model.ViewModels;
using System.Security.Claims;

namespace MovieReservationSystem.Controllers
{
    
    public class TicketController : Controller
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly TheaterRepository _theaterRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TicketController(ITicketRepository ticketRepository,IMovieRepository movieRepository
            ,TheaterRepository theaterRepository,IUnitOfWork unitOfWork)
        {
            
            _ticketRepository = ticketRepository;
            _movieRepository = movieRepository;
            _theaterRepository = theaterRepository;
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult Index(int id)
        {

            Movie? movie=_movieRepository.GetItemAsync(m => m.ID == id).Result;
            Theater? theater = _theaterRepository.GetTheaterByMovieId(id).Result;

            //if(movie == null)
            //{
            //    return NotFound("Movie not found.");
            //}
            //if (theater == null)
            //{
            //    return NotFound("Theater not found.");
            //}

            MovieTheaterViewModel movieTheater = new MovieTheaterViewModel() {
            MovieId=id,
            MovieDescription=movie?.Description??"",
            MovieDuration=movie?.Duration ?? 0,
            MovieGenre=movie?.Genre??"",
            MovieProductionYear=movie?.ProductionYear?? default(DateOnly),
            MovieTitle=movie?.Title??"",
            TheaterId=theater?.ID??0,
            TheaterName=theater?.Name??"",
            TheaterSeats=theater?.Seats?? new List<Seat>(),
            TheaterSeatsCount=theater?.SeatsCount??0
            
            };


            return View("MovieDetailsTest", movieTheater);
            // return View(movieTheater);
        }


        [Authorize]
        [HttpGet]
        public IActionResult Create(int movieId, int theaterId, string seatNumbers)
        {
            TicketBookingDto request = new TicketBookingDto() { MovieId = movieId, TheaterId = theaterId };
            request.SeatNumbers = seatNumbers.Split(',').Select(int.Parse).ToList();

            if (request.SeatNumbers == null || !request.SeatNumbers.Any())
            {
                return BadRequest("No seats selected.");
            }

            // You can map the request to your DTO or use it directly
            var bookingDto = new TicketBookingDto
            {
                UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                SeatNumbers = request.SeatNumbers,
                MovieId = request.MovieId,
                TheaterId = request.TheaterId
            };

            // TODO: Save booking or pass to service
            // return RedirectToAction("Confirmation", bookingDto); // if you want to redirect
            // return Ok(new { message = "Booking successful", data = bookingDto });
            return View(bookingDto);
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveCreate(TicketBookingDto ticketBooking)
        {
            if (ticketBooking.SeatNumbers == null || !ticketBooking.SeatNumbers.Any())
            {
                return BadRequest("No seats selected.");
            }

            IEnumerable<Ticket> existingTickets = await _ticketRepository.GetByMovieAsync(ticketBooking.MovieId);
            IEnumerable<int> alreadyReserved = existingTickets
            .Where(t => t.Seat != null &&
                ticketBooking?.SeatNumbers != null &&
                ticketBooking.SeatNumbers.Contains(t.Seat.ID) &&
                t.Seat.IsReserved)
            .Select(t => t.Seat.ID)
            .ToList();

            if (alreadyReserved.Any())
            {
                return BadRequest($"Seats already reserved: {string.Join(", ", alreadyReserved)}");
            }

            Theater? theater = await _theaterRepository.GetTheaterByMovieId(ticketBooking.MovieId);

            foreach (int seatId in ticketBooking.SeatNumbers)
            {
                Seat? seat = theater?.Seats?.FirstOrDefault(s => s.ID == seatId);
                seat.IsReserved = true;
                Ticket ticket = new Ticket
                {
                    MovieId = ticketBooking.MovieId,
                    UserId = ticketBooking.UserId,
                    PurchaseDate = DateTime.Now,
                    Price = 20,
                };

                ticket.Seat = seat;
                await _ticketRepository.AddAsync(ticket);
            }
            await _unitOfWork.Save();
            return View();
        }

        public IActionResult Update()
        {
            return View();
        }

        public IActionResult Delete()
        {
            return View();
        }

        public IActionResult Details()
        {
            return View();
        }
    }
}
