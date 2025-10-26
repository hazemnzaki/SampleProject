using Bunit;
using SampleProject.Components.Pages;

namespace SampleProject.Tests;

[TestClass]
public class WeatherComponentTests : Bunit.TestContext
{
    // The Weather component has a 500ms delay in OnInitializedAsync
    // We wait slightly longer to ensure initialization completes
    private const int ComponentInitializationDelayMs = 600;

    [TestMethod]
    public void Weather_InitiallyShowsLoadingMessage()
    {
        // Arrange & Act
        var cut = RenderComponent<Weather>();

        // Assert
        var loadingText = cut.Find("p em");
        Assert.AreEqual("Loading...", loadingText.TextContent);
    }

    [TestMethod]
    public async Task Weather_AfterInitialization_DisplaysForecasts()
    {
        // Arrange
        var cut = RenderComponent<Weather>();

        // Act - Wait for the async initialization to complete
        await Task.Delay(ComponentInitializationDelayMs);
        cut.WaitForState(() => cut.FindAll("table tbody tr").Count > 0, TimeSpan.FromSeconds(2));

        // Assert
        var rows = cut.FindAll("table tbody tr");
        Assert.AreEqual(5, rows.Count, "Should display 5 weather forecasts");
    }

    [TestMethod]
    public void Weather_HasCorrectPageTitle()
    {
        // Arrange & Act
        var cut = RenderComponent<Weather>();

        // Assert
        var pageTitle = cut.Find("h1");
        Assert.AreEqual("Weather", pageTitle.TextContent);
    }

    [TestMethod]
    public void Weather_DisplaysCorrectDescription()
    {
        // Arrange & Act
        var cut = RenderComponent<Weather>();

        // Assert
        var description = cut.Find("p:not(:has(em))");
        Assert.AreEqual("This component demonstrates showing data.", description.TextContent);
    }

    [TestMethod]
    public async Task Weather_TableHasCorrectHeaders()
    {
        // Arrange
        var cut = RenderComponent<Weather>();

        // Act - Wait for the async initialization
        await Task.Delay(ComponentInitializationDelayMs);
        cut.WaitForState(() => cut.FindAll("table").Count > 0, TimeSpan.FromSeconds(2));

        // Assert
        var headers = cut.FindAll("table thead tr th");
        Assert.AreEqual(4, headers.Count);
        Assert.AreEqual("Date", headers[0].TextContent);
        Assert.AreEqual("Temp. (C)", headers[1].TextContent);
        Assert.AreEqual("Temp. (F)", headers[2].TextContent);
        Assert.AreEqual("Summary", headers[3].TextContent);
    }

    [TestMethod]
    public async Task Weather_TableHasTableClass()
    {
        // Arrange
        var cut = RenderComponent<Weather>();

        // Act - Wait for the async initialization
        await Task.Delay(ComponentInitializationDelayMs);
        cut.WaitForState(() => cut.FindAll("table").Count > 0, TimeSpan.FromSeconds(2));

        // Assert
        var table = cut.Find("table");
        Assert.IsTrue(table.ClassList.Contains("table"));
    }

    [TestMethod]
    public async Task Weather_AllRowsHaveFourColumns()
    {
        // Arrange
        var cut = RenderComponent<Weather>();

        // Act - Wait for the async initialization
        await Task.Delay(ComponentInitializationDelayMs);
        cut.WaitForState(() => cut.FindAll("table tbody tr").Count > 0, TimeSpan.FromSeconds(2));

        // Assert
        var rows = cut.FindAll("table tbody tr");
        foreach (var row in rows)
        {
            var columns = row.QuerySelectorAll("td");
            Assert.AreEqual(4, columns.Length, "Each row should have 4 columns");
        }
    }
}
