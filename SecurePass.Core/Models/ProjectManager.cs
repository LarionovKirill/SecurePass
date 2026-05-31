namespace SecurePass.Core.Models;

/// <summary>
/// Менеджер проекта.
/// </summary>
public class ProjectManager
{
    /// <summary>
    /// Создает объект класса <see cref="ProjectManager"/>.
    /// </summary>
    public ProjectManager()
    {
    }

    /// <summary>
    /// Возвращает и задает настройки для генерации пароля.
    /// </summary>
    public PasswordGeneratorOptions PasswordGeneratorOptions { get; set; }

    /// <summary>
    /// Возвращает и задает список записей паролей.
    /// </summary>
    public List<Account> Accounts { get; set; }

    /// <summary>
    /// Возвращает и задает список записей паролей.
    /// </summary>
    public bool UseBiometric { get; set; }
}
