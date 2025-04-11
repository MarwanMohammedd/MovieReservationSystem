using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieReservationSystem.DataAccess.Data;
using MovieReservationSystem.DataAccess.Repository;
using MovieReservationSystem.DataAccess.Repository.TheaterRepository;
using MovieReservationSystem.DataAccess.Repository.TicketRepository;
using MovieReservationSystem.DataAccess.UnitOfWork;
using MovieReservationSystem.Hubs;
using MovieReservationSystem.Model.Models;

namespace MovieReservationSystem
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ApplicationDBContext>(
                options =>
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DataBaseConnectionString"));
                }
            );

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IMovieRepository, MovieRepository>();
            builder.Services.AddScoped<IMovieSchedleRepository, MovieSchedleRepository>();
            builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
            builder.Services.AddScoped<ITicketRepository, TicketRepository>();
            builder.Services.AddScoped<TheaterRepository, TheaterRepository>();
            builder.Services.AddSignalR();

            builder.Services.AddAutoMapper(typeof(Program));


            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDBContext>();

            builder.Services.ConfigureApplicationCookie(option => { });

            builder.Services.AddAuthentication().AddGoogle(options => { });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapHub<CommentHub>("/commentHub");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Movie}/{action=ShowAll}");

            app.Run();

        }
    }
}