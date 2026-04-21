using System.Text;
using HotelBookingSys.Application.UseCases.Customers;
using HotelBookingSys.Application.UseCases.Analytics;
using HotelBookingSys.Application.UseCases.Auth;
using HotelBookingSys.Application.UseCases.Reservations;
using HotelBookingSys.Application.UseCases.Rooms;
using HotelBookingSys.API.Middleware;
using HotelBookingSys.Infrastructure.Data;
using HotelBookingSys.Infrastructure.DepencyInjection;
using HotelBookingSys.Infrastructure.Seeders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;


var builder = WebApplication.CreateBuilder(args);

// Get the connection string from configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' is missing or empty. Configure it via appsettings or environment variable ConnectionStrings__DefaultConnection.");
}

var jwtSecret = builder.Configuration["Jwt:Secret"];
if (string.IsNullOrWhiteSpace(jwtSecret))
    throw new InvalidOperationException("JWT secret is missing.");

//Add contorllers, swagger and infrastructure services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token. Example: eyJhbGci..."
    });

    c.AddSecurityRequirement(_ => new OpenApiSecurityRequirement
{
    {
        new OpenApiSecuritySchemeReference("Bearer"),
        new List<string>()
    }
});

});
//Add health checks, including a custom check for database connectivity
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>("database");

//Add infrastructure services, including the database context and repositories
//and dependency injection for use cases
builder.Services.AddInfrastructure(connectionString);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSecret))
        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("Token validated successfully");
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

//Add use cases
builder.Services.AddScoped<LoginUseCase>();
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
builder.Services.AddScoped<GetAvailableRoomsUseCase>();
builder.Services.AddScoped<CancelReservationUseCase>();
builder.Services.AddScoped<UpdateReservationUseCase>();
builder.Services.AddScoped<CompleteReservationUseCase>();
builder.Services.AddScoped<GetOccupancyUseCase>();
builder.Services.AddScoped<GetMonthlyRevenueUseCase>();
builder.Services.AddScoped<GetPopularRoomTypesUseCase>();
builder.Services.AddScoped<UploadRoomImageUseCase>();
builder.Services.AddScoped<DeleteRoomImageUseCase>();

var app = builder.Build();

// Global exception handling middleware should run first
app.UseMiddleware<ExceptionHandlingMiddleware>();

//Swagger to test endpoints also in AZURE
app.UseSwagger();
// Enable "Persist Authorization" in Swagger UI to allow users to stay logged in while testing endpoints
app.UseSwaggerUI(c =>
{
    c.ConfigObject.AdditionalItems["persistAuthorization"] = true;
});
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Apply migrations and seed the database on startup
if (app.Environment.IsDevelopment())
{
    // Automatically apply migrations and seed the database in development
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
        DatabaseSeeder.Seed(context);
    }
}
else
{
    // In production, seed the rooms and a test customer, seeder checks if they alrady exists
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        DatabaseSeeder.Seed(context);
    }
}

app.MapControllers();
// Configuration for the health check endpoint to return a detailed JSON response
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description
            }),
            duration = report.TotalDuration
        };
        await context.Response.WriteAsJsonAsync(result);
    }
}).AllowAnonymous();
app.Run();