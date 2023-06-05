using System.Collections.Generic;
using System.Linq;

namespace DotEngine.BB
{
    public class Blackboard<TKey> : IBlackboard<TKey>
    {
        private Dictionary<TKey, object> m_ItemDic = null;

        private TKey[] m_Keys = null;
        public TKey[] keys
        {
            get
            {
                if (m_Keys == null)
                {
                    m_Keys = m_ItemDic.Keys.ToArray();
                }
                return m_Keys;
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
            if (!m_ItemDic.TryGetValue(key, out object value))
            {
                throw new BlackboardKeyNotFoundException(key);
            }

            if (value != null && value is TValue result)
            {
                return result;
            }

            throw new BlackboardKeyNotFoundException(key);
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
            if (m_ItemDic.TryGetValue(key, out var item))
            {
                if (item != null && item is TValue result)
                {
                    value = result;
                    return true;
                }
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

            m_ItemDic.Add(key, value);
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
                return true;
            }

            return false;
        }

        public bool RemoveValue(TKey key)
        {
            if (!ContainsKey(key))
            {
                return false;
            }

            var oldValue = m_ItemDic[key];

            m_ItemDic.Remove(key);
            return true;
        }

        public void Clear()
        {
            m_ItemDic.Clear();
        }

        public void RegisterListener(TKey key, BlackboardChanged<TKey> listener) { }
        public void UnregisterListener(TKey key, BlackboardChanged<TKey> listener) { }
        public void UnregisterAllListener(TKey key) { }
        public void UnregisterAllListner() { }
    }
}
