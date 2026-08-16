using System.Collections;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WpfToDo.Core.Wrappers;

public class NotifyDataErrorInfoBase : ObservableObject, INotifyDataErrorInfo
{
    protected readonly Dictionary<string, List<string>> Errors = new();

    public bool HasErrors => Errors.Count > 0;

    public IEnumerable GetErrors(string? propertyName)
        => string.IsNullOrEmpty(propertyName) || !Errors.TryGetValue(propertyName, out var errors)
            ? Enumerable.Empty<string>()
            : errors;

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    protected void ClearErrors()
    {
        foreach (var propertyName in Errors.Keys.ToList())
        {
            Errors.Remove(propertyName);
            OnErrorsChanged(propertyName);
        }
    }

    protected void SetErrors(string propertyName, IEnumerable<string> errors)
    {
        var distinctErrors = errors.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToList();
        if (distinctErrors.Count == 0)
        {
            if (Errors.Remove(propertyName))
                OnErrorsChanged(propertyName);
            return;
        }

        Errors[propertyName] = distinctErrors;
        OnErrorsChanged(propertyName);
    }

    protected void OnErrorsChanged(string propertyName)
        => ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
}
