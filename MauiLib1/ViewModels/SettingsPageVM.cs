using CommunityToolkit.Mvvm.ComponentModel;
using SecurePass.Core.Models;
using SecurePass.Core.Services;

namespace SecurePass.VM.ViewModels;

public partial class SettingsPageVM : ObservableObject
{
    private readonly ProjectManager _projectManager;

    private bool _useSpecialChars;

    private bool _useUppercase;

    private bool _useLowercase;

    private bool _useDigits;

    private int _passwordLength;

    public SettingsPageVM(ProjectStateManager projectStateManager)
    {
        _projectManager = projectStateManager.CurrentProject;
        UpdateSettings(_projectManager);
    }

    private void UpdateSettings(ProjectManager projectManager)
    {
        if (projectManager.PasswordGeneratorOptions != null)
        {
            var options = _projectManager.PasswordGeneratorOptions;

            UseSpecialChars = options.UseSpecialCharacters;
            UseUppercase = options.UseCapitalLetters;
            UseLowercase = options.UseLowercaseLetters;
            UseDigits = options.UseDigits;
            PasswordLength = 15;
        }
        // Маловерятный случай, потому что у настроек есть занчения по-умолчанию.
        else
        {
            UseSpecialChars = true;
            UseUppercase = true;
            UseLowercase = true;
            UseDigits = true;
            PasswordLength = 10;
        }
    }

    public bool UseSpecialChars
    {
        get => _useSpecialChars;
        set
        {
            SetProperty(ref _useSpecialChars, value);
            _projectManager.PasswordGeneratorOptions.UseSpecialCharacters = value;
        }
    }

    public bool UseUppercase
    {
        get => _useUppercase;
        set
        {
            SetProperty(ref _useUppercase, value);
            _projectManager.PasswordGeneratorOptions.UseCapitalLetters = value;
        }
    }

    public bool UseLowercase
    {
        get => _useLowercase;
        set
        {
            SetProperty(ref _useLowercase, value);
            _projectManager.PasswordGeneratorOptions.UseLowercaseLetters = value;
        }
    }

    public bool UseDigits
    {
        get => _useDigits;
        set
        {
            SetProperty(ref _useDigits, value);
            _projectManager.PasswordGeneratorOptions.UseDigits = value;
        }
    }

    public int PasswordLength
    {
        get => _passwordLength;
        set
        {
            SetProperty(ref _passwordLength, value);
            _projectManager.PasswordGeneratorOptions.Length = value;
        }
    }
}