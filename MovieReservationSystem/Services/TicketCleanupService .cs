namespace MovieReservationSystem.Services
{
    public class TicketCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(10); 

        public TicketCleanupService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var ticketRepository = scope.ServiceProvider.GetRequiredService<ITicketRepository>();

                    try
                    {
                        int count = await ticketRepository.RemoveExpiredTicketsAsync();
                        Console.WriteLine($"{DateTime.Now}: Removed {count} expired tickets.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[TicketCleanup Error] {ex.Message}");
                    }
                }

                await Task.Delay(_interval, stoppingToken); 
            }
        }
    }
}
