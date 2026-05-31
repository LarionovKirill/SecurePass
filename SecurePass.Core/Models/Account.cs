namespace SecurePass.Core.Models;

/// <summary>
/// Учетная запись.
/// </summary>
public class Account
{
    public Account(string name, string login, string password)
    {
        Name = name;
        Login = login;
        Password = password;
    }

    public Account(string name, string login, string password, string url)
    {
        Name = name;
        Login = login;
        Password = password;
        WebsiteUrl = url;
    }


    /// <summary>
    /// ID записи.
    /// </summary>
    public string Id { get; set; } 

    /// <summary>
    /// Название записи.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Url до ресурса.
    /// </summary>
    public string WebsiteUrl { get; set; }  
    
    /// <summary>
    /// Логин
    /// </summary>
    public string Login { get; set; }
    
    /// <summary>
    /// Пароль.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Доп информация записи.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Дата создания записи.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Дата изменения записи.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}
