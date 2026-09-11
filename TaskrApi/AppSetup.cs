using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskrApi.Data;

public static class AppSetup
{
  public static void ConfigureServices(WebApplicationBuilder builder, string connectionString)
  {
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));

    builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddApiEndpoints();

    builder.Services.AddControllers();

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
  }

  public static void ConfigurePipeline(WebApplication app)
  {
    app.UseHttpsRedirection();

    app.UseCors();

    app.UseHealthChecks("/health");

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapIdentityApi<IdentityUser>();
    app.MapControllers();
  }
}