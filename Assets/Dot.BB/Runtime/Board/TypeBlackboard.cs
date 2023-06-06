using System;

namespace DotEngine.BB
{
    public class TypeBlackboard : Blackboard<Type>
    {
        public TValue GetValue<TValue>()
        {
            return GetValue<TValue>(typeof(TValue));
        }

        public bool TryGetValue<TValue>(out TValue value)
        {
            return TryGetValue<TValue>(typeof(TValue), out value);
        }

        public void AddValue<TValue>(TValue value)
        {
            AddValue(typeof(TValue), value);
        }

        public bool UpdateValue<TValue>(TValue value)
        {
            return UpdateValue(typeof(TValue), value);
        }

        public void AddOrUpdateValue<TValue>(TValue value)
        {
            AddOrUpdateValue(typeof(TValue), value);
        }

        public bool RemoveValue<TValue>()
        {
            return RemoveValue(typeof(TValue));
        }
    }
}
