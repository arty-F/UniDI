using System;
using UniDI.Providers;
using UniDI.Settings;
using UniDI.Strategies;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UniDI
{
    internal class UniDIContext
    {
        private static UniDIContext _instance = null;

        internal static UniDIContext Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UniDIContext();
                }
                return _instance;
            }
        }

        private ResolvingStrategy _resolvingStrategy;
        private InstancesProvider _instancesProvider;

        private UniDIContext()
        {
            var settings = Resources.Load<UniDISettings>(nameof(UniDISettings));
            if (settings == null)
            {
                throw new UniDIException("Can't load settings. Please try to reimport package.");
            }
            _instancesProvider = new InstancesProvider(settings);
            _resolvingStrategy = new ResolvingStrategy(_instancesProvider, settings);
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }

        internal void Resolve(object consumer)
        {
            _resolvingStrategy.Resolve(consumer);
        }

        internal void Resolve(object consumer, int id)
        {
            _resolvingStrategy.Resolve(consumer, id);
        }

        internal void Inject<I>(I injected, Lifetime lifetime)
        {
            if (_instancesProvider.Store(injected, lifetime))
            {
                _resolvingStrategy.ClearCacheForParameterType(typeof(I));
            }
        }

        internal void Inject<I>(I injected, int id, Lifetime lifetime)
        {
            if (_instancesProvider.Store(injected, id, lifetime))
            {
                _resolvingStrategy.ClearCacheForParameterType(typeof(I), id);
            }
        }

        internal void ReleaseDependency(Type type)
        {
            _instancesProvider.ClearInstances(type);
            _resolvingStrategy.ClearGlobalCache();
        }

        internal void ReleaseDependency(Type type, int id, bool clearFullScope)
        {
            _instancesProvider.ClearInstancesById(type, id, clearFullScope);
            _resolvingStrategy.ClearLocalCache(id);
        }

        private void OnActiveSceneChanged(Scene current, Scene next)
        {
            _instancesProvider.ClearSceneInstances();
            _resolvingStrategy.ClearGlobalCache();
            _resolvingStrategy.ClearLocalCache();
        }
    }
}
