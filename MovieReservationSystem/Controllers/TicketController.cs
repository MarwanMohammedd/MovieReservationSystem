using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieReservationSystem.DataAccess.Repository;
using MovieReservationSystem.DataAccess.Repository.TheaterRepository;
using MovieReservationSystem.DataAccess.Repository.TicketRepository;
using MovieReservationSystem.DataAccess.UnitOfWork;
using MovieReservationSystem.Model;
using MovieReservationSystem.Model.Models;
using MovieReservationSystem.Model.ViewModels;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json.Nodes;
using System.Text;
using MovieReservationSystem.ViewModels;

namespace MovieReservationSystem.Controllers
{
    
    public class TicketController : Controller
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly TheaterRepository _theaterRepository;
        private readonly IUnitOfWork _unitOfWork;
        public string total { get; set; } = "20.00";

        //Paypal
        public string PaypalUrl { get; set; } = "";
        public string PaypalClientId { get; set; } = "";

        public string PaypalSecret { get; set; } = "";

        public TicketController(ITicketRepository ticketRepository,IMovieRepository movieRepository
            ,TheaterRepository theaterRepository,IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            
            _ticketRepository = ticketRepository;
            _movieRepository = movieRepository;
            _theaterRepository = theaterRepository;
            _unitOfWork = unitOfWork;
            PaypalUrl = configuration["paypalSetting:Url"] ?? "";
            PaypalClientId = configuration["paypalSetting:ClientId"] ?? "";
            PaypalSecret = configuration["paypalSetting:ClientSecret"] ?? "";

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
            //papypal
            ViewBag.clientid = PaypalClientId;

            TicketBookingVM request = new TicketBookingVM() { MovieId = movieId, TheaterId = theaterId };
            request.SeatNumbers = seatNumbers.Split(',').Select(int.Parse).ToList();

            if (request.SeatNumbers == null || !request.SeatNumbers.Any())
            {
                return BadRequest("No seats selected.");
            }

            // You can map the request to your DTO or use it directly
            var bookingDto = new TicketBookingVM
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
        public async Task<IActionResult> SaveCreate(TicketBookingVM ticketBooking)
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
        //papypal
        public async Task<IActionResult> Create2()
        {



            JsonObject createOrderRequest = new JsonObject();
            createOrderRequest["intent"] = "CAPTURE";

            JsonObject amount = new JsonObject();
            amount.Add("currency_code", "USD");
            amount.Add("value", total);

            JsonObject purchaseUnit = new JsonObject();
            purchaseUnit.Add("amount", amount);

            JsonArray purchaseUnits = new JsonArray();
            purchaseUnits.Add(purchaseUnit);

            createOrderRequest.Add("purchase_units", purchaseUnits);

            string accessToken = await GetAccessToken();

            var url = PaypalUrl + "/v2/checkout/orders";

            string orderId = "";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Content = new StringContent(createOrderRequest.ToJsonString(), null, "application/json");


                var result = await client.SendAsync(requestMessage);
                if (result.IsSuccessStatusCode)
                {
                    var strRespones = await result.Content.ReadAsStringAsync();
                    var jsonObject = JsonNode.Parse(strRespones);
                    if (jsonObject != null)
                    {
                        orderId = jsonObject["id"]!.ToString();
                    }
                }
            }

            var response = new
            {
                id = orderId
            };
            return new JsonResult(response);
        }

        public async Task<IActionResult> Approve([FromBody] JsonObject data)
        {
            if (data == null)
            {
                return new JsonResult("");
            }
            string accessToken = await GetAccessToken();
            var orderId = data["orderID"]!.ToString();

            var url = PaypalUrl + "/v2/checkout/orders/" + orderId + "/capture";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Content = new StringContent("", null, "application/json");
                var result = await client.SendAsync(requestMessage);

                if (result.IsSuccessStatusCode)
                {

                    var strRespones = await result.Content.ReadAsStringAsync();

                    var jsonObject = JsonNode.Parse(strRespones);
                    if (jsonObject != null)
                    {
                        string paypalOrderStatus = jsonObject["status"]!.ToString();
                        if (paypalOrderStatus == "COMPLETED")
                        {
                            return new JsonResult("success");
                        }

                    }

                }
            }
            return new JsonResult("");
        }
        public IActionResult Cancel([FromBody] JsonObject data)
        {
            if (data == null)
            {
                return new JsonResult("");
            }
            var orderId = data["orderID"]!.ToString();
            return new JsonResult("");
        }
        private async Task<string> GetAccessToken()
        {
            string accessToken = "";

            var url = PaypalUrl + "/v1/oauth2/token";

            using (var client = new HttpClient())
            {
                string credentials =
                    Convert.ToBase64String(
                        Encoding.UTF8.GetBytes(PaypalClientId + ":" + PaypalSecret));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);


                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Content = new StringContent("grant_type=client_credentials",
                    null, "application/x-www-form-urlencoded");


                var result = await client.SendAsync(requestMessage);

                if (result.IsSuccessStatusCode)
                {
                    var strRespones = await result.Content.ReadAsStringAsync();

                    var jsonObject = JsonNode.Parse(strRespones);
                    if (jsonObject != null)
                    {
                        accessToken = jsonObject["access_token"]!.ToString();
                    }
                }
                else
                {
                    var error = await result.Content.ReadAsStringAsync();
                    Console.WriteLine("Failed to get access token:");
                    Console.WriteLine(error);
                }


            }

            return accessToken;
        }
    }
}
