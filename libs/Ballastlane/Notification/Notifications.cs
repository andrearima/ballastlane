using System.Collections.ObjectModel;

namespace Ballastlane.Notification;

public class Notifications : INotifications
{
    private readonly List<string> _notifications = [];

    public void AddNotification(string message)
    {
        _notifications.Add(message);
    }

    public bool IsValid => _notifications.Count == 0;

    public IReadOnlyCollection<string> GetNotifications() => new ReadOnlyCollection<string>(_notifications);

    public void Clear()
    {
        _notifications.Clear();
    }
}
