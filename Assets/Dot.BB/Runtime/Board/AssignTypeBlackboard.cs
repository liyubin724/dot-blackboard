using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.BB
{
    public class AssignTypeBlackboard : TypeBlackboard
    {
        public override bool ContainsKey(Type key)
        {
            if (itemDic.ContainsKey(key)) return true;

            foreach (var kvp in itemDic)
            {
                if (key.IsAssignableFrom(kvp.Key))
                {
                    return true;
                }
            }

            return false;
        }

        public override bool TryGetValue(Type key, out object value)
        {
            return TryGetValue(key, out value, out _);
        }

        public override void RemoveValue(Type key)
        {
            if (!TryGetValue(key, out var oldValue, out var savedType))
            {
                throw new BlackboardKeyNotFoundException(key);
            }

            cachedKeys = null;
            itemDic.Remove(key);

            //onValueRemoved(this, key, oldValue, null);
        }

        private bool TryGetValue(Type key, out object value, out Type savedType)
        {
            if (itemDic.TryGetValue(key, out value))
            {
                savedType = key;
                return true;
            }

            foreach (var kvp in itemDic)
            {
                if (key.IsAssignableFrom(kvp.Key))
                {
                    value = kvp.Value;
                    savedType = kvp.Key;
                    return true;
                }
            }

            value = null;
            savedType = null;
            return false;
        }
    }
}
