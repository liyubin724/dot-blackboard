using System;
using System.Collections.Generic;

namespace DotEngine.BB
{
    public class TypeBlackboard : Blackboard<Type>
    {
        public TypeBlackboard() : base(new TypeBlackboardComparer())
        { }
    }

    public class TypeBlackboardComparer : IEqualityComparer<Type>
    {
        public bool Equals(Type x, Type y)
        {
            if (x == null || y == null) return false;

            if (x == y) return true;

            if (x.IsAssignableFrom(y))
            {
                return true;
            }

            return false;
        }

        public int GetHashCode(Type obj)
        {
            return obj.GetHashCode();
        }
    }
}
