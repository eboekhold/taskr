using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskrApi.Data;
using TaskrApi.Models;

public static class AppSetup
{
  public static void ConfigureServices(WebApplicationBuilder builder, string connectionString)
  {
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
      options.UseSqlServer(connectionString));

    builder.Services.AddIdentityCore<ApplicationUser>()
      .AddRoles<IdentityRole>()
      .AddEntityFrameworkStores<ApplicationDbContext>()
      .AddApiEndpoints()
      .AddSignInManager()
      .AddDefaultTokenProviders();

    builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
      .AddIdentityCookies();

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

    app.MapIdentityApi<ApplicationUser>();
    ForceLoginPathToUseCookies(app);

    app.MapControllers();
  }

  // The ASP.NET Core Identity login endpoint from `.MapIdentityApi` uses bearer tokens by default.
  // Attempting to login without the `?useCookies=true` causes an internal exception because we don't support bearer tokens as authentication method.
  // Forcing the parameter to be present is the easiest way to avoid the exception without having to write custom authentication endpoints.
  //   (there is allegedly no way to overwrite ~only~ the login endpoint from `.MapIdentityApi`, it's a take it all or leave it kind of situation)
  private static void ForceLoginPathToUseCookies(WebApplication app)
  {
    app.Use(async (context, next) =>
    {
      if (context.Request.Path == "/login" && context.Request.Method == HttpMethods.Post && !context.Request.Query.ContainsKey("useCookies"))
      {
        context.Request.QueryString = context.Request.QueryString.Add("useCookies", "true");
      }

      await next();
    });
  }
}