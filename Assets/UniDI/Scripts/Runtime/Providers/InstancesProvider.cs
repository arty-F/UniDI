using System;
using System.Collections.Generic;
using UniDI.Settings;
using UnityEngine;

namespace UniDI.Providers
{
    internal class InstancesProvider
    {
        private readonly UniDISettings _settings;
        private readonly Dictionary<Type, object> _gameInstancesMap;
        private readonly Dictionary<Type, object> _sceneInstancesMap;
        private readonly Dictionary<int, Dictionary<Type, object>> _gameIdInstancesMap;
        private readonly Dictionary<int, Dictionary<Type, object>> _sceneIdInstancesMap;

        public InstancesProvider(UniDISettings settings)
        {
            _settings = settings;
            _gameInstancesMap = new(settings.GameInstances);
            _sceneInstancesMap = new(settings.SceneInstances);
            _gameIdInstancesMap = new(settings.LocalScopes);
            _sceneIdInstancesMap = new(settings.LocalScopes);
        }

        internal bool Store<I>(I injected, Lifetime lifetime)
        {
            if (lifetime == Lifetime.Game)
            {
                return StoreInstance(injected, _gameInstancesMap, typeof(I));
            }
            else
            {
                return StoreInstance(injected, _sceneInstancesMap, typeof(I));
            }
        }

        internal bool Store<I>(I injected, int id, Lifetime lifetime)
        {
            if (lifetime == Lifetime.Game)
            {
                return StoreInstanceById(injected, id, _gameIdInstancesMap, _settings.GameLocalInstances);
            }
            else
            {
                return StoreInstanceById(injected, id, _sceneIdInstancesMap, _settings.SceneLocalInstances);
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
            throw new UniDIException($"Type of {type.FullName} has not yet been injected.");
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

        private bool StoreInstanceById<I>(I injected, int id, Dictionary<int, Dictionary<Type, object>> idInstancesMap, int capacity)
        {
            if (!idInstancesMap.ContainsKey(id))
            {
                idInstancesMap.Add(id, new Dictionary<Type, object>(capacity));
            }
            return StoreInstance(injected, idInstancesMap[id], typeof(I));
        }

        private bool StoreInstance<I>(I injected, Dictionary<Type, object> instancesMap, Type type)
        {
            if (instancesMap.ContainsKey(type))
            {
#if UNITY_EDITOR
                if (_settings.ShowReinjectLog)
                {
                    Debug.Log($"Type {type.Name} are reinjected.");
                }
#endif
                instancesMap[type] = injected;
                return true;
            }
            else
            {
                instancesMap.Add(typeof(I), injected);
                return false;
            }
        }
    }
}
