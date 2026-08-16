using FluentAssertions;
using WpfToDo.Core.Models;

namespace WpfToDo.Tests.Models;

public class DomainModelTests
{
    [Fact]
    public void TodoList_WithDefaults_HasCreatedAtAndEmptyItems()
    {
        var list = new TodoList { Name = "Work" };

        list.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
        list.Items.Should().BeEmpty();
    }

    [Fact]
    public void TodoItem_WithDefaults_IsNotCompleted()
    {
        var item = new TodoItem { Title = "Buy milk" };

        item.IsCompleted.Should().BeFalse();
        item.CompletedAt.Should().BeNull();
        item.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
    }

    [Fact]
    public void Category_HasEmptyTodoListsCollection()
    {
        var category = new Category { Name = "Personal", ColorKey = "Blue" };

        category.TodoLists.Should().BeEmpty();
    }
}
