using Bogus;
using Taskr.Data;

public static class DatabaseSeeder
{
  public static void Seed(ApplicationDbContext context)
  {
    if (!context.Tasks.Any())
    {
      var taskFaker = new Faker<TaskrApi.Models.Task>()
        .RuleFor(t => t.Name, f => f.Hacker.Phrase())
        .RuleFor(t => t.IsComplete, f => f.Random.Bool());

      var tasks = taskFaker.Generate(50);

      context.Tasks.AddRange(tasks);
      context.SaveChanges();
    }
  }
}