namespace MovieReservationSystem.ViewModels
{
    public class MovieAllVM
    {
        public int ID { get; set; }
        public string Title { get; set; } = null!;
        public int RateAvg { get; set; }
        public int TotalCount { get; set; }

        public string ImagePath { get; set; } = string.Empty;
        public string? Description { get; set; } = null!;

    }
}
