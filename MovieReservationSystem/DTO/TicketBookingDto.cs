namespace MovieReservationSystem.DTO
{
    public class TicketBookingDto
    {
        public int UserId { get; set; }
        public List<int> SeatNumbers { get; set; } = new List<int>();
        public int MovieId { get; set; }
        public int TheaterId { get; set; }
    }
}
