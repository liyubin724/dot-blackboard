namespace DotEngine.BB
{
    public delegate void BlackboardValueChanged<TKey>(
        IBlackboard<TKey> blackboard,
        TKey key,
        object oldValue,
        object newValue);

    public interface IBlackboard<TKey>
    {
        event BlackboardValueChanged<TKey> onValueAdded;
        event BlackboardValueChanged<TKey> onValueUpdated;
        event BlackboardValueChanged<TKey> onValueRemoved;

        TKey[] keys { get; }

        bool ContainsKey(TKey key);

        object GetValue(TKey key);
        TValue GetValue<TValue>(TKey key);

        bool TryGetValue(TKey key, out object value);
        bool TryGetValue<TValue>(TKey key, out TValue value);

        void AddValue(TKey key, object value);
        bool UpdateValue(TKey key, object newValue);
        void AddOrUpdateValue(TKey key, object value);
        bool RemoveValue(TKey key);

        void Clear();
    }
}
