using Bogus;
using Microsoft.EntityFrameworkCore;
using TaskrApi.Data;

public static class DatabaseSeeder
{
  public static void Seed(ApplicationDbContext context) => SeedAsync(context).GetAwaiter().GetResult();

  public static async Task SeedAsync(ApplicationDbContext context)
  {
    if (!await context.Tasks.AnyAsync())
    {
      var taskFaker = new Faker<TaskrApi.Models.Task>()
        .RuleFor(t => t.Name, f => f.Hacker.Phrase())
        .RuleFor(t => t.IsComplete, f => f.Random.Bool());

      var tasks = taskFaker.Generate(50);

      context.Tasks.AddRange(tasks);
      await context.SaveChangesAsync();
    }
  }
}