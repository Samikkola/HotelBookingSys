FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["HotelBookingSys.API/HotelBookingSys.API.csproj", "HotelBookingSys.API/"]
COPY ["HotelBookingSys.Application/HotelBookingSys.Application.csproj", "HotelBookingSys.Application/"]
COPY ["HotelBookingSys.Domain/HotelBookingSys.Domain.csproj", "HotelBookingSys.Domain/"]
COPY ["HotelBookingSys.Infrastructure/HotelBookingSys.Infrastructure.csproj", "HotelBookingSys.Infrastructure/"]
RUN dotnet restore "HotelBookingSys.API/HotelBookingSys.API.csproj"

COPY . .
RUN dotnet publish "HotelBookingSys.API/HotelBookingSys.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "HotelBookingSys.API.dll"]
