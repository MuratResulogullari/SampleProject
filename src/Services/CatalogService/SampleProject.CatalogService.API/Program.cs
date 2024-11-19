using Microsoft.EntityFrameworkCore;
using SampleProject.CatalogService.API;
using SampleProject.CatalogService.API.Contexts;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddRabbitMQClient("messaging");
builder.AddRedisDistributedCache("cache");
builder.AddApplicationServices();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
  var dbContext = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
  dbContext.Database.Migrate(); // Veritabanı migrasyonlarını uygular
}
app.Run();
