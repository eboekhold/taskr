using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using TaskrApi.Models;
using Microsoft.AspNetCore.Identity;

[Collection("ApplicationUsersTests")]
public class ApplicationUsersApiTests(CustomWebApplicationFactory factory) : IAsyncLifetime
{
  public ValueTask InitializeAsync() => ValueTask.CompletedTask;
  public async ValueTask DisposeAsync() => await factory.ResetDatabaseAsync();

  [Fact]
  public async System.Threading.Tasks.Task RegisterUser_WhenUserDoesNotExist_ReturnsCreated()
  {
    // Act
    var response = await factory.HttpClient.PostAsJsonAsync("/register", new { email = "example@email.com", password = "Password1!" }, TestContext.Current.CancellationToken);

    Console.WriteLine(response.Content);
    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact]
  public async System.Threading.Tasks.Task RegisterUser_WhenUserExists_ReturnsBadRequest()
  {
    // Arrange
    var userManager = factory.Services.GetRequiredService<UserManager<ApplicationUser>>();
    await userManager.CreateAsync(new ApplicationUser { UserName = "example@email.com" });

    // Act
    var response = await factory.HttpClient.PostAsJsonAsync("/register", new { email = "example@email.com", password = "Password1!" }, TestContext.Current.CancellationToken);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    var body = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
    body.Should().Contain("already taken");
  }

  [Fact]
  public async System.Threading.Tasks.Task RegisterUser_WhenEmailIsInvalid_ReturnsBadRequest()
  {
    // Act
    var response = await factory.HttpClient.PostAsJsonAsync("/register", new { email = "exampleemail", password = "Password1!" }, TestContext.Current.CancellationToken);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    var body = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
    body.Should().Contain("InvalidEmail");
  }

  [Fact]
  public async System.Threading.Tasks.Task RegisterUser_WhenPasswordIsTooWeak_ReturnsBadRequest()
  {
    // Act
    var response = await factory.HttpClient.PostAsJsonAsync("/register", new { email = "example@email.com", password = "password" }, TestContext.Current.CancellationToken);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    var body = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
    body.Should().Contain("Passwords must have at least");
  }

  [Fact]
  public async System.Threading.Tasks.Task LoginUser_WhenUserDoesNotExist_ReturnsUnauthorized()
  {
    // Act
    var response = await factory.HttpClient.PostAsJsonAsync("/login", new { email = "example@email.com", password = "Password1!" }, TestContext.Current.CancellationToken);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
  }

  [Fact]
  public async System.Threading.Tasks.Task LoginUser_WhenUserExists_ReturnsOkAndSetsCookie()
  {
    // Arrange
    var userManager = factory.Services.GetRequiredService<UserManager<ApplicationUser>>();
    await userManager.CreateAsync(new ApplicationUser { UserName = "example@email.com" }, "Password1!");

    // Act
    var response = await factory.HttpClient.PostAsJsonAsync("/login?useCookies=true", new { email = "example@email.com", password = "Password1!" }, TestContext.Current.CancellationToken);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.OK);
    response.Headers.Should().ContainKey("Set-Cookie");
  }

  [Fact] // Verifies that cookie authentication is forced even when `?useCookies=true` is not present.
  public async System.Threading.Tasks.Task LoginUser_WhenUserExists_WithoutCookies_ReturnsOkAndSetsCookie()
  {
    // Arrange
    var userManager = factory.Services.GetRequiredService<UserManager<ApplicationUser>>();
    await userManager.CreateAsync(new ApplicationUser { UserName = "example@email.com" }, "Password1!");

    // Act
    var response = await factory.HttpClient.PostAsJsonAsync("/login", new { email = "example@email.com", password = "Password1!" }, TestContext.Current.CancellationToken);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.OK);
    response.Headers.Should().ContainKey("Set-Cookie");
  }
}