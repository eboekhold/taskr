using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Taskr.Data;

public class TestApiFactory : WebApplicationFactory<Program>
{
  private readonly string _connectionString;

  public TestApiFactory(string connectionString)
  {
    _connectionString = connectionString;
  }

  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    builder.ConfigureServices(services =>
    {
      // Remove existing DbContext
      services.Remove(services.Single(service => service.ServiceType == typeof(DbContextOptions<ApplicationDbContext>)));

      // Add Testcontainers MSSQL DbContext
      services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(_connectionString));

      // Apply migrations
      using var scope = services.BuildServiceProvider().CreateScope();
      var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
      db.Database.Migrate();
    });
  }
}