using Databases;
using InterfaceAdapters;

namespace WebService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

            builder.Services.AddControllersWithViews();

// DI stuff
            builder.Services.AddScoped<MeetingService>();
            builder.Services.AddScoped<GroupService>();
            builder.Services.AddScoped<PersonService>();
            builder.Services.AddSingleton<IDatabaseAccess, InMemoryDatabase>();

            var app = builder.Build();
            
            app.UseStaticFiles();
            app.UseRouting();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");

            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}