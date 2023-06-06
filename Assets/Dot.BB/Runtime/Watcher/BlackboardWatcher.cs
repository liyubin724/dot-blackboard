using System;
using System.Collections.Generic;

namespace DotEngine.BB
{
    public class BlackboardWatcher<TKey> : IBlackboardWatcher<TKey>
    {
        public event BlackboardCollectorChanged<TKey> onCollectorChanged;

        public IBlackboard<TKey> blackboard { get; private set; }
        public TKey[] interestedKeys { get; private set; }
        public BlackboardAction[] interestedActions { get; private set; }

        private Dictionary<TKey, BlackboardAction> m_KeyToActionDic = new Dictionary<TKey, BlackboardAction>();

        public BlackboardWatcher(IBlackboard<TKey> blackboard, TKey interestedKey, BlackboardAction interestedAction)
            : this(blackboard, new[] { interestedKey }, new[] { interestedAction })
        {
        }

        public BlackboardWatcher(
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

        ~BlackboardWatcher()
        {
            if (blackboard != null)
            {
                blackboard.onValueAdded -= OnValueAdded;
                blackboard.onValueUpdated -= OnValueUpdated;
                blackboard.onValueRemoved -= OnValueRemoved;
            }

            onCollectorChanged = null;
            blackboard = null;
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

            onCollectorChanged?.Invoke(this, BlackboardAction.Add, key, oldValue, newValue);
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

            onCollectorChanged?.Invoke(this, BlackboardAction.Update, key, oldValue, newValue);
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

            onCollectorChanged?.Invoke(this, BlackboardAction.Remove, key, oldValue, newValue);
        }
    }
}
