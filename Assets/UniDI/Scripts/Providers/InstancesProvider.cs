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
        private readonly List<int> _sceneIds = new();

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

            if (lifetime == Lifetime.Scene)
            {
                _sceneIds.Add(id);
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
            var idInstancesMap = GetIdInstancesMap(id)[id];
            if (idInstancesMap.ContainsKey(type))
            {
                return (I)idInstancesMap[type];
            }
            throw new UniDIException($"Type of {type.Name} has not yet been injected by {id} id.");
        }

        internal void ClearSceneInstances()
        {
            _sceneInstancesMap.Clear();
            _sceneIdInstancesMap.Clear();
            _sceneIds.Clear();
        }

        internal void ClearInstanceById(int id)
        {
            var idInstancesMap = GetIdInstancesMap(id)[id];
            idInstancesMap.Clear();
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

        private Dictionary<int, Dictionary<Type, object>> GetIdInstancesMap(int id)
        {
            return _sceneIds.Contains(id) ? _sceneIdInstancesMap : _gameIdInstancesMap;
        }
    }
}
