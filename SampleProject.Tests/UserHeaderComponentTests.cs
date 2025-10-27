using Bunit;
using Microsoft.Extensions.DependencyInjection;
using SampleProject.Components.Layout;
using SampleProject.Services;

namespace SampleProject.Tests;

[TestClass]
public class UserHeaderComponentTests : Bunit.TestContext
{
    [TestMethod]
    public void UserHeader_NoUserLoggedIn_DisplaysNothing()
    {
        // Arrange
        Services.AddScoped<UserSessionService>();

        // Act
        var cut = RenderComponent<UserHeader>();

        // Assert
        var alerts = cut.FindAll(".user-header");
        Assert.AreEqual(0, alerts.Count);
    }

    [TestMethod]
    public void UserHeader_UserLoggedIn_DisplaysUsername()
    {
        // Arrange
        Services.AddScoped<UserSessionService>();
        var userSession = Services.GetRequiredService<UserSessionService>();
        userSession.SetUsername("testuser");

        // Act
        var cut = RenderComponent<UserHeader>();

        // Assert
        var header = cut.Find(".user-header");
        Assert.IsNotNull(header);
        Assert.IsTrue(header.TextContent.Contains("testuser"));
        Assert.IsTrue(header.TextContent.Contains("Logged in as:"));
    }

    [TestMethod]
    public void UserHeader_HasCorrectCssClasses()
    {
        // Arrange
        Services.AddScoped<UserSessionService>();
        var userSession = Services.GetRequiredService<UserSessionService>();
        userSession.SetUsername("admin");

        // Act
        var cut = RenderComponent<UserHeader>();

        // Assert
        var header = cut.Find(".user-header");
        Assert.IsTrue(header.ClassList.Contains("alert"));
        Assert.IsTrue(header.ClassList.Contains("alert-info"));
        Assert.IsTrue(header.ClassList.Contains("mb-3"));
    }
}
