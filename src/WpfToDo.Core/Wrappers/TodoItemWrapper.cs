using System.ComponentModel.DataAnnotations;
using WpfToDo.Core.Models;

namespace WpfToDo.Core.Wrappers;

public sealed class TodoItemWrapper : ModelWrapper<TodoItem>
{
    public TodoItemWrapper(TodoItem model) : base(model)
    {
    }

    public int Id
    {
        get => GetValue<int>();
        set => SetValue(value);
    }

    public bool IdIsChanged => GetIsChanged(nameof(Id));
    public int? IdOriginalValue => GetOriginalValue<int>(nameof(Id));

    public int TodoListId
    {
        get => GetValue<int>();
        set => SetValue(value);
    }

    public bool TodoListIdIsChanged => GetIsChanged(nameof(TodoListId));
    public int? TodoListIdOriginalValue => GetOriginalValue<int>(nameof(TodoListId));

    [Required(ErrorMessage = "عنوان نمی‌تواند خالی باشد.")]
    [MaxLength(200, ErrorMessage = "عنوان نمی‌تواند بیش از {1} کاراکتر باشد.")]
    public string Title
    {
        get => GetValue<string>() ?? string.Empty;
        set => SetValue(value);
    }

    public bool TitleIsChanged => GetIsChanged(nameof(Title));
    public string? TitleOriginalValue => GetOriginalValue<string>(nameof(Title));

    public string? Notes
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public bool NotesIsChanged => GetIsChanged(nameof(Notes));
    public string? NotesOriginalValue => GetOriginalValue<string>(nameof(Notes));

    public bool IsCompleted
    {
        get => GetValue<bool>();
        set => SetValue(value);
    }

    public bool IsCompletedIsChanged => GetIsChanged(nameof(IsCompleted));
    public bool? IsCompletedOriginalValue => GetOriginalValue<bool>(nameof(IsCompleted));

    public DateTime? DueDate
    {
        get => GetValue<DateTime?>();
        set => SetValue(value);
    }

    public bool DueDateIsChanged => GetIsChanged(nameof(DueDate));
    public DateTime? DueDateOriginalValue => GetOriginalValue<DateTime?>(nameof(DueDate));

    public DateTime CreatedAt
    {
        get => GetValue<DateTime>();
        set => SetValue(value);
    }

    public bool CreatedAtIsChanged => GetIsChanged(nameof(CreatedAt));
    public DateTime CreatedAtOriginalValue => GetOriginalValue<DateTime>(nameof(CreatedAt));

    public DateTime? CompletedAt
    {
        get => GetValue<DateTime?>();
        set => SetValue(value);
    }

    public bool CompletedAtIsChanged => GetIsChanged(nameof(CompletedAt));
    public DateTime? CompletedAtOriginalValue => GetOriginalValue<DateTime?>(nameof(CompletedAt));
}
