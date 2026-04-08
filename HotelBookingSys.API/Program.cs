using HotelBookingSys.Application.UseCases.Customers;
using HotelBookingSys.Application.UseCases.Reservations;
using HotelBookingSys.Application.UseCases.Rooms;
using HotelBookingSys.Infrastructure.Data;
using HotelBookingSys.Infrastructure.DepencyInjection;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Get the connection string from configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' is missing or empty. Configure it via appsettings or environment variable ConnectionStrings__DefaultConnection.");
}

//Add contorllers, swagger and infrastructure services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure(connectionString);

//Add use cases
builder.Services.AddScoped<CreateReservationUseCase>();
builder.Services.AddScoped<CreateCustomerUseCase>();
builder.Services.AddScoped<GetCustomerByIdUseCase>();
builder.Services.AddScoped<GetCustomerByEmailUseCase>();
builder.Services.AddScoped<GetCustomerByPhoneUseCase>();
builder.Services.AddScoped<UpdateCustomerUseCase>();
builder.Services.AddScoped<DeleteCustomerUseCase>();
builder.Services.AddScoped<GetAllRoomsUseCase>();
builder.Services.AddScoped<GetCustomersUseCase>();
builder.Services.AddScoped<GetReservationsUseCase>();
builder.Services.AddScoped<GetReservationByIdUseCase>();
builder.Services.AddScoped<GetActiveReservationsByDateRangeUseCase>();
builder.Services.AddScoped<GetAvailableRoomsUseCase>();
builder.Services.AddScoped<CancelReservationUseCase>();
builder.Services.AddScoped<UpdateReservationUseCase>();
builder.Services.AddScoped<CompleteReservationUseCase>();

//Build the app
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Automatically apply migrations and seed the database in development
    using (var scope = app.Services.CreateScope())
    {
        
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
        HotelBookingSys.Infrastructure.Seeders.DatabaseSeeder.Seed(context);
    }
}

app.MapControllers();
app.Run();
