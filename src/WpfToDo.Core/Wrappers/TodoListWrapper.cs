using System.ComponentModel.DataAnnotations;
using WpfToDo.Core.Models;

namespace WpfToDo.Core.Wrappers;

public sealed class TodoListWrapper : ModelWrapper<TodoList>
{
    public TodoListWrapper(TodoList model) : base(model)
    {
    }

    public int Id
    {
        get => GetValue<int>();
        set => SetValue(value);
    }

    public bool IdIsChanged => GetIsChanged(nameof(Id));
    public int? IdOriginalValue => GetOriginalValue<int>(nameof(Id));

    [Required(ErrorMessage = "نام لیست نمی‌تواند خالی باشد.")]
    [MaxLength(100, ErrorMessage = "نام لیست نمی‌تواند بیش از {1} کاراکتر باشد.")]
    public string Name
    {
        get => GetValue<string>() ?? string.Empty;
        set => SetValue(value);
    }

    public bool NameIsChanged => GetIsChanged(nameof(Name));
    public string? NameOriginalValue => GetOriginalValue<string>(nameof(Name));

    public int? CategoryId
    {
        get => GetValue<int?>();
        set => SetValue(value);
    }

    public bool CategoryIdIsChanged => GetIsChanged(nameof(CategoryId));
    public int? CategoryIdOriginalValue => GetOriginalValue<int?>(nameof(CategoryId));

    public DateTime CreatedAt
    {
        get => GetValue<DateTime>();
        set => SetValue(value);
    }

    public bool CreatedAtIsChanged => GetIsChanged(nameof(CreatedAt));
    public DateTime CreatedAtOriginalValue => GetOriginalValue<DateTime>(nameof(CreatedAt));
}
