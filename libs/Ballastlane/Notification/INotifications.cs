namespace Ballastlane.Notification;

public interface INotifications
{
    bool IsValid { get; }
    void AddNotification(string message);
    IReadOnlyCollection<string> GetNotifications();
    void Clear();
}
