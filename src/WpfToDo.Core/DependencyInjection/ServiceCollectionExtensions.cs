using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WpfToDo.Core.Data;
using WpfToDo.Core.Services;
using WpfToDo.Core.ViewModels;

namespace WpfToDo.Core.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWpfToDoServices(
        this IServiceCollection services,
        string connectionString = "Data Source=wpftodo.db")
    {
        services.AddDbContext<WpfToDoDbContext>(options =>
            options.UseSqlite(connectionString));

        services.AddScoped<ITodoListService, TodoListService>();
        services.AddScoped<ITodoItemService, TodoItemService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddTransient<TodoListsViewModel>();
        services.AddTransient<TodoItemsViewModel>();

        return services;
    }
}
