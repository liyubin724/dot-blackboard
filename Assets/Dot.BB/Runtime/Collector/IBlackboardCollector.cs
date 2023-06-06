namespace DotEngine.BB.Collector
{
    public interface IBlackboardCollector<TKey>
    {
        IBlackboard<TKey> blackboard { get; }
        TKey[] interestedKeys { get; }
        BlackboardAction[] interestedActions { get; }

        TKey[] GetCollectedKeys();
        void ClearCollectedKeys();
    }
}
