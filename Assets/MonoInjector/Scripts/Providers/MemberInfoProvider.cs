using System;
using System.Collections.Generic;
using System.Reflection;

namespace MonoInjector.Providers
{
    internal class MemberInfoProvider
    {
        private const int TEMP_LIST_SIZE = 10;

        private readonly BindingFlags _flags;
        private readonly Dictionary<Type, FieldInfo[]> _fieldInfoMap = new();
        private readonly Dictionary<Type, PropertyInfo[]> _propertyInfoMap = new();
        private readonly Dictionary<Type, MethodInfo[]> _methodInfoMap = new();
        private readonly List<FieldInfo> _tempFieldsInfo;
        private readonly List<PropertyInfo> _tempPropertiesInfo;
        private readonly List<MethodInfo> _tempMethodsInfo;

        public MemberInfoProvider(BindingFlags flags, int tempListSize = TEMP_LIST_SIZE)
        {
            _flags = flags;
            _tempFieldsInfo = new(tempListSize);
            _tempPropertiesInfo = new(tempListSize);
            _tempMethodsInfo= new(tempListSize);
        }

        internal FieldInfo[] GetFieldInfos(Type consumerType)
        {
            if (!_fieldInfoMap.TryGetValue(consumerType, out FieldInfo[] injectedFields))
            {
                injectedFields = GetInjectedFields(consumerType);
                _fieldInfoMap.Add(consumerType, injectedFields);
            }
            return injectedFields;
        }

        internal PropertyInfo[] GetPropertyInfos(Type consumerType)
        {
            if (!_propertyInfoMap.TryGetValue(consumerType, out PropertyInfo[] injectedProperties))
            {
                injectedProperties = GetInjectedProperties(consumerType);
                _propertyInfoMap.Add(consumerType, injectedProperties);
            }
            return injectedProperties;
        }

        internal MethodInfo[] GetMethodInfos(Type consumerType)
        {
            if (!_methodInfoMap.TryGetValue(consumerType, out MethodInfo[] injectedMethods))
            {
                injectedMethods = GetInjectedMethods(consumerType);
                _methodInfoMap.Add(consumerType, injectedMethods);
            }
            return injectedMethods;
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
            _tempPropertiesInfo.Clear();
            return result;
        }

        private MethodInfo[] GetInjectedMethods(Type consumerType)
        {
            var methods = consumerType.GetMethods(_flags);
            foreach (var method in methods)
            {
                if (method.GetCustomAttribute<Inject>() != null)
                {
                    _tempMethodsInfo.Add(method);
                }
            }

            var result = _tempMethodsInfo.ToArray();
            _tempMethodsInfo.Clear();
            return result;
        }
    }
}
