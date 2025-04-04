using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieReservationSystem.DataAccess.Data;
using MovieReservationSystem.DataAccess.Repository;
using MovieReservationSystem.Model;
using MovieReservationSystem.Model.Models;
using MovieReservationSystem.Model.ViewModels;

namespace MovieReservationSystem
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ApplicationDBContext>(options =>
            {
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DataBaseConnectionString"),
                    b => b.MigrationsAssembly("MovieReservationSystem.DataAccess")
                );
            });

            builder.Services.AddIdentity<ApplicationUser , IdentityRole<int>>()
            .AddEntityFrameworkStores<ApplicationDBContext>();
            builder.Services.AddScoped<ReviewsRepository>();
            builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
            builder.Services.AddScoped<ITicketRepository,TicketRepository>();
            builder.Services.AddScoped<IMovieRepository,MovieRepository>();
            builder.Services.AddScoped<TheaterRepository,TheaterRepository>();

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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();

        }
    }
}