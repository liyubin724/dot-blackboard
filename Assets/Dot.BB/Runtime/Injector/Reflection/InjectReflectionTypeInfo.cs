using System;
using System.Collections.Generic;
using System.Reflection;

namespace DotEngine.BB.Injector
{
    internal class InjectReflectionTypeInfo
    {
        private List<InjectMemberInfo> m_InjectMembers = null;
        private List<InjectMemberInfo> m_ExtractMembers = null;

        public Type targetType { get; private set; }

        public InjectReflectionTypeInfo(Type type)
        {
            targetType = type;
        }

        public void ReflectMemebers()
        {
            if (m_InjectMembers != null || m_ExtractMembers != null)
            {
                return;
            }

            m_InjectMembers = new List<InjectMemberInfo>();
            m_ExtractMembers = new List<InjectMemberInfo>();

            ReflectFields();
            ReflectProperties();
        }

        public void InjectTo<TKey>(IBlackboardInjector<TKey> context, object target)
        {
            if (m_InjectMembers == null)
            {
                ReflectMemebers();
            }

            foreach (var mInfo in m_InjectMembers)
            {
                if (!context.TryGetValue((TKey)mInfo.key, out var value))
                {
                    if (!mInfo.isOptional)
                    {
                        throw new InjectValueNotFoundException(mInfo.key);
                    }
                    continue;
                }
                mInfo.SetValue(target, value);
            }
        }

        public void ExtractFrom<K>(IBlackboardInjector<K> context, object target)
        {
            if (m_ExtractMembers == null)
            {
                ReflectMemebers();
            }

            foreach (var mInfo in m_ExtractMembers)
            {
                var value = mInfo.GetValue(target);
                if (!mInfo.isOptional)
                {
                    context.AddOrUpdateValue((K)mInfo.key, value);
                }
                else if (value != null)
                {
                    context.AddOrUpdateValue((K)mInfo.key, value);
                }
            }
        }

        private void ReflectFields()
        {
            FieldInfo[] fieldInfos = targetType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (fieldInfos != null && fieldInfos.Length > 0)
            {
                foreach (var fieldInfo in fieldInfos)
                {
                    InjectUsageAttribute attr = fieldInfo.GetCustomAttribute<InjectUsageAttribute>();
                    if (attr != null && attr.Key != null)
                    {
                        InjectMemberInfo imInfo = new InjectMemberInfo(fieldInfo, attr);
                        if (attr.OperationType == EInjectOperationType.Inject)
                        {
                            m_InjectMembers.Add(imInfo);
                        }
                        else
                        if (attr.OperationType == EInjectOperationType.Extract)
                        {
                            m_ExtractMembers.Add(imInfo);
                        }
                        else
                        {
                            m_InjectMembers.Add(imInfo);
                            m_ExtractMembers.Add(imInfo);
                        }
                    }
                }
            }
        }

        private void ReflectProperties()
        {
            PropertyInfo[] propertyInfos = targetType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (propertyInfos != null && propertyInfos.Length > 0)
            {
                foreach (var propertyInfo in propertyInfos)
                {
                    InjectUsageAttribute attr = propertyInfo.GetCustomAttribute<InjectUsageAttribute>();
                    if (attr != null && attr.Key != null)
                    {
                        InjectMemberInfo imInfo = new InjectMemberInfo(propertyInfo, attr);
                        if (attr.OperationType == EInjectOperationType.Inject)
                        {
                            m_InjectMembers.Add(imInfo);
                        }
                        else
                        if (attr.OperationType == EInjectOperationType.Extract)
                        {
                            m_ExtractMembers.Add(imInfo);
                        }
                        else
                        {
                            m_InjectMembers.Add(imInfo);
                            m_ExtractMembers.Add(imInfo);
                        }
                    }
                }
            }
        }
    }
}
