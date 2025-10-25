using Microsoft.EntityFrameworkCore;
using RESTful.Domain.Interfaces;
using RESTful.Infrastructure.Data;
using RESTful.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Настройка Entity Framework
builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(
  builder.Configuration.GetConnectionString("DefaultConnection") ?? 
        "Server=(localdb)\\mssqllocaldb;Database=ConferenceDB;Trusted_Connection=True;MultipleActiveResultSets=true"));

// Регистрация репозиториев
builder.Services.AddScoped<IParticipantRepository, ParticipantRepository>();
builder.Services.AddScoped<IPresentationRepository, PresentationRepository>();
builder.Services.AddScoped<ICoSpeakerRepository, CoSpeakerRepository>();

// CORS для работы с Blazor
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor", policy =>
  {
        policy.WithOrigins("http://localhost:5274", "https://localhost:7146")
    .AllowAnyHeader()
          .AllowAnyMethod();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowBlazor");

app.UseAuthorization();

app.MapControllers();

app.Run();
