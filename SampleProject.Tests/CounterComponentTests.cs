using Bunit;
using Microsoft.Extensions.DependencyInjection;
using SampleProject.Components.Pages;

namespace SampleProject.Tests;

[TestClass]
public class CounterComponentTests : Bunit.TestContext
{
    [TestInitialize]
    public void Setup()
    {
        Services.AddLocalization();
    }

    [TestMethod]
    public void Counter_InitialCountIsZero()
    {
        // Arrange & Act
        var cut = RenderComponent<Counter>();

        // Assert
        cut.Find("p[role='status']").MarkupMatches("<p role=\"status\">Current count: 0</p>");
    }

    [TestMethod]
    public void Counter_ClickButton_IncrementsCount()
    {
        // Arrange
        var cut = RenderComponent<Counter>();
        var button = cut.Find("button");

        // Act
        button.Click();

        // Assert
        cut.Find("p[role='status']").MarkupMatches("<p role=\"status\">Current count: 1</p>");
    }

    [TestMethod]
    public void Counter_ClickButtonMultipleTimes_IncrementsCountCorrectly()
    {
        // Arrange
        var cut = RenderComponent<Counter>();
        var button = cut.Find("button");

        // Act
        button.Click();
        button.Click();
        button.Click();

        // Assert
        cut.Find("p[role='status']").MarkupMatches("<p role=\"status\">Current count: 3</p>");
    }

    [TestMethod]
    public void Counter_HasCorrectPageTitle()
    {
        // Arrange & Act
        var cut = RenderComponent<Counter>();

        // Assert
        var pageTitle = cut.Find("h1");
        Assert.AreEqual("Counter", pageTitle.TextContent);
    }

    [TestMethod]
    public void Counter_ButtonHasCorrectClass()
    {
        // Arrange & Act
        var cut = RenderComponent<Counter>();

        // Assert
        var button = cut.Find("button");
        Assert.IsTrue(button.ClassList.Contains("btn"));
        Assert.IsTrue(button.ClassList.Contains("btn-primary"));
    }

    [TestMethod]
    public void Counter_ButtonHasCorrectText()
    {
        // Arrange & Act
        var cut = RenderComponent<Counter>();

        // Assert
        var button = cut.Find("button");
        Assert.AreEqual("Click me", button.TextContent);
    }
}
