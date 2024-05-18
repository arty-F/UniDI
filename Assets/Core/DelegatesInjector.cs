using Assets.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Assets
{
    public class DelegatesInjector
    {
        private const BindingFlags FLAGS = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        private Dictionary<Type, object> _bindedTypes = new();

        private GenericMethodsProvider _genericMethodsProvider;

        public DelegatesInjector()
        {
            //_genericMethodsProvider = new GenericMethodsProvider(this, nameof(InjectDependencies), FLAGS);
        }

        public void Map<I>(I injected)
        {
            _bindedTypes.Add(typeof(I), injected);
        }

        public void Inject(object consumer, object injected)
        {
            //var resolveDependencies = _genericMethodsProvider.Get(consumer.GetType(), injected.GetType());
            //resolveDependencies.Invoke(this, new object[] { consumer });
        }

        private void InjectDependencies<C, I>(C consumer)
        {
            var fields = consumer.GetType().GetFields(FLAGS);
            foreach (var fieldInfo in fields)
            {
                if (fieldInfo.GetCustomAttribute<Inject>() != null)
                {
                    var setter = CreateSetter<C, I>(fieldInfo);
                    setter(consumer, (I)_bindedTypes[typeof(I)]);
                }
            }

            var properties = consumer.GetType().GetProperties(FLAGS);
            foreach (var propertyInfo in properties)
            {
                if (propertyInfo.GetCustomAttribute<Inject>() != null)
                {
                    var a = propertyInfo.GetSetMethod(true);
                    var setter = CreateSetter<C, I>(propertyInfo);
                    setter(consumer, (I)_bindedTypes[typeof(I)]);
                }
            }

            var methods = consumer.GetType().GetMethods(FLAGS);
            foreach (var methodInfo in methods)
            {
                if (methodInfo.GetCustomAttribute<Inject>() == null)
                {
                    continue;
                }

                var parameters = methodInfo.GetParameters().Select(p => p.ParameterType).ToArray();

                var resolvedParameters = new object[parameters.Length];
                for (int i = 0; i < resolvedParameters.Length; i++)
                {
                    resolvedParameters[i] = _bindedTypes[parameters[i]];
                }
                
                var setter = CreateSetter<C, I>(methodInfo);
                setter(consumer, (I)_bindedTypes[typeof(I)]);
                //setter(consumer, (I)_bindedTypes[typeof(I)], 5);
            }
        }

        private Action<C, I> CreateSetter<C, I>(MethodInfo method)
        {
            return (Action<C, I>)Delegate.CreateDelegate(typeof(Action<C, I>), null, method);
        }

        private Action<C, I> CreateSetter<C, I>(FieldInfo field)
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
            return (Action<C, I>)setterMethod.CreateDelegate(typeof(Action<C, I>));
        }

        private Action<C, I> CreateSetter<C, I>(PropertyInfo property) =>
            (Action<C, I>)Delegate.CreateDelegate(typeof(Action<C, I>), null, property.GetSetMethod(true));
    }
}
