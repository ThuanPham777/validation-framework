using ValidationFramework.Core;

namespace ValidationFramework.Notification;

public sealed class NotificationPublisher
{
    private readonly Dictionary<ValidationEventType, List<IValidationNotifierSubscriber>> _listeners = new();

    public void Subscribe(ValidationEventType eventType, IValidationNotifierSubscriber listener)
    {
        if (listener == null) throw new ArgumentNullException(nameof(listener));
        if (!_listeners.TryGetValue(eventType, out var list))
        {
            list = new List<IValidationNotifierSubscriber>();
            _listeners[eventType] = list;
        }
        if (!list.Contains(listener))
            list.Add(listener);
    }

    public void Unsubscribe(ValidationEventType eventType, IValidationNotifierSubscriber listener)
    {
        if (listener == null) return;
        if (_listeners.TryGetValue(eventType, out var list))
            list.Remove(listener);
    }

    public void Notify(ValidationEventType eventType, List<ValidationResult> results)
    {
        if (!_listeners.TryGetValue(eventType, out var list) || list.Count == 0)
            return;

        // Defensive copy in case a listener modifies subscriptions
        foreach (var l in list.ToArray())
            l.Notify(results);
    }
}
