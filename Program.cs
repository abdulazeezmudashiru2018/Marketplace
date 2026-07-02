using Microsoft.EntityFrameworkCore;
using MarketPlace.Models;

namespace MarketPlace
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
            builder.WebHost.UseUrls($"http://*:{port}");

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Get connection string and convert URL format if needed
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            if (!string.IsNullOrEmpty(connectionString) && connectionString.StartsWith("postgres"))
            {
                // Convert postgresql:// URL format to key=value format
                var uri = new Uri(connectionString);
                var userInfo = uri.UserInfo.Split(':');

                connectionString = $"Host={uri.Host};" +
                                   $"Port={(uri.Port > 0 ? uri.Port : 5432)};" +
                                   $"Database={uri.AbsolutePath.TrimStart('/')};" +
                                   $"Username={userInfo[0]};" +
                                   $"Password={userInfo[1]};" +
                                   $"SSL Mode=Require;Trust Server Certificate=true";
            }

            builder.Services.AddDbContext<MarketPlaceDbContext>(options =>
                options.UseNpgsql(connectionString));
            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=LandingPage}/{action=Index}/{id?}")
                .WithStaticAssets();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<MarketPlaceDbContext>();
                db.Database.Migrate(); // Creates tables automatically
            }

            app.Run();
        }
    }
}