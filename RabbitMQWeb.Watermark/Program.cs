using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQWeb.Watermark.BackgroundServices;
using RabbitMQWeb.Watermark.Models;
using RabbitMQWeb.Watermark.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHostedService<ImageWatermarkProcessBackgroundService>();

builder.Services.AddSingleton<RabbitMQClientService>();
builder.Services.AddSingleton<RabbidMQPublisher>();
builder.Services.AddSingleton(sp => new ConnectionFactory()
{
    Uri = new Uri(builder.Configuration.GetConnectionString("RabbitMQ")),
    DispatchConsumersAsync = true
});
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase(databaseName: "RabbitMQWatermakDB");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Product/Create");
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
