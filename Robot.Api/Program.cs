using Robot.Api.Configuration;
using Robot.Api.Models;
using Robot.Api.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IMoveRobotService, MoveRobotService>();
builder.Services.AddScoped<GridValidation>();
builder.Services.AddScoped<RobotValidation>();
builder.Services.AddScoped<IScent, Scent>();
builder.Services.Configure<RobotConstraintOptions>(
    builder.Configuration.GetSection("RobotConstraints"));

// Add CORS policy for port 5173
builder.Services.AddCors(options =>
{
    options.AddPolicy("Allow5173", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

WebApplication app = builder.Build();

// Use CORS policy
app.UseCors("Allow5173");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
