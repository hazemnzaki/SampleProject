namespace SampleProject.Services;

public class UserSessionService
{
    public string? Username { get; private set; }

    public event Action? OnChange;

    public void SetUsername(string username)
    {
        if (username == null)
        {
            throw new ArgumentNullException(nameof(username));
        }
        
        Username = username;
        NotifyStateChanged();
    }

    public void ClearSession()
    {
        Username = null;
        NotifyStateChanged();
    }

    public bool IsLoggedIn => !string.IsNullOrEmpty(Username);

    private void NotifyStateChanged() => OnChange?.Invoke();
}
