using System.Collections.Generic;
using System.Linq;

namespace DotEngine.BB
{
    public class Blackboard<TKey> : IBlackboard<TKey>
    {
        public event BlackboardValueChanged<TKey> onValueAdded;
        public event BlackboardValueChanged<TKey> onValueUpdated;
        public event BlackboardValueChanged<TKey> onValueRemoved;

        protected Dictionary<TKey, object> itemDic = null;

        protected TKey[] cachedKeys = null;
        public TKey[] keys
        {
            get
            {
                if (cachedKeys == null)
                {
                    cachedKeys = itemDic.Keys.ToArray();
                }
                return cachedKeys;
            }
        }

        public Blackboard() : this(EqualityComparer<TKey>.Default)
        {

        }

        public Blackboard(IEqualityComparer<TKey> comparer)
        {
            itemDic = new Dictionary<TKey, object>(comparer);
        }

        public virtual bool ContainsKey(TKey key)
        {
            return itemDic.ContainsKey(key);
        }

        public virtual object GetValue(TKey key)
        {
            if (!TryGetValue(key, out object value))
            {
                throw new BlackboardKeyNotFoundException(key);
            }

            return value;
        }

        public virtual TValue GetValue<TValue>(TKey key)
        {
            if (!TryGetValue(key, out object value))
            {
                throw new BlackboardKeyNotFoundException(key);
            }

            if (value == null)
            {
                return default(TValue);
            }

            if (value is TValue result)
            {
                return result;
            }

            throw new BlackboardValueNotCastException(key, typeof(TValue), value);
        }

        public virtual bool TryGetValue(TKey key, out object value)
        {
            if (itemDic.TryGetValue(key, out value))
            {
                return true;
            }

            value = null;
            return false;
        }

        public virtual bool TryGetValue<TValue>(TKey key, out TValue value)
        {
            if (!TryGetValue(key, out var item))
            {
                value = default(TValue);
                return false;
            }

            if (item == null)
            {
                value = default(TValue);
                return true;
            }

            if (item is TValue result)
            {
                value = result;
                return true;
            }

            value = default(TValue);
            return false;
        }

        public virtual void AddValue(TKey key, object value)
        {
            if (ContainsKey(key))
            {
                throw new BlackboardKeyRepeatedException(key);
            }

            cachedKeys = null;
            itemDic.Add(key, value);

            onValueAdded?.Invoke(this, key, null, value);
        }

        public virtual void UpdateValue(TKey key, object value)
        {
            if (!TryGetValue(key, out var oldValue))
            {
                throw new BlackboardKeyNotFoundException(key);
            }

            itemDic[key] = value;

            onValueUpdated?.Invoke(this, key, oldValue, value);
        }

        public virtual void AddOrUpdateValue(TKey key, object value)
        {
            if (!ContainsKey(key))
            {
                cachedKeys = null;
                itemDic.Add(key, value);

                onValueAdded?.Invoke(this, key, null, value);
            }
            else
            {
                var oldValue = GetValue(key);
                itemDic[key] = value;

                onValueUpdated?.Invoke(this, key, oldValue, value);
            }
        }

        public virtual void RemoveValue(TKey key)
        {
            if (!TryGetValue(key, out var oldValue))
            {
                throw new BlackboardKeyNotFoundException(key);
            }

            cachedKeys = null;
            itemDic.Remove(key);

            onValueRemoved?.Invoke(this, key, oldValue, null);
        }

        public virtual bool TryRemoveValue(TKey key)
        {
            if (!TryGetValue(key, out var oldValue))
            {
                return false;
            }

            cachedKeys = null;
            itemDic.Remove(key);

            onValueRemoved?.Invoke(this, key, oldValue, null);

            return true;
        }

        public virtual void Clear()
        {
            cachedKeys = null;
            itemDic.Clear();
        }
    }
}
