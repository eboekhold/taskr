using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskrApi.Data;
using Testcontainers.MsSql;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
  private readonly MsSqlContainer _msSqlContainer;

  public HttpClient HttpClient { get; private set; } = null!;

  public CustomWebApplicationFactory()
  {
    _msSqlContainer = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2025-CU7-ubuntu-24.04")
      .WithPassword("My_strong_password123!") // Not a real password, only used within the docker container during testing runs.
      .Build();
  }

  public async ValueTask InitializeAsync()
  {
    await _msSqlContainer.StartAsync();
    HttpClient = CreateClient();
  }

  public new async Task DisposeAsync()
  {
    await _msSqlContainer.DisposeAsync();
  }

  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    builder.UseEnvironment("Testing");

    builder.ConfigureServices(services =>
    {
      // Remove existing DbContext
      services.Remove(services.Single(service => service.ServiceType == typeof(DbContextOptions<ApplicationDbContext>)));

      // Add Testcontainers MSSQL DbContext
      services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(_msSqlContainer.GetConnectionString()));

      // Apply migrations
      using var scope = services.BuildServiceProvider().CreateScope();
      var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
      db.Database.Migrate();
    });
  }
}