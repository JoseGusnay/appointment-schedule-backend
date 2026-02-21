FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["HexagonalApi.Domain/VehicleAppointments.Domain.csproj", "HexagonalApi.Domain/"]
COPY ["HexagonalApi.Application/VehicleAppointments.Application.csproj", "HexagonalApi.Application/"]
COPY ["HexagonalApi.Infrastructure/VehicleAppointments.Infrastructure.csproj", "HexagonalApi.Infrastructure/"]
COPY ["HexagonalApi.Web/VehicleAppointments.Web.csproj", "HexagonalApi.Web/"]

RUN dotnet restore "HexagonalApi.Web/VehicleAppointments.Web.csproj"

COPY . .
WORKDIR "/src/HexagonalApi.Web"
RUN dotnet build "VehicleAppointments.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VehicleAppointments.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VehicleAppointments.Web.dll"]
