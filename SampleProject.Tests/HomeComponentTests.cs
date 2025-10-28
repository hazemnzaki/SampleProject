using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using SampleProject.Components.Pages;
using SampleProject.Resources;

namespace SampleProject.Tests;

[TestClass]
public class HomeComponentTests : Bunit.TestContext
{
    [TestInitialize]
    public void Setup()
    {
        Services.AddLocalization();
    }

    [TestMethod]
    public void Home_DisplaysCorrectHeading()
    {
        // Arrange & Act
        var cut = RenderComponent<Home>();

        // Assert
        var heading = cut.Find("h1");
        Assert.AreEqual("Hello, world!", heading.TextContent);
    }

    [TestMethod]
    public void Home_DisplaysWelcomeMessage()
    {
        // Arrange & Act
        var cut = RenderComponent<Home>();

        // Assert
        var content = cut.Markup;
        Assert.IsTrue(content.Contains("Welcome to your new app."));
    }
}
