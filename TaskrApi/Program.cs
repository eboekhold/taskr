using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Respawn;
using TaskrApi.Data;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsEnvironment("E2E"))
{
    builder.Configuration.AddUserSecrets<Program>();
}

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddApiEndpoints();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("Angular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseHealthChecks("/health");

app.UseAuthentication();
app.UseAuthorization();

app.MapIdentityApi<IdentityUser>();
app.MapControllers();

// Seed the database if it does not contain any tasks already.
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("E2E"))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.Database.EnsureCreatedAsync();

    if (app.Environment.IsDevelopment())
    {
        var anyTask = await db.Tasks.FirstOrDefaultAsync();
        if (anyTask == null)
        {
            DatabaseSeeder.Seed(db);
        }
    }
}

// Set up database reset checkpoint
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("E2E"))
{
    // Set up Respawner checkpoint
    await using var connection = new SqlConnection(connectionString);
    await connection.OpenAsync();

    var checkpoint = await Respawner.CreateAsync(
        connection,
        new RespawnerOptions
        {
            SchemasToInclude = ["dbo"],
        }
    );

    // Add reset db endpoint
    app.MapPost("/test/reset-db", async () =>
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        await checkpoint.ResetAsync(connection);
        return Results.Ok();
    });
}

app.Run();
