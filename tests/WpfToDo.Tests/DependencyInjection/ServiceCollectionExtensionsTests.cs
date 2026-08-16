using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using WpfToDo.Core.Data;
using WpfToDo.Core.DependencyInjection;
using WpfToDo.Core.Services;
using WpfToDo.Core.ViewModels;

namespace WpfToDo.Tests.DependencyInjection;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddWpfToDoServices_RegistersCoreServicesAndViewModels()
    {
        var services = new ServiceCollection();
        services.AddWpfToDoServices("Data Source=:memory:");
        using var provider = services.BuildServiceProvider();

        provider.GetService<WpfToDoDbContext>().Should().NotBeNull();
        provider.GetService<ITodoListService>().Should().BeOfType<TodoListService>();
        provider.GetService<ITodoItemService>().Should().BeOfType<TodoItemService>();
        provider.GetService<ICategoryService>().Should().BeOfType<CategoryService>();
        provider.GetService<TodoListsViewModel>().Should().NotBeNull();
        provider.GetService<TodoItemsViewModel>().Should().NotBeNull();
    }
}
