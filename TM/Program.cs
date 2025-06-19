using Microsoft.EntityFrameworkCore;
using TM.Mapper;
using TM.Models;
using Hangfire;
using Hangfire.MemoryStorage;
using TM.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(TourProfile));

// Thêm cấu hình Hangfire
builder.Services.AddHangfire(config =>
    config.UseMemoryStorage() // hoặc UseSqlServerStorage(connectionString)
);
builder.Services.AddHangfireServer();

// Đăng ký PassengerStatusChecker
builder.Services.AddTransient<PassengerStatusChecker>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Tour}/{action=Index}/{id?}")
    .WithStaticAssets();

app.UseHangfireDashboard(); // Truy cập /hangfire để xem job

app.Run();
