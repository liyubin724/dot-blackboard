using System;
using System.Reflection;

namespace DotEngine.BB.Injector
{
    internal class InjectMemberInfo
    {
        private FieldInfo m_FieldInfo;
        private PropertyInfo m_PropertyInfo;
        private InjectUsageAttribute m_UsageAttr;

        public string valueName { get; private set; }
        public Type valueType { get; private set; }

        public object key
        {
            get
            {
                return m_UsageAttr.Key;
            }
        }

        public bool isOptional
        {
            get
            {
                return m_UsageAttr.IsOptional;
            }
        }

        public InjectMemberInfo(FieldInfo fInfo, InjectUsageAttribute attr)
        {
            m_FieldInfo = fInfo;
            valueName = m_FieldInfo.Name;
            valueType = m_FieldInfo.FieldType;
            m_UsageAttr = attr;
        }

        public InjectMemberInfo(PropertyInfo pInfo, InjectUsageAttribute attr)
        {
            m_PropertyInfo = pInfo;
            valueName = m_PropertyInfo.Name;
            valueType = m_PropertyInfo.PropertyType;
            m_UsageAttr = attr;
        }

        public object GetValue(object target)
        {
            if (m_FieldInfo != null)
            {
                return m_FieldInfo.GetValue(target);
            }
            else if (m_PropertyInfo != null && m_PropertyInfo.CanRead)
            {
                return m_PropertyInfo.GetValue(target);
            }
            else
            {
                throw new Exception();
            }
        }

        public void SetValue(object target, object value)
        {
            if (m_FieldInfo != null)
            {
                m_FieldInfo.SetValue(target, value);
            }
            else if (m_PropertyInfo != null && m_PropertyInfo.CanWrite)
            {
                m_PropertyInfo.SetValue(target, value);
            }
            else
            {
                throw new Exception();
            }

        }
    }
}
