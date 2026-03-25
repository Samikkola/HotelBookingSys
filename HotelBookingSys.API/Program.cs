using HotelBookingSys.Application.UseCases.Customers;
using HotelBookingSys.Application.UseCases.Reservations;
using HotelBookingSys.Application.UseCases.Rooms;
using HotelBookingSys.Infrastructure;
using HotelBookingSys.Infrastructure.Data;
using HotelBookingSys.Infrastructure.DepencyInjection;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

//Add DbContext with SQLite connection string from appsettings.json
//TODO: Move this to AddInfrastructure ?
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

//Add contorllers, swagger and infrastructure services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure();

//Add use cases
builder.Services.AddScoped<CreateReservationUseCase>();
builder.Services.AddScoped<CreateCustomerUseCase>();
builder.Services.AddScoped<GetCustomerByIdUseCase>();
builder.Services.AddScoped<GetAllRoomsUseCase>();
builder.Services.AddScoped<GetCustomersUseCase>();
builder.Services.AddScoped<GetReservationsUseCase>();
builder.Services.AddScoped<GetActiveReservationsByDateRangeUseCase>();
builder.Services.AddScoped<GetAvailableRoomsUseCase>();
builder.Services.AddScoped<CancelReservationUseCase>();
builder.Services.AddScoped<UpdateReservationDatesUseCase>();
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
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();
            HotelBookingSys.Infrastructure.Seeders.DatabaseSeeder.Seed(context);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while migrating or seeding the database.");
        }
    }
}

app.MapControllers();
app.Run();
