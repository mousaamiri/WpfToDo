using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace WpfToDo.Core.Wrappers;

public class ChangeTrackingCollection<T> : ObservableCollection<T>, IValidatableTrackingObject
    where T : class, IValidatableTrackingObject
{
    private readonly IList<T> _initialCollection;
    private readonly ObservableCollection<T> _addedItems = new();
    private readonly ObservableCollection<T> _removedItems = new();
    private readonly ObservableCollection<T> _modifiedItems = new();

    public ChangeTrackingCollection(IEnumerable<T> items)
        : base(items)
    {
        _initialCollection = this.ToList();
        AddedItems = new ReadOnlyObservableCollection<T>(_addedItems);
        RemovedItems = new ReadOnlyObservableCollection<T>(_removedItems);
        ModifiedItems = new ReadOnlyObservableCollection<T>(_modifiedItems);
        AttachItemHandlers(_initialCollection);
    }

    public ReadOnlyObservableCollection<T> AddedItems { get; }
    public ReadOnlyObservableCollection<T> RemovedItems { get; }
    public ReadOnlyObservableCollection<T> ModifiedItems { get; }
    public bool IsChanged => _addedItems.Count > 0 || _removedItems.Count > 0 || _modifiedItems.Count > 0;
    public bool IsValid => this.All(x => x.IsValid);

    public void AcceptChanges()
    {
        _addedItems.Clear();
        _removedItems.Clear();
        _modifiedItems.Clear();
        _initialCollection.Clear();

        foreach (var item in this)
        {
            item.AcceptChanges();
            _initialCollection.Add(item);
        }

        OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsChanged)));
    }

    public void RejectChanges()
    {
        foreach (var item in _modifiedItems.ToList())
            item.RejectChanges();
        foreach (var item in _addedItems.ToList())
            Remove(item);
        foreach (var item in _removedItems.ToList())
        {
            item.RejectChanges();
            Add(item);
        }

        OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsChanged)));
    }

    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        var added = this.Where(x => !_initialCollection.Contains(x)).ToList();
        var removed = _initialCollection.Where(x => !Contains(x)).ToList();
        var modified = this.Except(added).Except(removed).Where(x => x.IsChanged).ToList();

        ReplaceContents(_addedItems, added);
        ReplaceContents(_removedItems, removed);
        ReplaceContents(_modifiedItems, modified);

        if (e.OldItems is not null)
            DetachItemHandlers(e.OldItems.Cast<T>());
        if (e.NewItems is not null)
            AttachItemHandlers(e.NewItems.Cast<T>());

        OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsChanged)));
        base.OnCollectionChanged(e);
    }

    private void AttachItemHandlers(IEnumerable<T> items)
    {
        foreach (var item in items)
            item.PropertyChanged += OnItemPropertyChanged;
    }

    private void DetachItemHandlers(IEnumerable<T> items)
    {
        foreach (var item in items)
            item.PropertyChanged -= OnItemPropertyChanged;
    }

    private void OnItemPropertyChanged(object? sender, PropertyChangedEventArgs args)
    {
        if (sender is not T item || _addedItems.Contains(item))
            return;

        if (item.IsChanged && !_modifiedItems.Contains(item))
            _modifiedItems.Add(item);
        else if (!item.IsChanged)
            _modifiedItems.Remove(item);

        OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsChanged)));
    }

    private static void ReplaceContents(ObservableCollection<T> target, IEnumerable<T> items)
    {
        target.Clear();
        foreach (var item in items)
            target.Add(item);
    }
}
