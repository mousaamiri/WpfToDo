using FluentAssertions;
using WpfToDo.Core.Models;
using WpfToDo.Core.Services;

namespace WpfToDo.Tests.Services;

public class TodoListServiceTests
{
    [Fact]
    public async Task AddAsync_WithValidList_PersistsAndGeneratesId()
    {
        await using var context = ServiceTestFixture.CreateContext();
        var service = new TodoListService(context);

        var result = await service.AddAsync(new TodoList { Name = "Work" });

        result.Id.Should().NotBe(0);
        (await service.GetByIdAsync(result.Id))!.Name.Should().Be("Work");
    }

    [Fact]
    public async Task GetAllAsync_ReturnsListsOrderedByCreatedAt()
    {
        await using var context = ServiceTestFixture.CreateContext();
        context.TodoLists.AddRange(
            new TodoList { Name = "Later", CreatedAt = DateTime.UtcNow.AddMinutes(1) },
            new TodoList { Name = "First", CreatedAt = DateTime.UtcNow });
        await context.SaveChangesAsync();
        var service = new TodoListService(context);

        var result = await service.GetAllAsync();

        result.Select(x => x.Name).Should().ContainInOrder("First", "Later");
    }

    [Fact]
    public async Task UpdateAsync_ChangesPersistedList()
    {
        await using var context = ServiceTestFixture.CreateContext();
        var service = new TodoListService(context);
        var list = await service.AddAsync(new TodoList { Name = "Old" });

        list.Name = "New";
        await service.UpdateAsync(list);

        (await service.GetByIdAsync(list.Id))!.Name.Should().Be("New");
    }

    [Fact]
    public async Task DeleteAsync_WithExistingId_RemovesList()
    {
        await using var context = ServiceTestFixture.CreateContext();
        var service = new TodoListService(context);
        var list = await service.AddAsync(new TodoList { Name = "Temporary" });

        var deleted = await service.DeleteAsync(list.Id);

        deleted.Should().BeTrue();
        (await service.GetByIdAsync(list.Id)).Should().BeNull();
    }
}
