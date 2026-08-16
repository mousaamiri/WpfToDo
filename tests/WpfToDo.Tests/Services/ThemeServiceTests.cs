using FluentAssertions;
using WpfToDo.Core.Services;
using WpfToDo.Core.Themes;

namespace WpfToDo.Tests.Services;

public class ThemeServiceTests
{
    [Fact]
    public void GetTheme_WhenStorageDoesNotExist_ReturnsLight()
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), "theme.txt");
        var service = new ThemeService(path);

        service.GetTheme().Should().Be(AppTheme.Light);
    }

    [Fact]
    public void SetTheme_PersistsSelectionForNextServiceInstance()
    {
        var directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        var path = Path.Combine(directory, "theme.txt");
        var service = new ThemeService(path);

        service.SetTheme(AppTheme.Dark);

        var reloaded = new ThemeService(path);
        reloaded.CurrentTheme.Should().Be(AppTheme.Dark);
    }

    [Fact]
    public void InvalidStoredTheme_FallsBackToLight()
    {
        var directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(directory);
        var path = Path.Combine(directory, "theme.txt");
        File.WriteAllText(path, "unknown");

        new ThemeService(path).CurrentTheme.Should().Be(AppTheme.Light);
    }
}
