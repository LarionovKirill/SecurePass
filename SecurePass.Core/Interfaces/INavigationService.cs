namespace SecurePass.Core.Interfaces;

public interface INavigationService
{
    /// <summary>
    /// Переходит к следующей странице.
    /// </summary>
    /// <param name="route">Путь до новой страницы.</param>
    /// <param name="parameters">Параметры перехода к новой странице.</param>
    Task GoToAsync(string route, IDictionary<string, object> parameters = null);

    /// <summary>
    /// Переходит к прошлой странице.
    /// </summary>
    /// <returns></returns>
    Task GoBackAsync();
}
