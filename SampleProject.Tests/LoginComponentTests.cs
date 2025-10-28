using Bunit;
using Microsoft.Extensions.Configuration;
using SampleProject.Components.Pages;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using SampleProject.Services;
using Microsoft.AspNetCore.Components;

namespace SampleProject.Tests;

[TestClass]
public class LoginComponentTests : Bunit.TestContext
{
    [TestInitialize]
    public void Setup()
    {
        var inMemorySettings = new Dictionary<string, string> {
            {"Authentication:Username", "admin"},
            {"Authentication:Password", "password123"},
            {"Authentication:UseApi", "false"}
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();

        Services.AddSingleton(configuration);
        Services.AddHttpClient("AuthApi");
        Services.AddScoped<UserSessionService>();
        Services.AddLocalization();
    }

    [TestMethod]
    public void Login_DisplaysCorrectHeading()
    {
        // Arrange & Act
        var cut = RenderComponent<Login>();

        // Assert
        var heading = cut.Find("h1");
        Assert.AreEqual("Login", heading.TextContent);
    }

    [TestMethod]
    public void Login_HasUsernameInput()
    {
        // Arrange & Act
        var cut = RenderComponent<Login>();

        // Assert
        var usernameInput = cut.Find("#username");
        Assert.IsNotNull(usernameInput);
        Assert.AreEqual("text", usernameInput.GetAttribute("type"));
    }

    [TestMethod]
    public void Login_HasPasswordInput()
    {
        // Arrange & Act
        var cut = RenderComponent<Login>();

        // Assert
        var passwordInput = cut.Find("#password");
        Assert.IsNotNull(passwordInput);
        Assert.AreEqual("password", passwordInput.GetAttribute("type"));
    }

    [TestMethod]
    public void Login_HasLoginButton()
    {
        // Arrange & Act
        var cut = RenderComponent<Login>();

        // Assert
        var button = cut.Find("button");
        Assert.AreEqual("Login", button.TextContent);
    }

    [TestMethod]
    public void Login_EmptyCredentials_ShowsError()
    {
        // Arrange
        var cut = RenderComponent<Login>();
        var button = cut.Find("button");

        // Act
        button.Click();

        // Assert
        var errorMessage = cut.Find(".alert-danger");
        Assert.IsTrue(errorMessage.TextContent.Contains("Username and password are required."));
    }

    [TestMethod]
    public void Login_WrongUsername_ShowsError()
    {
        // Arrange
        var cut = RenderComponent<Login>();
        var usernameInput = cut.Find("#username");
        var passwordInput = cut.Find("#password");
        var button = cut.Find("button");

        // Act
        usernameInput.Change("wronguser");
        passwordInput.Change("password123");
        button.Click();

        // Assert
        var errorMessage = cut.Find(".alert-danger");
        Assert.IsTrue(errorMessage.TextContent.Contains("Username is not right."));
    }

    [TestMethod]
    public void Login_WrongPassword_ShowsError()
    {
        // Arrange
        var cut = RenderComponent<Login>();
        var usernameInput = cut.Find("#username");
        var passwordInput = cut.Find("#password");
        var button = cut.Find("button");

        // Act
        usernameInput.Change("admin");
        passwordInput.Change("wrongpassword");
        button.Click();

        // Assert
        var errorMessage = cut.Find(".alert-danger");
        Assert.IsTrue(errorMessage.TextContent.Contains("Password is not right."));
    }

    [TestMethod]
    public void Login_CorrectCredentials_NoError()
    {
        // Arrange
        var navManager = Services.GetRequiredService<NavigationManager>();
        var cut = RenderComponent<Login>();
        var usernameInput = cut.Find("#username");
        var passwordInput = cut.Find("#password");
        var button = cut.Find("button");

        // Act
        usernameInput.Change("admin");
        passwordInput.Change("password123");
        button.Click();

        // Assert - no error message should be displayed
        var errorMessages = cut.FindAll(".alert-danger");
        Assert.AreEqual(0, errorMessages.Count);
        
        // Assert - should navigate to home page
        Assert.IsTrue(navManager.Uri.EndsWith("/"));
    }

    [TestMethod]
    public void Login_HasCorrectPageTitle()
    {
        // Arrange & Act
        var cut = RenderComponent<Login>();

        // Assert
        var pageTitle = cut.Find("h1");
        Assert.AreEqual("Login", pageTitle.TextContent);
    }
}
