using SecurePass.Core.Interfaces;

namespace SecurePass.Core.Services;

public class NavigationService : INavigationService
{
    /// <inheritdoc cref="INavigationService.GoBackAsync()"/>
    public async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    /// <inheritdoc cref="INavigationService.GoToAsync(string, IDictionary{string, object})"/>
    public async Task GoToAsync(string route, IDictionary<string, object> parameters = null)
    {
        if (parameters != null)
        {
            await Shell.Current.GoToAsync(route, parameters);
        }

        await Shell.Current.GoToAsync(route);
    }
}
