using SampleProject.PaymentService.API.ApiClients;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
//builder.AddRabbitMQClient("messaging");
builder.AddRedisDistributedCache("cache");
// Add services to the container.

builder.Services.AddHttpClient<OrderServiceApiClient>(client =>
{
  // This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
  // Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
  client.BaseAddress = new("https+http://order-service-api");
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowAngularApp", builder =>
  {
    builder.WithOrigins("http://localhost:4200") 
           .AllowAnyHeader()
           .AllowAnyMethod();
  });
});
var app = builder.Build();
app.UseCors("AllowAngularApp");
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

app.Run();
