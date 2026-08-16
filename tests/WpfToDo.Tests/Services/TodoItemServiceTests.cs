using FluentAssertions;
using WpfToDo.Core.Models;
using WpfToDo.Core.Services;

namespace WpfToDo.Tests.Services;

public class TodoItemServiceTests
{
    [Fact]
    public async Task GetByListIdAsync_ReturnsOnlyItemsForRequestedList()
    {
        await using var context = ServiceTestFixture.CreateContext();
        var firstList = new TodoList { Name = "First" };
        var secondList = new TodoList { Name = "Second" };
        context.TodoLists.AddRange(firstList, secondList);
        context.TodoItems.AddRange(
            new TodoItem { TodoList = firstList, Title = "One" },
            new TodoItem { TodoList = secondList, Title = "Two" });
        await context.SaveChangesAsync();
        var service = new TodoItemService(context);

        var result = await service.GetByListIdAsync(firstList.Id);

        result.Should().ContainSingle().Which.Title.Should().Be("One");
    }

    [Fact]
    public async Task ToggleCompletedAsync_TogglesStateAndCompletedAt()
    {
        await using var context = ServiceTestFixture.CreateContext();
        var service = new TodoItemService(context);
        var item = await service.AddAsync(new TodoItem { TodoListId = 1, Title = "Finish tests" });

        var completed = await service.ToggleCompletedAsync(item.Id);
        completed!.IsCompleted.Should().BeTrue();
        completed.CompletedAt.Should().NotBeNull();

        var reopened = await service.ToggleCompletedAsync(item.Id);
        reopened!.IsCompleted.Should().BeFalse();
        reopened.CompletedAt.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_WithMissingId_ReturnsFalse()
    {
        await using var context = ServiceTestFixture.CreateContext();
        var service = new TodoItemService(context);

        (await service.DeleteAsync(999)).Should().BeFalse();
    }
}
