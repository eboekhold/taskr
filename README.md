# Taskr
Simple task manager.

## Requirements
- ASP.net
- Microsoft SQL Server
- Angular
- Docker (only for testing)

## Setup

### Install packages

#### Backend:
No additional input required, does so automatically when `dotnet run` is called.

#### Frontend:
```
cd TaskrClient
npm install
```

#### E2E Testing:
> Make sure you're in the root directory of the project.

```
npm install
```

### Database
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

#### Backend:
> Ensure you have Docker running. 

```
dotnet test --project TaskrApi.Tests
```

#### Frontend:
```
cd TaskrClient
ng test
```

#### End to end:
> Ensure you have Docker running. 

```
npx playwright test
```

## Running the application
#### Server:
```
> dotnet run --project TaskrApi

# with https:
> dotnet run --project TaskrApi --launch-profile https
```

#### Client:
```
> cd TaskrClient
> npm run start
```

Navigate to `http://localhost:4200/` to view the application.

## Documentation

After the server is up and running visit `http://localhost:5257/swagger/index.html` to view the API documentation.
