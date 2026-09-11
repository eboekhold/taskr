using Microsoft.EntityFrameworkCore;
using TaskrApi.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

AppSetup.ConfigureServices(builder, connectionString);

if (builder.Environment.IsDevelopment())
{
  builder.Services.AddOpenApi();
  builder.Services.AddEndpointsApiExplorer();
  builder.Services.AddSwaggerGen();
}

var app = builder.Build();

AppSetup.ConfigurePipeline(app);

if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
  app.UseSwagger();
  app.UseSwaggerUI();
}

// Seed the database if it does not contain any tasks already.
if (app.Environment.IsDevelopment())
{
  using var scope = app.Services.CreateScope();
  var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
  await db.Database.EnsureCreatedAsync();

  var anyTask = await db.Tasks.FirstOrDefaultAsync();
  if (anyTask == null)
  {
    DatabaseSeeder.Seed(db);
  }
}

app.Run();
