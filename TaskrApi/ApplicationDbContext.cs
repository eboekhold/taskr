using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Taskr.Data
{
  public class ApplicationDbContext : IdentityDbContext
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
      : base(options)
    {
    }
    public DbSet<TaskrApi.Models.Task> Tasks { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      => optionsBuilder
          .UseSeeding((context, _) =>
            DatabaseSeeder.Seed((ApplicationDbContext)context))
          .UseAsyncSeeding(async (context, _, _) =>
            await DatabaseSeeder.SeedAsync((ApplicationDbContext)context));
  }
}