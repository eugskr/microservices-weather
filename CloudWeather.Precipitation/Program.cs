using CloudWeather.Precipitation.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PrecipDbContext>(opt =>
{
    opt.EnableSensitiveDataLogging();
    opt.EnableDetailedErrors();
    opt.UseNpgsql(builder.Configuration.GetConnectionString("AppDb"));
}, ServiceLifetime.Transient);

var app = builder.Build();

app.MapGet("/observation/{zip}", async (string zip, [FromQuery] int? days, PrecipDbContext db) =>
{
    if (days is null or < 1 or > 30)
    {
        return Results.BadRequest("Please provide a 'days' query parameters between 1 and 30");
    }

    var startDate = DateTime.UtcNow - TimeSpan.FromDays(days.Value);
    var result = await db.Precipitations.Where(precip => precip.ZipCode == zip && precip.CreatedOn > startDate)
        .ToListAsync();
    return Results.Ok(result);
});

app.Run();