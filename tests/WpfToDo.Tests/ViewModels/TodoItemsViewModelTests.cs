using FluentAssertions;
using NSubstitute;
using WpfToDo.Core.Models;
using WpfToDo.Core.Services;
using WpfToDo.Core.ViewModels;
using WpfToDo.Core.Wrappers;

namespace WpfToDo.Tests.ViewModels;

public class TodoItemsViewModelTests
{
    [Fact]
    public async Task LoadAsync_ForList_PopulatesOnlyThatListItems()
    {
        var service = Substitute.For<ITodoItemService>();
        service.GetByListIdAsync(7, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<TodoItem>>(
                [new TodoItem { Id = 1, TodoListId = 7, Title = "Task" }]));
        var viewModel = new TodoItemsViewModel(service);

        await viewModel.LoadAsync(7);

        viewModel.TodoListId.Should().Be(7);
        viewModel.Items.Should().ContainSingle().Which.Title.Should().Be("Task");
    }

    [Fact]
    public async Task AddItemAsync_WithValidTitle_AddsItemToCurrentList()
    {
        var service = Substitute.For<ITodoItemService>();
        service.AddAsync(Arg.Any<TodoItem>(), Arg.Any<CancellationToken>())
            .Returns(call =>
            {
                var item = call.Arg<TodoItem>();
                item.Id = 3;
                return Task.FromResult(item);
            });
        var viewModel = new TodoItemsViewModel(service);
        await viewModel.LoadAsync(7);

        var result = await viewModel.AddItemAsync("  Task  ");

        result!.Title.Should().Be("Task");
        result.TodoListId.Should().Be(7);
        viewModel.Items.Should().ContainSingle();
        viewModel.NewItemTitle.Should().BeEmpty();
    }

    [Fact]
    public async Task ToggleCompletedAsync_ReplacesItemWithUpdatedState()
    {
        var service = Substitute.For<ITodoItemService>();
        var item = new TodoItem { Id = 3, TodoListId = 7, Title = "Task" };
        var updated = new TodoItem
        {
            Id = 3, TodoListId = 7, Title = "Task", IsCompleted = true, CompletedAt = DateTime.UtcNow
        };
        service.ToggleCompletedAsync(3, Arg.Any<CancellationToken>()).Returns(Task.FromResult<TodoItem?>(updated));
        var viewModel = new TodoItemsViewModel(service);
        await viewModel.LoadAsync(7);
        var wrapper = new TodoItemWrapper(item);
        viewModel.Items.Add(wrapper);
        viewModel.SelectedItem = wrapper;

        var result = await viewModel.ToggleCompletedAsync(wrapper);

        result.Should().BeTrue();
        viewModel.Items.Single().IsCompleted.Should().BeTrue();
        viewModel.SelectedItem.Should().Be(viewModel.Items.Single());
    }
}
