namespace SecurePass.Core.Models;

public class Account
{
    public Account()
    {
        Id = Guid.NewGuid().ToString();
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public Account(string name, string login, string password) : this()
    {
        Name = name;
        Login = login;
        Password = password;
    }

    public Account(string name, string login, string password, string url) : this(name, login, password)
    {
        WebsiteUrl = url;
    }

    public string Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string WebsiteUrl { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public void UpdateFrom(Account other)
    {
        Name = other.Name;
        WebsiteUrl = other.WebsiteUrl;
        Login = other.Login;
        Password = other.Password;
        Description = other.Description;
        UpdatedAt = DateTime.UtcNow;
    }
}