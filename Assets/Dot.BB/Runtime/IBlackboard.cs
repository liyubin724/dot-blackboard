namespace DotEngine.BB
{
    public enum EBlackboardAction
    {
        Add = 0,
        Remove,
        Update,
    }

    public delegate void BlackboardChanged<TKey>(
        IBlackboard<TKey> blackboard,
        EBlackboardAction action,
        TKey key,
        object oldValue,
        object newValue);

    public interface IBlackboard<TKey>
    {
        TKey[] keys { get; }

        bool ContainsKey(TKey key);

        object GetValue(TKey key);
        TValue GetValue<TValue>(TKey key);

        bool TryGetValue(TKey key, out object value);
        bool TryGetValue<TValue>(TKey key, out TValue value);

        void AddValue(TKey key, object value);
        bool UpdateValue(TKey key, object newValue);
        bool RemoveValue(TKey key);

        void Clear();

        void RegisterListener(TKey key, BlackboardChanged<TKey> listener);
        void UnregisterListener(TKey key, BlackboardChanged<TKey> listener);
        void UnregisterAllListener(TKey key);
        void UnregisterAllListner();
    }
}
