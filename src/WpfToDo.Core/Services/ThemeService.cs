using WpfToDo.Core.Themes;

namespace WpfToDo.Core.Services;

public sealed class ThemeService : IThemeService
{
    private readonly string _storagePath;

    public ThemeService(string? storagePath = null)
    {
        _storagePath = storagePath
            ?? Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "WpfToDo",
                "theme.txt");
        CurrentTheme = LoadTheme();
    }

    public AppTheme CurrentTheme { get; private set; }

    public AppTheme GetTheme() => CurrentTheme;

    public void SetTheme(AppTheme theme)
    {
        CurrentTheme = theme;
        var directory = Path.GetDirectoryName(_storagePath);
        if (!string.IsNullOrWhiteSpace(directory))
            Directory.CreateDirectory(directory);
        File.WriteAllText(_storagePath, theme.ToString());
    }

    private AppTheme LoadTheme()
    {
        if (!File.Exists(_storagePath))
            return AppTheme.Light;

        var value = File.ReadAllText(_storagePath).Trim();
        return Enum.TryParse<AppTheme>(value, ignoreCase: true, out var theme)
            ? theme
            : AppTheme.Light;
    }
}
