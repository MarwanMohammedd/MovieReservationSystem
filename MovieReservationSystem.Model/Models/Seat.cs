namespace MovieReservationSystem.Model.Models{
    public class Seat : BaseEnitiy
    {
        public string SeatNumber { get; set; }
        public int TheaterID { get; set; } 
        public int? TicketID { get; set; }
        public bool IsReserved { get; set; }
        public Theater? Theater { get; set; }
        public Ticket Ticket { get; set; } = null!;
    }
}