using System;
using System.Collections.Generic;
using System.Linq;

namespace DotEngine.BB
{
    public class BlackboardCollector<TKey> : IBlackboardCollector<TKey>
    {
        public IBlackboard<TKey> blackboard { get; private set; }
        public TKey[] interestedKeys { get; private set; }
        public BlackboardAction[] interestedActions { get; private set; }

        private HashSet<TKey> m_CollectedKeySet = new HashSet<TKey>();
        private TKey[] m_CachedCollectedKeys = null;
        private Dictionary<TKey, BlackboardAction> m_KeyToActionDic = new Dictionary<TKey, BlackboardAction>();

        public BlackboardCollector(IBlackboard<TKey> blackboard, TKey interestedKey, BlackboardAction interestedAction)
            : this(blackboard, new[] { interestedKey }, new[] { interestedAction })
        {
        }

        public BlackboardCollector(
            IBlackboard<TKey> blackboard,
            TKey[] interestedKeys,
            BlackboardAction[] interestedActions)
        {
            this.blackboard = blackboard;
            this.interestedKeys = interestedKeys;
            this.interestedActions = interestedActions;

            if (this.interestedKeys.Length != this.interestedActions.Length)
            {
                throw new ArgumentException("the length is not equal");
            }

            bool isNeedAdded = false;
            bool isNeedUpdated = false;
            bool isNeedRemoved = false;
            for (int i = 0; i < this.interestedKeys.Length; i++)
            {
                var key = this.interestedKeys[i];
                var action = this.interestedActions[i];

                m_KeyToActionDic.Add(key, action);
                if (!isNeedAdded && (action & BlackboardAction.Add) > 0)
                {
                    isNeedAdded = true;
                }
                if (!isNeedUpdated && (action & BlackboardAction.Update) > 0)
                {
                    isNeedAdded = true;
                }
                if (!isNeedRemoved && (action & BlackboardAction.Remove) > 0)
                {
                    isNeedRemoved = true;
                }
            }
            if (isNeedAdded)
            {
                this.blackboard.onValueAdded += OnValueAdded;
            }
            if (isNeedUpdated)
            {
                this.blackboard.onValueUpdated += OnValueUpdated;
            }
            if (isNeedRemoved)
            {
                this.blackboard.onValueRemoved += OnValueRemoved;
            }
        }

        ~BlackboardCollector()
        {
            if (blackboard != null)
            {
                blackboard.onValueAdded -= OnValueAdded;
                blackboard.onValueUpdated -= OnValueUpdated;
                blackboard.onValueRemoved -= OnValueRemoved;
            }

        }

        public TKey[] GetCollectedKeys()
        {
            if (m_CachedCollectedKeys == null)
            {
                m_CachedCollectedKeys = m_CollectedKeySet.ToArray();
            }
            return m_CachedCollectedKeys;
        }
        public void ClearCollectedKeys()
        {
            m_CollectedKeySet.Clear();
            m_CachedCollectedKeys = null;
        }

        private void OnValueAdded(
            IBlackboard<TKey> blackboard,
            TKey key,
            object oldValue,
            object newValue)
        {
            if (!m_KeyToActionDic.TryGetValue(key, out var action) || (action & BlackboardAction.Add) <= 0)
            {
                return;
            }
            m_CachedCollectedKeys = null;
            m_CollectedKeySet.Add(key);
        }

        private void OnValueRemoved(
            IBlackboard<TKey> blackboard,
            TKey key,
            object oldValue,
            object newValue)
        {
            if (!m_KeyToActionDic.TryGetValue(key, out var action) || (action & BlackboardAction.Update) <= 0)
            {
                return;
            }
            m_CachedCollectedKeys = null;
            m_CollectedKeySet.Add(key);
        }

        private void OnValueUpdated(
            IBlackboard<TKey> blackboard,
            TKey key,
            object oldValue,
            object newValue)
        {
            if (!m_KeyToActionDic.TryGetValue(key, out var action) || (action & BlackboardAction.Remove) <= 0)
            {
                return;
            }
            m_CachedCollectedKeys = null;
            m_CollectedKeySet.Add(key);
        }
    }
}
