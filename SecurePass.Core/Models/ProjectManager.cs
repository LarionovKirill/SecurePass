namespace SecurePass.Core.Models;

public class ProjectManager
{
    public ProjectManager()
    {
        Accounts = new List<Account>();
        PasswordGeneratorOptions = new PasswordGeneratorOptions();
    }

    public PasswordGeneratorOptions PasswordGeneratorOptions { get; set; }
    public List<Account> Accounts { get; set; }
    public bool UseBiometric { get; set; }
    public string MasterPasswordHash { get; set; } = string.Empty;
}