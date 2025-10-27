using Bunit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SampleProject.Components.Pages;
using SampleProject.Services;

namespace SampleProject.Tests;

[TestClass]
public class LoginLoggingTests : Bunit.TestContext
{
    private TestLoggerProvider? _loggerProvider;

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

        _loggerProvider = new TestLoggerProvider();

        Services.AddSingleton(configuration);
        Services.AddHttpClient("AuthApi");
        Services.AddScoped<UserSessionService>();
        Services.AddLogging(builder =>
        {
            builder.AddProvider(_loggerProvider);
        });
    }

    [TestMethod]
    public void Login_LogsLoginAttempt()
    {
        // Arrange
        var cut = RenderComponent<Login>();
        JSInterop.SetupVoid("alert", "Login successful");
        var usernameInput = cut.Find("#username");
        var passwordInput = cut.Find("#password");
        var button = cut.Find("button");

        // Act
        usernameInput.Change("admin");
        passwordInput.Change("password123");
        button.Click();

        // Assert
        var logs = _loggerProvider!.GetLogs();
        Assert.IsTrue(logs.Any(l => l.Contains("Login attempt for username: admin")), 
            "Should log login attempt");
    }

    [TestMethod]
    public void Login_WithEmptyCredentials_LogsWarning()
    {
        // Arrange
        var cut = RenderComponent<Login>();
        var button = cut.Find("button");

        // Act
        button.Click();

        // Assert
        var logs = _loggerProvider!.GetLogs();
        Assert.IsTrue(logs.Any(l => l.Contains("Login failed: empty credentials for username")), 
            "Should log warning for empty credentials");
    }

    [TestMethod]
    public void Login_WithValidCredentials_LogsSuccess()
    {
        // Arrange
        var cut = RenderComponent<Login>();
        JSInterop.SetupVoid("alert", "Login successful");
        var usernameInput = cut.Find("#username");
        var passwordInput = cut.Find("#password");
        var button = cut.Find("button");

        // Act
        usernameInput.Change("admin");
        passwordInput.Change("password123");
        button.Click();

        // Assert
        var logs = _loggerProvider!.GetLogs();
        Assert.IsTrue(logs.Any(l => l.Contains("Login successful via config for username: admin")), 
            "Should log successful login");
    }

    [TestMethod]
    public void Login_WithInvalidUsername_LogsWarning()
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
        var logs = _loggerProvider!.GetLogs();
        Assert.IsTrue(logs.Any(l => l.Contains("Login failed: incorrect username")), 
            "Should log warning for incorrect username");
    }

    [TestMethod]
    public void Login_WithInvalidPassword_LogsWarning()
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
        var logs = _loggerProvider!.GetLogs();
        Assert.IsTrue(logs.Any(l => l.Contains("Login failed: incorrect password for username")), 
            "Should log warning for incorrect password");
    }
}

// Test logger implementation to capture log messages
public class TestLoggerProvider : ILoggerProvider
{
    private readonly List<string> _logs = new();

    public ILogger CreateLogger(string categoryName)
    {
        return new TestLogger(_logs);
    }

    public List<string> GetLogs() => _logs;

    public void Dispose() { }
}

public class TestLogger : ILogger
{
    private readonly List<string> _logs;

    public TestLogger(List<string> logs)
    {
        _logs = logs;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        _logs.Add(formatter(state, exception));
    }
}
