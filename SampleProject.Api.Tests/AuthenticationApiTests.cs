using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using SampleProject.Api.Models;

namespace SampleProject.Api.Tests;

[TestClass]
public class AuthenticationApiTests
{
    private WebApplicationFactory<Program>? _factory;
    private HttpClient? _client;

    [TestInitialize]
    public void Setup()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }

    [TestMethod]
    public async Task Authenticate_WithValidCredentials_ReturnsTrue()
    {
        // Arrange
        var request = new AuthenticationRequest
        {
            Username = "admin",
            Password = "123456"
        };

        // Act
        var response = await _client!.PostAsJsonAsync("/api/authenticate", request);
        
        // Assert
        Assert.IsTrue(response.IsSuccessStatusCode);
        var result = await response.Content.ReadFromJsonAsync<AuthenticationResponse>();
        Assert.IsNotNull(result);
        Assert.IsTrue(result.IsValid);
    }

    [TestMethod]
    public async Task Authenticate_WithInvalidUsername_ReturnsFalse()
    {
        // Arrange
        var request = new AuthenticationRequest
        {
            Username = "wronguser",
            Password = "123456"
        };

        // Act
        var response = await _client!.PostAsJsonAsync("/api/authenticate", request);
        
        // Assert
        Assert.IsTrue(response.IsSuccessStatusCode);
        var result = await response.Content.ReadFromJsonAsync<AuthenticationResponse>();
        Assert.IsNotNull(result);
        Assert.IsFalse(result.IsValid);
    }

    [TestMethod]
    public async Task Authenticate_WithInvalidPassword_ReturnsFalse()
    {
        // Arrange
        var request = new AuthenticationRequest
        {
            Username = "admin",
            Password = "wrongpassword"
        };

        // Act
        var response = await _client!.PostAsJsonAsync("/api/authenticate", request);
        
        // Assert
        Assert.IsTrue(response.IsSuccessStatusCode);
        var result = await response.Content.ReadFromJsonAsync<AuthenticationResponse>();
        Assert.IsNotNull(result);
        Assert.IsFalse(result.IsValid);
    }

    [TestMethod]
    public async Task Authenticate_WithEmptyCredentials_ReturnsFalse()
    {
        // Arrange
        var request = new AuthenticationRequest
        {
            Username = "",
            Password = ""
        };

        // Act
        var response = await _client!.PostAsJsonAsync("/api/authenticate", request);
        
        // Assert
        Assert.IsTrue(response.IsSuccessStatusCode);
        var result = await response.Content.ReadFromJsonAsync<AuthenticationResponse>();
        Assert.IsNotNull(result);
        Assert.IsFalse(result.IsValid);
    }
}
