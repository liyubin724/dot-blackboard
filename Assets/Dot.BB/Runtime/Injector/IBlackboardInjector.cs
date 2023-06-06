namespace DotEngine.BB.Injector
{
    public interface IBlackboardInjector<TKey>
    {
        IBlackboard<TKey> blackboard { get; }

        bool TryGetValue(TKey key, out object value);
        void AddOrUpdateValue(TKey key, object value);

        void InjectTo(object injectObject);
        void ExtractFrom(object extractObject);
    }
}
