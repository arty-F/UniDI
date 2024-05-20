using System;
using System.Collections.Generic;
using System.Reflection;

namespace Assets.Core
{
    internal class MemberInfoProvider
    {
        private const int TEMP_LIST_SIZE = 10;

        private readonly BindingFlags _flags;
        private readonly Dictionary<Type, FieldInfo[]> _fieldInfoMap = new();
        private readonly Dictionary<Type, PropertyInfo[]> _propertyInfoMap = new();
        private readonly List<FieldInfo> _tempFieldsInfo;
        private readonly List<PropertyInfo> _tempPropertiesInfo;

        public MemberInfoProvider(BindingFlags flags, int tempListSize = TEMP_LIST_SIZE)
        {
            _flags = flags;
            _tempFieldsInfo = new(tempListSize);
            _tempPropertiesInfo = new(tempListSize);
        }

        internal FieldInfo[] GetFieldInfos(Type consumerType)
        {
            FieldInfo[] injectedFields;
            if (!_fieldInfoMap.TryGetValue(consumerType, out injectedFields))
            {
                injectedFields = GetInjectedFields(consumerType);
                _fieldInfoMap.Add(consumerType, injectedFields);
            }
            return injectedFields;
        }

        internal PropertyInfo[] GetPropertyInfos(Type consumerType)
        {
            PropertyInfo[] injectedProperties;
            if (!_propertyInfoMap.TryGetValue(consumerType, out injectedProperties))
            {
                injectedProperties = GetInjectedProperties(consumerType);
                _propertyInfoMap.Add(consumerType, injectedProperties);
            }
            return injectedProperties;
        }

        private FieldInfo[] GetInjectedFields(Type consumerType)
        {
            var fields = consumerType.GetFields(_flags);
            foreach (var fieldInfo in fields)
            {
                if (fieldInfo.GetCustomAttribute<Inject>() != null)
                {
                    _tempFieldsInfo.Add(fieldInfo);
                }
            }

            var result = _tempFieldsInfo.ToArray();
            _tempFieldsInfo.Clear();
            return result;
        }
        
        private PropertyInfo[] GetInjectedProperties(Type consumerType)
        {
            var properties = consumerType.GetProperties(_flags);
            foreach (var property in properties)
            {
                if (property.GetCustomAttribute<Inject>() != null)
                {
                    _tempPropertiesInfo.Add(property);
                }
            }

            var result = _tempPropertiesInfo.ToArray();
            _tempFieldsInfo.Clear();
            return result;
        }
    }
}
