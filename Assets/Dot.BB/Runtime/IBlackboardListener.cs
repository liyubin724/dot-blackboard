namespace DotEngine.BB
{
    public interface IBlackboardListener<TKey>
    {
        TKey[] ListInterestedBlackboardKeys();
        void OnHandleBlackboardChanged(
            IBlackboard<TKey> blackboard,
            TKey key,
            object oldValue,
            object newValue);
    }
}
