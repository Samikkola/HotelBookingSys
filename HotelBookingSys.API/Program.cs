using HotelBookingSys.Application.UseCases;
using HotelBookingSys.Infrastructure;
using HotelBookingSys.Infrastructure.DepencyInjection;


var builder = WebApplication.CreateBuilder(args);

//Add contorllers, swagger and infrastructure services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure();

//Add use cases
builder.Services.AddScoped<CreateReservationUseCase>();
builder.Services.AddScoped<CreateCustomerUseCase>();
builder.Services.AddScoped<GetAllRoomsUseCase>();

//Build the app
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
