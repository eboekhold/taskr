# Taskr
Task manager for meatspace activities.

## Requirements
- ASP.net
- Microsoft SQL Server
- Angular
- Docker (only for testing)

## Setup

Initialise database connection string secret:
```
> dotnet user-secrets init
> dotnet user-secrets set ConnectionStrings:DefaultConnection "Server=localhost;Database=<database_name>;User Id=<user_name> Password=<password>;TrustServerCertificate=True"
```

Create the database
```
> dotnet ef database create
```

Trust the self-signed certificate for https support (optional):
```
> dotnet dev-certs https --trust
```

## Testing

Ensure you have Docker running. 

Then:
```
cd TaskrApi.Tests
dotnet test
```

## Running the application

Server:
```
> cd TaskrApi
> dotnet run

# with https:
> dotnet run --launch-profile https
```

Client:
```
> cd TaskrClient
> ng serve
```

Navigate to `http://localhost:4200/` to view the application.

## Documentation

After the server is up and running visit `http://localhost:5257/swagger/index.html` to view the API documentation.
