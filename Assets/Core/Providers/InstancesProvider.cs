using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Core
{
    internal class InstancesProvider
    {
        private readonly Dictionary<Type, object> _storedInstancesMap = new();

        internal void Store<I>(I injected)
        {
            var type = typeof(I);
            if (_storedInstancesMap.ContainsKey(type))
            {
                Debug.Log($"Type {type.Name} are reinjected.");
                _storedInstancesMap[type] = injected;
            }
            else
            {
                _storedInstancesMap.Add(typeof(I), injected);
            }
        }

        internal I GetInstance<I>(Type type)
        {
            return (I)_storedInstancesMap[type];
        }
    }
}
