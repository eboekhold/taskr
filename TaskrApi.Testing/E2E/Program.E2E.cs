using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class ProgramE2E
{
  public static async Task Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);

    var databaseManager = new TestDatabaseManager();
    await databaseManager.StartContainerAsync();
    await databaseManager.SetupDatabaseAsync();

    AppSetup.ConfigureServices(builder, databaseManager.ConnectionString);

    builder.Services.AddSingleton(databaseManager);

    var app = builder.Build();

    AppSetup.ConfigurePipeline(app);

    await databaseManager.SetCheckpointAsync();

    // Set up database reset checkpoint
    if (app.Environment.IsEnvironment("E2E"))
    {
      // Add reset db endpoint
      app.MapPost("/test/reset-db", async (TestDatabaseManager dbManager) =>
      {
        await dbManager.RestoreCheckpointAsync();
        return Results.Ok();
      });
    }

    app.Run();
  }
}

