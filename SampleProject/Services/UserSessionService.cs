namespace SampleProject.Services;

public class UserSessionService
{
    public string? Username { get; private set; }

    public void SetUsername(string username)
    {
        Username = username;
    }

    public void ClearSession()
    {
        Username = null;
    }

    public bool IsLoggedIn => !string.IsNullOrEmpty(Username);
}
