using RepFrame.Api;
using RepFrame.Api.Features.Exercises;
using RepFrame.Api.Features.Sets;
using RepFrame.Api.Features.WorkoutSessions;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();

// Database
builder.Services.AddDbContext<RepFrameDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ??
        throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));

// FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddControllers()
    .AddFluentValidation(cv => cv.RegisterValidatorsFromAssembly(typeof(Program).Assembly));

// Feature handlers
builder.Services.AddScoped<SetHandler>();
builder.Services.AddScoped<ExerciseHandler>();
builder.Services.AddScoped<WorkoutSessionHandler>();

var app = builder.Build();

app.UseHttpsRedirection();

// Global exception handler middleware
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Unhandled exception");
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";
        var error = new { StatusCode = 500, Message = "Wystąpił nieoczekiwany błąd serwera." };
        await context.Response.WriteAsJsonAsync(error);
    }
});

app.MapControllers();

app.Run();
