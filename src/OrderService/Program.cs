using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Mappings;
using Refit;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<OrderDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
var paymentUrl = builder.Configuration.GetValue<string>("PaymentServiceUrl");
builder.Services.AddRefitClient<OrderService.Clients.IPaymentClient>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(paymentUrl));

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.Run();