using System;

namespace DotEngine.BB
{
    public delegate void BlackboardCollectorChanged<TKey>(
        IBlackboardWatcher<TKey> collector,
        BlackboardAction action,
        TKey key,
        object oldValue,
        object newValue);

    public interface IBlackboardWatcher<TKey>
    {
        event BlackboardCollectorChanged<TKey> onCollectorChanged;

        IBlackboard<TKey> blackboard { get; }
        TKey[] interestedKeys { get; }
        BlackboardAction[] interestedActions { get; }
    }
}
