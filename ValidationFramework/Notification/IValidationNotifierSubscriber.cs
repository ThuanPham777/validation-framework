using ValidationFramework.Core;

namespace ValidationFramework.Notification;

public interface IValidationNotifierSubscriber
{
    void Notify(List<ValidationResult> results);
}
