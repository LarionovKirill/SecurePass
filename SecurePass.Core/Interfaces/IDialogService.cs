namespace SecurePass.Core.Interfaces;

public interface IDialogService
{
    Task<bool> ShowCreateMasterPasswordDialogAsync();
    Task ShowAlertAsync(string title, string message);
    Task<bool> ShowConfirmationAsync(string title, string message);
}