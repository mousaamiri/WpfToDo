using WpfToDo.Core.Themes;

namespace WpfToDo.Core.Services;

public interface IThemeService
{
    AppTheme CurrentTheme { get; }
    AppTheme GetTheme();
    void SetTheme(AppTheme theme);
}
