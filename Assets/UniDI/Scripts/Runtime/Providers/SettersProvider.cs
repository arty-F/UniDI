using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UniDI.Settings;

namespace UniDI.Providers
{
    internal class SettersProvider
    {
        private readonly Dictionary<FieldInfo, object> _fieldSetters = new();
        private readonly Dictionary<PropertyInfo, object> _propertySetters = new();

        public SettersProvider(UniDISettings settings)
        {
            _fieldSetters = new(settings.InjectedFields);
            _propertySetters = new(settings.InjectedProperties);
        }

        internal Action<C, I> GetFieldSetter<C, I>(FieldInfo field)
        {
            if (!_fieldSetters.TryGetValue(field, out object setter))
            {
                var methodName = field.ReflectedType.FullName + ".set_" + field.Name;
                var setterMethod = new DynamicMethod(methodName, null, new Type[2] { typeof(C), typeof(I) }, true);
                var gen = setterMethod.GetILGenerator();
                if (field.IsStatic)
                {
                    gen.Emit(OpCodes.Ldarg_1);
                    gen.Emit(OpCodes.Stsfld, field);
                }
                else
                {
                    gen.Emit(OpCodes.Ldarg_0);
                    gen.Emit(OpCodes.Ldarg_1);
                    gen.Emit(OpCodes.Stfld, field);
                }
                gen.Emit(OpCodes.Ret);
                setter = setterMethod.CreateDelegate(typeof(Action<C, I>));
                _fieldSetters.Add(field, setter);
            }
            return (Action<C, I>)setter;
        }

        internal Action<C, I> GetPropertySetter<C, I>(PropertyInfo property)
        {
            if (!_propertySetters.TryGetValue(property, out object setter))
            {
                setter = Delegate.CreateDelegate(typeof(Action<C, I>), null, property.GetSetMethod(true));
                _propertySetters.Add(property, setter);
            }
            return (Action<C, I>)setter;
        }
    }
}
