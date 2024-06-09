using System;
using System.Collections.Generic;
using UnityEngine;

namespace UniDI.Providers
{
    internal class InstancesProvider
    {
        private readonly Dictionary<Type, object> _gameInstancesMap = new();
        private readonly Dictionary<Type, object> _sceneInstancesMap = new();
        private readonly Dictionary<int, Dictionary<Type, object>> _gameIdInstancesMap = new();
        private readonly Dictionary<int, Dictionary<Type, object>> _sceneIdInstancesMap = new();

        internal void Store<I>(I injected, Lifetime lifetime)
        {
            if (lifetime == Lifetime.Game)
            {
                StoreInstance(injected, _gameInstancesMap, typeof(I));
            }
            else
            {
                StoreInstance(injected, _sceneInstancesMap, typeof(I));
            }
        }

        internal void Store<I>(I injected, int id, Lifetime lifetime)
        {
            if (lifetime == Lifetime.Game)
            {
                StoreInstanceById(injected, id, _gameIdInstancesMap);
            }
            else
            {
                StoreInstanceById(injected, id, _sceneIdInstancesMap);
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

        internal I GetInstance<I>(Type type, int id)
        {
            if (_gameIdInstancesMap.ContainsKey(id) && _gameIdInstancesMap[id].ContainsKey(type))
            {
                return (I)_gameIdInstancesMap[id][type];
            }
            if (_sceneIdInstancesMap.ContainsKey(id) && _sceneIdInstancesMap[id].ContainsKey(type))
            {
                return (I)_sceneIdInstancesMap[id][type];
            }
            return GetInstance<I>(type);
        }

        internal void ClearSceneInstances()
        {
            _sceneInstancesMap.Clear();
            _sceneIdInstancesMap.Clear();
        }

        internal void ClearInstances(Type type)
        {
            ClearInstance(_gameInstancesMap, type);
            ClearInstance(_sceneInstancesMap, type);
        }

        private void ClearInstance(Dictionary<Type, object> map, Type type)
        {
            if (map.ContainsKey(type))
            {
                map.Remove(type);
            }
        }

        internal void ClearInstancesById(Type type, int id, bool clearFullScope)
        {
            ClearInstanceById(_gameIdInstancesMap, type, id, clearFullScope);
            ClearInstanceById(_sceneIdInstancesMap, type, id, clearFullScope);
        }

        private void ClearInstanceById(Dictionary<int, Dictionary<Type, object>> map, Type type, int id, bool clearFullScope)
        {
            if (map.ContainsKey(id))
            {
                if (clearFullScope)
                {
                    map.Remove(id);
                }
                else
                {
                    map[id].Remove(type);
                }
            }
        }

        private void StoreInstanceById<I>(I injected, int id, Dictionary<int, Dictionary<Type, object>> idInstancesMap)
        {
            if (!idInstancesMap.ContainsKey(id))
            {
                idInstancesMap.Add(id, new Dictionary<Type, object>());
            }
            StoreInstance(injected, idInstancesMap[id], typeof(I));
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
