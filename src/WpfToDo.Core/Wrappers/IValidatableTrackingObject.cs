using System.ComponentModel;

namespace WpfToDo.Core.Wrappers;

public interface IValidatableTrackingObject : IRevertibleChangeTracking, INotifyPropertyChanged
{
    bool IsValid { get; }
}
