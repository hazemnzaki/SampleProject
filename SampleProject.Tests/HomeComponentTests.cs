using Bunit;
using SampleProject.Components.Pages;

namespace SampleProject.Tests;

[TestClass]
public class HomeComponentTests : Bunit.TestContext
{
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

    [TestMethod]
    public void Home_RendersCorrectly()
    {
        // Arrange & Act
        var cut = RenderComponent<Home>();

        // Assert
        var markup = cut.Markup;
        Assert.IsTrue(markup.Contains("Hello, world!"));
        Assert.IsTrue(markup.Contains("Welcome to your new app."));
    }
}
