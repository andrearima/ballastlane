namespace Ballastlane.Integration.Tests.Order;

internal class PriorityAttribute : Attribute
{
    public PriorityAttribute(int priority)
    {
        Priority = priority;
    }

    public int Priority { get; }
}
