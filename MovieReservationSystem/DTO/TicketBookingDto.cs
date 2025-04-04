namespace MovieReservationSystem.DTO
{
    public class TicketBookingDto
    {

        public List<int> SeatNumbers { get; set; } = new List<int>();
        public int MovieId { get; set; }
        public int TheaterId { get; set; }
    }
}
