using Databases;
using InterfaceAdapters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

// DI stuff
builder.Services.AddScoped<MeetingService>();
builder.Services.AddSingleton<IDatabaseAccess, InMemoryDatabase>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "actions",
    pattern: "action/start",
    defaults: new { controller = "Action", action = "Start" });

app.MapFallbackToFile("index.html");
;

app.Run();
