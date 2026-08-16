using FluentAssertions;
using WpfToDo.Core.Models;
using WpfToDo.Core.Wrappers;

namespace WpfToDo.Tests.Wrappers;

public class WrapperTests
{
    [Fact]
    public void TodoListWrapper_WithEmptyName_IsInvalid()
    {
        var wrapper = new TodoListWrapper(new TodoList());

        wrapper.IsValid.Should().BeFalse();
        wrapper.HasErrors.Should().BeTrue();
        wrapper.GetErrors(nameof(TodoListWrapper.Name)).Cast<string>()
            .Should().ContainSingle();
    }

    [Fact]
    public void TodoItemWrapper_WithLongTitle_IsInvalid()
    {
        var wrapper = new TodoItemWrapper(new TodoItem { Title = new string('x', 201) });

        wrapper.IsValid.Should().BeFalse();
        wrapper.GetErrors(nameof(TodoItemWrapper.Title)).Cast<string>()
            .Should().ContainSingle();
    }

    [Fact]
    public void CategoryWrapper_WithValidName_IsValid()
    {
        var wrapper = new CategoryWrapper(new Category { Name = "Personal" });

        wrapper.IsValid.Should().BeTrue();
        wrapper.HasErrors.Should().BeFalse();
    }

    [Fact]
    public void TodoItemWrapper_ChangingThenRejecting_RestoresOriginalValue()
    {
        var wrapper = new TodoItemWrapper(new TodoItem { Title = "Original" });

        wrapper.Title = "Changed";

        wrapper.TitleIsChanged.Should().BeTrue();
        wrapper.TitleOriginalValue.Should().Be("Original");
        wrapper.IsChanged.Should().BeTrue();

        wrapper.RejectChanges();

        wrapper.Title.Should().Be("Original");
        wrapper.TitleIsChanged.Should().BeFalse();
        wrapper.IsChanged.Should().BeFalse();
    }

    [Fact]
    public void TodoListWrapper_ChangingBackToOriginal_ClearsPropertyChange()
    {
        var wrapper = new TodoListWrapper(new TodoList { Name = "Inbox" });

        wrapper.Name = "Changed";
        wrapper.Name = "Inbox";

        wrapper.NameIsChanged.Should().BeFalse();
        wrapper.IsChanged.Should().BeFalse();
    }
}
