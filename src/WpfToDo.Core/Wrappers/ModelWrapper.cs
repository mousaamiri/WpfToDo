using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace WpfToDo.Core.Wrappers;

public abstract class ModelWrapper<T> : NotifyDataErrorInfoBase, IValidatableTrackingObject, IValidatableObject
{
    private readonly T _model;
    private readonly Dictionary<string, object?> _originalValues = new();
    private readonly List<IValidatableTrackingObject> _trackedObjects = new();

    protected ModelWrapper(T model)
    {
        _model = model ?? throw new ArgumentNullException(nameof(model));
        InitializeComplexProperties(model);
        InitializeCollectionProperties(model);
        Validate();
    }

    public T Model => _model;
    public bool IsChanged => _originalValues.Count > 0 || _trackedObjects.Any(x => x.IsChanged);
    public bool IsValid => !HasErrors && _trackedObjects.All(x => x.IsValid);

    public void AcceptChanges()
    {
        _originalValues.Clear();
        foreach (var trackedObject in _trackedObjects)
            trackedObject.AcceptChanges();

        OnPropertyChanged(string.Empty);
    }

    public void RejectChanges()
    {
        foreach (var propertyName in _originalValues.Keys.ToList())
            typeof(T).GetProperty(propertyName)?.SetValue(_model, _originalValues[propertyName]);

        _originalValues.Clear();
        foreach (var trackedObject in _trackedObjects)
            trackedObject.RejectChanges();

        Validate();
        OnPropertyChanged(string.Empty);
    }

    public TValue? GetOriginalValue<TValue>(string propertyName)
        => _originalValues.TryGetValue(propertyName, out var value)
            ? (TValue?)value
            : GetValue<TValue>(propertyName);

    public bool GetIsChanged(string propertyName) => _originalValues.ContainsKey(propertyName);

    protected virtual void InitializeComplexProperties(T model)
    {
    }

    protected virtual void InitializeCollectionProperties(T model)
    {
    }

    protected void RegisterComplexProperties<TModel>(TModel model)
        where TModel : IValidatableTrackingObject
        => RegisterChangeTrackingObject(model);

    protected void RegisterCollectionProperties<TWrapper, TModel>(
        ChangeTrackingCollection<TWrapper> wrapperCollection,
        ICollection<TModel> modelCollection)
        where TWrapper : ModelWrapper<TModel>
    {
        wrapperCollection.CollectionChanged += (_, _) =>
        {
            modelCollection.Clear();
            foreach (var model in wrapperCollection.Select(x => x.Model))
                modelCollection.Add(model);
            Validate();
        };
        RegisterChangeTrackingObject(wrapperCollection);
    }

    protected virtual void SetValue<TValue>(
        TValue newValue,
        [CallerMemberName] string? propertyName = null)
    {
        if (propertyName is null)
            return;

        var property = typeof(T).GetProperty(propertyName)
            ?? throw new ArgumentException($"Property {propertyName} does not exist on {typeof(T).Name}.");
        var currentValue = (TValue?)property.GetValue(_model);
        if (Equals(currentValue, newValue))
            return;

        if (!_originalValues.ContainsKey(propertyName))
        {
            _originalValues[propertyName] = currentValue;
            OnPropertyChanged(nameof(IsChanged));
        }
        else if (Equals(newValue, _originalValues[propertyName]))
        {
            _originalValues.Remove(propertyName);
            OnPropertyChanged(nameof(IsChanged));
        }

        property.SetValue(_model, newValue);
        Validate();
        OnPropertyChanged(propertyName);
        OnPropertyChanged($"{propertyName}IsChanged");
    }

    protected virtual TValue? GetValue<TValue>(
        [CallerMemberName] string? propertyName = null)
    {
        if (propertyName is null)
            return default;

        var property = typeof(T).GetProperty(propertyName)
            ?? throw new ArgumentException($"Property {propertyName} does not exist on {typeof(T).Name}.");
        return (TValue?)property.GetValue(_model);
    }

    protected virtual void Validate()
    {
        var validationResults = new List<ValidationResult>();
        Validator.TryValidateObject(
            this,
            new ValidationContext(this),
            validationResults,
            validateAllProperties: true);

        var propertyNames = validationResults
            .SelectMany(x => x.MemberNames)
            .Distinct()
            .ToHashSet(StringComparer.Ordinal);

        foreach (var oldPropertyName in Errors.Keys.ToList())
        {
            if (!propertyNames.Contains(oldPropertyName))
                SetErrors(oldPropertyName, []);
        }

        foreach (var propertyName in propertyNames)
        {
            var messages = validationResults
                .Where(x => x.MemberNames.Contains(propertyName))
                .Select(x => x.ErrorMessage ?? "Invalid value.");
            SetErrors(propertyName, messages);
        }

        OnPropertyChanged(nameof(IsValid));
    }

    public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        => [];

    private void RegisterChangeTrackingObject(IValidatableTrackingObject trackingObject)
    {
        if (_trackedObjects.Contains(trackingObject))
            return;

        _trackedObjects.Add(trackingObject);
        trackingObject.PropertyChanged += OnTrackedObjectPropertyChanged;
    }

    private void OnTrackedObjectPropertyChanged(object? sender, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == nameof(IsChanged))
            OnPropertyChanged(nameof(IsChanged));
        if (args.PropertyName == nameof(IsValid))
            OnPropertyChanged(nameof(IsValid));
    }
}
