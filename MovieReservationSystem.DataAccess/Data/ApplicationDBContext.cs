using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieReservationSystem.Model.Models;

namespace MovieReservationSystem.DataAccess.Data{
    public class ApplicationDBContext : IdentityDbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieSchedule> MovieSchedules { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Theater> Theaters { get; set; }
        public DbSet<TheatersSchedule> TheatersSchedules { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> dbContextOptions) : base(dbContextOptions)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   
            modelBuilder.Entity<MovieSchedule>().HasKey(compsite=> new {compsite.MovieId , compsite.StartTime});
            modelBuilder.Entity<Review>().HasKey(compsite=> new {compsite.MovieId , compsite.UserId});
            modelBuilder.Entity<TheatersSchedule>().HasKey(compsite=> new {compsite.MovieId , compsite.TheaterId});
            modelBuilder.Entity<Seat>().HasKey(compsite=> new {compsite.ID , compsite.TheaterID});
            base.OnModelCreating(modelBuilder);
        }
    }
}