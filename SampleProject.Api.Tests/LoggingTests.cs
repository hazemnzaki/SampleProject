using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using SampleProject.Api.Models;

namespace SampleProject.Api.Tests;

[TestClass]
public class LoggingTests
{
    private WebApplicationFactory<Program>? _factory;
    private HttpClient? _client;
    private TestLoggerProvider? _loggerProvider;

    [TestInitialize]
    public void Setup()
    {
        _loggerProvider = new TestLoggerProvider();
        
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddLogging(logging =>
                    {
                        logging.AddProvider(_loggerProvider);
                    });
                });
            });
        
        _client = _factory.CreateClient();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }

    [TestMethod]
    public void Api_LogsStartupMessages()
    {
        // Assert - Check that startup logs were recorded
        var logs = _loggerProvider!.GetLogs();
        Assert.IsTrue(logs.Any(l => l.Contains("application starting up")), 
            "Should log application starting up");
    }

    [TestMethod]
    public async Task Authenticate_LogsAuthenticationAttempt()
    {
        // Arrange
        var request = new AuthenticationRequest
        {
            Username = "admin",
            Password = "123456"
        };

        // Act
        await _client!.PostAsJsonAsync("/api/authenticate", request);

        // Assert
        var logs = _loggerProvider!.GetLogs();
        Assert.IsTrue(logs.Any(l => l.Contains("Authentication attempt for username: admin")), 
            "Should log authentication attempt");
    }

    [TestMethod]
    public async Task Authenticate_WithValidCredentials_LogsSuccess()
    {
        // Arrange
        var request = new AuthenticationRequest
        {
            Username = "admin",
            Password = "123456"
        };

        // Act
        await _client!.PostAsJsonAsync("/api/authenticate", request);

        // Assert
        var logs = _loggerProvider!.GetLogs();
        Assert.IsTrue(logs.Any(l => l.Contains("Authentication successful for username: admin")), 
            "Should log successful authentication");
    }

    [TestMethod]
    public async Task Authenticate_WithInvalidCredentials_LogsFailure()
    {
        // Arrange
        var request = new AuthenticationRequest
        {
            Username = "wronguser",
            Password = "wrongpass"
        };

        // Act
        await _client!.PostAsJsonAsync("/api/authenticate", request);

        // Assert
        var logs = _loggerProvider!.GetLogs();
        Assert.IsTrue(logs.Any(l => l.Contains("Authentication failed for username: wronguser")), 
            "Should log failed authentication");
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
