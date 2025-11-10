# Uppbeat API

Uppbeat is a small REST API for managing music tracks, users, and authentication. Built with **ASP.NET Core 9**, **Entity Framework Core**, and **SQLite**, it includes **JWT authentication** and **Swagger UI** for exploring and testing the API.

---

## Key Features

- User registration and JWT-based authentication
- CRUD operations for music tracks, including genres and ownership
- SQL Server with EF Core migrations
- Swagger UI with JWT Bearer support for testing secured endpoints
- Tests in seperate project using xUnit and Moq 

```bash
dotnet test
```

---

## Requirements

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- dotnet CLI
- docker

---

## Running with Docker

```bash
docker-compose up --build
```
Once running, navigate to: https://localhost:8080/swagger to explore and test the API using Swagger UI.

---
## Quick Start (Local Development)

1. Clone the repository:
   ```bash
   git clone <repo-url>    
   ```
   
2. Navigate to the project directory:
   ```bash
   cd UppbeatApi
   ```
   
3. Restore dependencies and Build the project:
   ```bash
   dotnet restore
   dotnet build
   ```
4. Start the SqlServer DB container:
   ```bash
   docker-compose -f docker-compose.db.yaml up -d
   dotnet ef database update
   ```
5. Run the application:
   ```bash
   dotnet run
   ```
   or from sln root folder
   ```bash
    dotnet run --project .\src\Uppbeat.Api
   ```
6. Open your browser and navigate to: https://localhost:5001/swagger to access Swagger UI.
