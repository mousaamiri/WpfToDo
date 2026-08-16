using System.ComponentModel.DataAnnotations;
using WpfToDo.Core.Models;

namespace WpfToDo.Core.Wrappers;

public sealed class CategoryWrapper : ModelWrapper<Category>
{
    public CategoryWrapper(Category model) : base(model)
    {
    }

    public int Id
    {
        get => GetValue<int>();
        set => SetValue(value);
    }

    public bool IdIsChanged => GetIsChanged(nameof(Id));
    public int? IdOriginalValue => GetOriginalValue<int>(nameof(Id));

    [Required(ErrorMessage = "نام دسته‌بندی نمی‌تواند خالی باشد.")]
    [MaxLength(50, ErrorMessage = "نام دسته‌بندی نمی‌تواند بیش از {1} کاراکتر باشد.")]
    public string Name
    {
        get => GetValue<string>() ?? string.Empty;
        set => SetValue(value);
    }

    public bool NameIsChanged => GetIsChanged(nameof(Name));
    public string? NameOriginalValue => GetOriginalValue<string>(nameof(Name));

    public string ColorKey
    {
        get => GetValue<string>() ?? string.Empty;
        set => SetValue(value);
    }

    public bool ColorKeyIsChanged => GetIsChanged(nameof(ColorKey));
    public string? ColorKeyOriginalValue => GetOriginalValue<string>(nameof(ColorKey));
}
