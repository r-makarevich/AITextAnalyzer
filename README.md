# AITextAnalyzer Solution

This solution implements a text analysis API using ASP.NET Core with CQRS pattern.

## Solution Structure

```
AITextAnalyzer/
├── frontend/                          # Frontend UI project (to be implemented)
└── backend/                           # Backend Web API solution
    ├── TextAnalyzerAPI/               # Web API project (Presentation Layer)
    │   └── Controllers/
    │       └── TextAnalysisController.cs
    ├── TextAnalyzerAPI.Application/   # Application Layer (CQRS)
    │   ├── Commands/
    │   │   ├── AnalyzeTextCommand.cs
    │   │   └── AnalyzeTextCommandHandler.cs
    │   ├── Queries/
    │   │   ├── GetAllTextsQuery.cs
    │   │   └── GetAllTextsQueryHandler.cs
    │   ├── Interfaces/
    │   │   └── IApplicationDbContext.cs
    │   └── DependencyInjection.cs
    ├── TextAnalyzerAPI.Domain/        # Domain Layer (Entities)
    │   └── Entities/
    │       └── TextAnalysis.cs
    └── TextAnalyzerAPI.Infrastructure/ # Infrastructure Layer (Data Access)
        ├── Data/
        │   └── ApplicationDbContext.cs
        └── DependencyInjection.cs
```

## Architecture

This project follows **Clean Architecture** principles with **CQRS pattern** implementation using **MediatR**:

- **Domain Layer**: Contains entities and domain logic
- **Application Layer**: Contains business logic, commands, queries, and handlers (CQRS)
- **Infrastructure Layer**: Contains data access implementation (Entity Framework Core with SQLite database)
- **Presentation Layer**: Contains API controllers

## Technologies Used

- ASP.NET Core 9.0
- Entity Framework Core 9.0.1 (SQLite Database)
- MediatR 14.0.0 (CQRS implementation)
- Swashbuckle 6.5.0 (Swagger/OpenAPI)

## API Endpoints

### TextAnalysis Controller

Base URL: `/api/textanalysis`

#### 1. Analyze Text
- **POST** `/api/textanalysis/analyze`
- **Body**: String content (text to analyze)
- **Response**: Integer (ID of saved text)
- **Description**: Accepts a string and stores it in the database

#### 2. Get All Texts
- **GET** `/api/textanalysis/all`
- **Response**: List of TextAnalysis objects
- **Description**: Retrieves all saved text analyses

## Running the Application

### Prerequisites
- .NET 9.0 SDK

### Build
```bash
cd backend
dotnet build
```

### Run
```bash
cd backend/TextAnalyzerAPI
dotnet run
```

The API will be available at:
- HTTPS: https://localhost:7123
- HTTP: http://localhost:5196

### Swagger UI
Navigate to https://localhost:7123/swagger in your browser to access the Swagger UI for testing the API.

## Database

The application uses **SQLite database** for persistent storage. The database file `TextAnalyzer.db` is created automatically in the Infrastructure project's Data folder when the application starts. Data persists between application restarts.

**Database Location**: `backend/TextAnalyzerAPI.Infrastructure/Data/TextAnalyzer.db`

To switch to a different database provider (e.g., SQL Server, PostgreSQL), update the `AddInfrastructure` method in `TextAnalyzerAPI.Infrastructure/DependencyInjection.cs`.

## CQRS Pattern Implementation

The application uses the CQRS (Command Query Responsibility Segregation) pattern:

### Commands (Write Operations)
- **AnalyzeTextCommand**: Stores text in the database
- Handled by **AnalyzeTextCommandHandler**

### Queries (Read Operations)
- **GetAllTextsQuery**: Retrieves all stored texts
- Handled by **GetAllTextsQueryHandler**

### Benefits
- Clear separation between read and write operations
- Easier to test and maintain
- Scalable architecture
- Better code organization

## Future Enhancements

- Implement actual text analysis logic (sentiment analysis, keyword extraction, etc.)
- Add authentication and authorization
- Implement data validation and error handling
- Add logging
- Switch to a persistent database
- Add unit and integration tests
- Implement the frontend UI
