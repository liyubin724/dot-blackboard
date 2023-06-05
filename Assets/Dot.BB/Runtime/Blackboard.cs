using System.Collections.Generic;
using System.Linq;

namespace DotEngine.BB
{
    public class Blackboard<TKey> : IBlackboard<TKey>
    {
        public event BlackboardValueChanged<TKey> onValueAdded;
        public event BlackboardValueChanged<TKey> onValueUpdated;
        public event BlackboardValueChanged<TKey> onValueRemoved;

        private Dictionary<TKey, object> m_ItemDic = null;

        private TKey[] m_CachedKeys = null;
        public TKey[] keys
        {
            get
            {
                if (m_CachedKeys == null)
                {
                    m_CachedKeys = m_ItemDic.Keys.ToArray();
                }
                return m_CachedKeys;
            }
        }

        public Blackboard() : this(EqualityComparer<TKey>.Default)
        {

        }

        public Blackboard(IEqualityComparer<TKey> comparer)
        {
            m_ItemDic = new Dictionary<TKey, object>(comparer);
        }

        public bool ContainsKey(TKey key)
        {
            return m_ItemDic.ContainsKey(key);
        }

        public object GetValue(TKey key)
        {
            if (!m_ItemDic.TryGetValue(key, out object value))
            {
                throw new BlackboardKeyNotFoundException(key);
            }

            return value;
        }

        public TValue GetValue<TValue>(TKey key)
        {
            if (!m_ItemDic.TryGetValue(key, out object value) || value == null)
            {
                throw new BlackboardKeyNotFoundException(key);
            }

            if (value is TValue result)
            {
                return result;
            }

            throw new BlackboardValueNotCastException(key, typeof(TValue), value);
        }

        public bool TryGetValue(TKey key, out object value)
        {
            if (m_ItemDic.TryGetValue(key, out value))
            {
                return true;
            }

            value = null;
            return false;
        }

        public bool TryGetValue<TValue>(TKey key, out TValue value)
        {
            if (!m_ItemDic.TryGetValue(key, out var item) || item == null)
            {
                value = default(TValue);
                return false;

            }
            if (item is TValue result)
            {
                value = result;
                return true;
            }

            value = default(TValue);
            return false;
        }

        public void AddValue(TKey key, object value)
        {
            if (m_ItemDic.ContainsKey(key))
            {
                throw new BlackboardKeyRepeatedException(key);
            }

            m_CachedKeys = null;
            m_ItemDic.Add(key, value);

            onValueAdded?.Invoke(this, key, null, value);
        }

        public bool UpdateValue(TKey key, object value)
        {
            if (!ContainsKey(key))
            {
                return false;
            }

            var oldValue = m_ItemDic[key];
            if (oldValue != value)
            {
                m_ItemDic[key] = value;

                onValueUpdated?.Invoke(this, key, oldValue, value);

                return true;
            }

            return false;
        }

        public void AddOrUpdateValue(TKey key, object value)
        {
            if (!ContainsKey(key))
            {
                m_CachedKeys = null;
                m_ItemDic.Add(key, value);

                onValueAdded?.Invoke(this, key, null, value);
            }
            else
            {
                var oldValue = m_ItemDic[key];
                if (oldValue != value)
                {
                    m_ItemDic[key] = value;

                    onValueUpdated?.Invoke(this, key, oldValue, value);
                }
            }
        }

        public bool RemoveValue(TKey key)
        {
            if (!ContainsKey(key))
            {
                return false;
            }

            m_CachedKeys = null;
            var oldValue = m_ItemDic[key];
            m_ItemDic.Remove(key);

            onValueRemoved?.Invoke(this, key, oldValue, null);

            return true;
        }

        public void Clear()
        {
            m_CachedKeys = null;
            m_ItemDic.Clear();
        }
    }
}
