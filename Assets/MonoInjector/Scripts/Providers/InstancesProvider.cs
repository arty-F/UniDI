using System;
using System.Collections.Generic;
using UnityEngine;

namespace MonoInjector.Providers
{
    internal class InstancesProvider
    {
        private readonly Dictionary<Type, object> _storedInstancesMap = new();

        internal void Store<I>(I injected)
        {
            var type = typeof(I);
            if (_storedInstancesMap.ContainsKey(type))
            {
#if UNITY_EDITOR
                Debug.Log($"Type {type.Name} are reinjected.");
#endif
                _storedInstancesMap[type] = injected;
            }
            else
            {
                _storedInstancesMap.Add(typeof(I), injected);
            }
        }

        internal I GetInstance<I>(Type type)
        {
            if (!_storedInstancesMap.ContainsKey(type))
            {
                throw new UniDIException($"Type of {type.Name} has not yet been injected.");
            }
            return (I)_storedInstancesMap[type];
        }
    }
}
