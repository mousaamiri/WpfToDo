using FluentAssertions;
using NSubstitute;
using WpfToDo.Core.Models;
using WpfToDo.Core.Services;
using WpfToDo.Core.ViewModels;

namespace WpfToDo.Tests.ViewModels;

public class TodoListsViewModelTests
{
    [Fact]
    public async Task LoadAsync_PopulatesWrappedLists()
    {
        var service = Substitute.For<ITodoListService>();
        service.GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<TodoList>>(
                [new TodoList { Id = 1, Name = "Inbox" }]));
        var viewModel = new TodoListsViewModel(service);

        await viewModel.LoadAsync();

        viewModel.Lists.Should().ContainSingle();
        viewModel.Lists[0].Name.Should().Be("Inbox");
    }

    [Fact]
    public async Task AddListAsync_WithValidName_AddsAndClearsInput()
    {
        var service = Substitute.For<ITodoListService>();
        service.AddAsync(Arg.Any<TodoList>(), Arg.Any<CancellationToken>())
            .Returns(call =>
            {
                var list = call.Arg<TodoList>();
                list.Id = 5;
                return Task.FromResult(list);
            });
        var viewModel = new TodoListsViewModel(service) { NewListName = "  Work  " };

        var result = await viewModel.AddListAsync();

        result!.Name.Should().Be("Work");
        viewModel.Lists.Should().ContainSingle();
        viewModel.NewListName.Should().BeEmpty();
        viewModel.SelectedList.Should().Be(result);
    }

    [Fact]
    public async Task DeleteListAsync_WithSelectedList_RemovesIt()
    {
        var service = Substitute.For<ITodoListService>();
        service.DeleteAsync(1, Arg.Any<CancellationToken>()).Returns(Task.FromResult(true));
        var viewModel = new TodoListsViewModel(service);
        await viewModel.LoadAsync();
        var list = new WpfToDo.Core.Wrappers.TodoListWrapper(new TodoList { Id = 1, Name = "Inbox" });
        viewModel.Lists.Add(list);
        viewModel.SelectedList = list;

        var result = await viewModel.DeleteListAsync();

        result.Should().BeTrue();
        viewModel.Lists.Should().BeEmpty();
        viewModel.SelectedList.Should().BeNull();
    }
}
