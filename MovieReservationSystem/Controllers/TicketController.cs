using Microsoft.AspNetCore.Mvc;
using MovieReservationSystem.DataAccess.Repository;
using MovieReservationSystem.DTO;
using MovieReservationSystem.Model;
using MovieReservationSystem.Model.Models;

namespace MovieReservationSystem.Controllers
{
    public class TicketController : Controller
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly ITheaterRepository _theaterRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TicketController(ITicketRepository ticketRepository,IMovieRepository movieRepository
            , ITheaterRepository theaterRepository,IUnitOfWork unitOfWork)
        {
            
            _ticketRepository = ticketRepository;
            _movieRepository = movieRepository;
            _theaterRepository = theaterRepository;
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult Index(int id)
        {

            Movie? movie=_movieRepository.GetMovieByIdAsync(id).Result;
            Theater? theater = _theaterRepository.GetTheaterByMovieId(id).Result;

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

            return View(movieTheater);
        }

        
        public IActionResult Create(string seatNumbers, int movieId, int theaterId)
        {
            if (string.IsNullOrEmpty(seatNumbers))
            {
                return BadRequest("No seats selected.");
            }

            List<int> selectedSeats = seatNumbers.Split(',').Select(int.Parse).ToList(); // selected seats Ids
            TicketBookingDto bookingDto = new TicketBookingDto
            {
                SeatNumbers = selectedSeats,
                MovieId = movieId,
                TheaterId = theaterId
            };
            return View(bookingDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveCreate(TicketBookingDto ticketBooking)
        {
            if (ticketBooking.SeatNumbers == null || !ticketBooking.SeatNumbers.Any())
            {
                return BadRequest("No seats selected.");
            }

            IEnumerable<Ticket> existingTickets =await _ticketRepository.GetByMovieAsync(ticketBooking.MovieId);
            IEnumerable<int> alreadyReserved = existingTickets
                .Where(t => ticketBooking.SeatNumbers.Contains(t.Seat.ID) && t.Seat.IsReserved)
                .Select(t => t.Seat.ID)
                .ToList();

            if (alreadyReserved.Any())
            {
                return BadRequest($"Seats already reserved: {string.Join(", ", alreadyReserved)}");
            }

            Theater? theater =await _theaterRepository.GetTheaterByMovieId(ticketBooking.MovieId);
           
            foreach (int seatId in ticketBooking.SeatNumbers)
            {
                Seat? seat = theater?.Seats?.FirstOrDefault(s => s.ID == seatId);
                seat.IsReserved = true;
                Ticket ticket = new Ticket
                {
                    MovieId = ticketBooking.MovieId,
                    UserId = 1,
                    PurchaseDate = DateTime.Now,
                    Price = 100, 
                };

                ticket.Seat = seat;
               await _ticketRepository.AddAsync(ticket);
              
                
            }
        await    _unitOfWork.Save();
            return RedirectToAction("Index", new { id = ticketBooking.MovieId });
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
