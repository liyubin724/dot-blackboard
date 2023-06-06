namespace DotEngine.BB.Injector
{
    public class BlackboardInjector<TKey> : IBlackboardInjector<TKey>
    {
        public IBlackboard<TKey> blackboard { get; private set; }

        public BlackboardInjector(IBlackboard<TKey> blackboard)
        {
            this.blackboard = blackboard;
        }

        public bool TryGetValue(TKey key, out object value)
        {
            return blackboard.TryGetValue(key, out value);
        }

        public void AddOrUpdateValue(TKey key, object value)
        {
            blackboard.AddOrUpdateValue(key, value);
        }

        public void InjectTo(object injectObject)
        {
            if (injectObject == null)
            {
                return;
            }

            InjectReflectionTypeInfo typeInfo = InjectReflection.GetTypeInfo(injectObject.GetType());
            typeInfo.InjectTo(this, injectObject);
        }

        public void ExtractFrom(object extractObject)
        {
            if (extractObject == null)
            {
                return;
            }

            InjectReflectionTypeInfo typeInfo = InjectReflection.GetTypeInfo(extractObject.GetType());
            typeInfo.ExtractFrom(this, extractObject);
        }
    }
}
