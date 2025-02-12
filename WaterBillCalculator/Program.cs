using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using WaterBillCalculator.Data;
using WaterBillCalculator.Interfaces;
using WaterBillCalculator.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Water Bill API", Version = "v1" });
});

builder.Services.AddTransient<MySqlConnection>(_ => new MySqlConnection(builder.Configuration.GetConnectionString("WaterBillDatabase")));

builder.Services.AddScoped<IWaterBillService, WaterBillService>();
 
builder.Services.AddDbContext<WaterBillContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("WaterBillDatabase"), new MySqlServerVersion(new Version(8, 0, 25)));
});

// Add CORS services and configure the policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        builder => builder
            .WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Water Bill API v1"));

// Apply the CORS policy
app.UseCors("AllowLocalhost");

app.MapControllers();

app.Run();