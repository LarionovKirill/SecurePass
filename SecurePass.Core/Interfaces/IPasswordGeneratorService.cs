using SecurePass.Core.Models;

namespace SecurePass.Core.Interfaces;

public interface IPasswordGeneratorService
{
    string GeneratePassword(PasswordGeneratorOptions options);
}