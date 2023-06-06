using System;
using System.Collections.Generic;

namespace DotEngine.BB.Injector
{
    public static class InjectReflection
    {
        private static Dictionary<Type, InjectReflectionTypeInfo> sm_TypeInfoDic = new Dictionary<Type, InjectReflectionTypeInfo>();

        internal static InjectReflectionTypeInfo GetTypeInfo(Type type)
        {
            if (sm_TypeInfoDic.TryGetValue(type, out var typeInfo))
            {
                return typeInfo;
            }

            RegisterTypeInfo(type);

            return sm_TypeInfoDic[type];
        }

        public static void RegisterTypeInfo(Type type, bool isDelayReflect = true)
        {
            if (!sm_TypeInfoDic.ContainsKey(type))
            {
                InjectReflectionTypeInfo typeInfo = new InjectReflectionTypeInfo(type);
                if (!isDelayReflect)
                {
                    typeInfo.ReflectMemebers();
                }
                sm_TypeInfoDic.Add(type, typeInfo);
            }
        }

        public static void UnregisterTypeInfo(Type type)
        {
            sm_TypeInfoDic.Remove(type);
        }

        public static void Clear()
        {
            sm_TypeInfoDic.Clear();
        }
    }
}
