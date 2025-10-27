using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleProject.Services;

namespace SampleProject.Tests;

[TestClass]
public class UserSessionServiceTests
{
    [TestMethod]
    public void UserSession_InitialState_IsNotLoggedIn()
    {
        // Arrange
        var userSession = new UserSessionService();

        // Assert
        Assert.IsFalse(userSession.IsLoggedIn);
        Assert.IsNull(userSession.Username);
    }

    [TestMethod]
    public void UserSession_SetUsername_SetsUsernameAndIsLoggedIn()
    {
        // Arrange
        var userSession = new UserSessionService();

        // Act
        userSession.SetUsername("testuser");

        // Assert
        Assert.IsTrue(userSession.IsLoggedIn);
        Assert.AreEqual("testuser", userSession.Username);
    }

    [TestMethod]
    public void UserSession_ClearSession_ClearsUsernameAndIsNotLoggedIn()
    {
        // Arrange
        var userSession = new UserSessionService();
        userSession.SetUsername("testuser");

        // Act
        userSession.ClearSession();

        // Assert
        Assert.IsFalse(userSession.IsLoggedIn);
        Assert.IsNull(userSession.Username);
    }

    [TestMethod]
    public void UserSession_SetUsername_EmptyString_IsNotLoggedIn()
    {
        // Arrange
        var userSession = new UserSessionService();

        // Act
        userSession.SetUsername("");

        // Assert
        Assert.IsFalse(userSession.IsLoggedIn);
        Assert.AreEqual("", userSession.Username);
    }

    [TestMethod]
    public void UserSession_SetUsername_CanOverwriteExistingUsername()
    {
        // Arrange
        var userSession = new UserSessionService();
        userSession.SetUsername("user1");

        // Act
        userSession.SetUsername("user2");

        // Assert
        Assert.IsTrue(userSession.IsLoggedIn);
        Assert.AreEqual("user2", userSession.Username);
    }
}
