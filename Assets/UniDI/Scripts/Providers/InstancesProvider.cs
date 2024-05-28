using System;
using System.Collections.Generic;
using UnityEngine;

namespace UniDI.Providers
{
    internal class InstancesProvider
    {
        private readonly Dictionary<Type, object> _gameInstancesMap = new();
        private readonly Dictionary<Type, object> _sceneInstancesMap = new();

        internal void Store<I>(I injected, Lifetime lifetime)
        {
            var type = typeof(I);
            if (lifetime == Lifetime.Game)
            {
                StoreInstance(injected, _gameInstancesMap, type);
            }
            else
            {
                StoreInstance(injected, _sceneInstancesMap, type);
            }
        }

        internal I GetInstance<I>(Type type)
        {
            if (_gameInstancesMap.ContainsKey(type))
            {
                return (I)_gameInstancesMap[type];
            }

            if (_sceneInstancesMap.ContainsKey(type))
            {
                return (I)_sceneInstancesMap[type];
            }

            throw new UniDIException($"Type of {type.Name} has not yet been injected.");
        }

        internal void ClearSceneInstances()
        {
            _sceneInstancesMap.Clear();
        }

        private void StoreInstance<I>(I injected, Dictionary<Type, object> instancesMap, Type type)
        {
            if (instancesMap.ContainsKey(type))
            {
#if UNITY_EDITOR
                Debug.Log($"Type {type.Name} are reinjected.");
#endif
                instancesMap[type] = injected;
            }
            else
            {
                instancesMap.Add(typeof(I), injected);
            }
        }
    }
}
