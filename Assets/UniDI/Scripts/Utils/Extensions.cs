using UnityEngine;

namespace UniDI
{
    public static class Extensions
    {
        public static void Inject<T>(this T injected, Lifetime lifetime = Lifetime.Game)
        {
            UniDIContext.Instance.Inject(injected, lifetime);
        }

        public static void Inject<T>(this T injected, int id, Lifetime lifetime = Lifetime.Game)
        {
            UniDIContext.Instance.Inject(injected, id, lifetime);
        }

        public static void Inject<T>(this GameObject injected, Lifetime lifetime = Lifetime.Game)
        {
            if (!injected.TryGetComponent<T>(out var component))
            {
                throw new UniDIException($"GameObject {injected.name} does not contain component of type {typeof(T).Name} to injection.");
            }
            component.Inject(lifetime);
        }

        public static void Inject<T>(this GameObject injected, int id, Lifetime lifetime = Lifetime.Game)
        {
            if (!injected.TryGetComponent<T>(out var component))
            {
                throw new UniDIException($"GameObject {injected.name} does not contain component of type {typeof(T).Name} to injection.");
            }
            component.Inject(id, lifetime);
        }

        public static void ReleaseDependency<T>(this T _)
        {
            UniDIContext.Instance.ReleaseDependency(typeof(T));
        }

        public static void ReleaseDependency<T>(this T _, int id, bool clearFullScope = true)
        {
            UniDIContext.Instance.ReleaseDependency(typeof(T), id, clearFullScope);
        }

        public static void Resolve<T>(this T consumer) where T : class
        {
            UniDIContext.Instance.Resolve(consumer);
        }

        public static void Resolve<T>(this T consumer, int id) where T : class
        {
            UniDIContext.Instance.Resolve(consumer, id);
        }

        public static GameObject InstantiateResolve<T>(this GameObject original, Vector3 position, Quaternion rotation) where T : class
        {
            var result = Object.Instantiate(original, position, rotation);
            ResolveComponent<T>(result);
            return result;
        }

        public static GameObject InstantiateResolve<T>(this GameObject original, Vector3 position, Quaternion rotation, int id) where T : class
        {
            var result = Object.Instantiate(original, position, rotation);
            ResolveComponent<T>(result, id);
            return result;
        }

        public static GameObject InstantiateResolve<T>(this GameObject original, Vector3 position, Quaternion rotation, Transform parent) where T : class
        {
            var result = Object.Instantiate(original, position, rotation, parent);
            ResolveComponent<T>(result);
            return result;
        }

        public static GameObject InstantiateResolve<T>(this GameObject original, Vector3 position, Quaternion rotation, Transform parent, int id) where T : class
        {
            var result = Object.Instantiate(original, position, rotation, parent);
            ResolveComponent<T>(result, id);
            return result;
        }

        public static GameObject InstantiateResolve<T>(this GameObject original) where T : class
        {
            var result = Object.Instantiate(original);
            ResolveComponent<T>(result);
            return result;
        }

        public static GameObject InstantiateResolve<T>(this GameObject original, int id) where T : class
        {
            var result = Object.Instantiate(original);
            ResolveComponent<T>(result, id);
            return result;
        }

        public static GameObject InstantiateResolve<T>(this GameObject original, Transform parent) where T : class
        {
            var result = Object.Instantiate(original, parent);
            ResolveComponent<T>(result);
            return result;
        }

        public static GameObject InstantiateResolve<T>(this GameObject original, Transform parent, int id) where T : class
        {
            var result = Object.Instantiate(original, parent);
            ResolveComponent<T>(result, id);
            return result;
        }

        public static GameObject InstantiateResolve<T>(this GameObject original, Transform parent, bool instantiateInWorldSpace) where T : class
        {
            var result = Object.Instantiate(original, parent, instantiateInWorldSpace);
            ResolveComponent<T>(result);
            return result;
        }

        public static GameObject InstantiateResolve<T>(this GameObject original, Transform parent, bool instantiateInWorldSpace, int id) where T : class
        {
            var result = Object.Instantiate(original, parent, instantiateInWorldSpace);
            ResolveComponent<T>(result, id);
            return result;
        }

        public static T InstantiateResolve<T>(this T original) where T : Object
        {
            var result = Object.Instantiate(original);
            result.Resolve();
            return result;
        }

        public static T InstantiateResolve<T>(this T original, int id) where T : Object
        {
            var result = Object.Instantiate(original);
            result.Resolve(id);
            return result;
        }

        public static T InstantiateResolve<T>(this T original, Vector3 position, Quaternion rotation) where T : Object
        {
            var result = Object.Instantiate(original, position, rotation);
            result.Resolve();
            return result;
        }

        public static T InstantiateResolve<T>(this T original, Vector3 position, Quaternion rotation, int id) where T : Object
        {
            var result = Object.Instantiate(original, position, rotation);
            result.Resolve(id);
            return result;
        }

        public static T InstantiateResolve<T>(this T original, Vector3 position, Quaternion rotation, Transform parent) where T : Object
        {
            var result = Object.Instantiate(original, position, rotation, parent);
            result.Resolve();
            return result;
        }

        public static T InstantiateResolve<T>(this T original, Vector3 position, Quaternion rotation, Transform parent, int id) where T : Object
        {
            var result = Object.Instantiate(original, position, rotation, parent);
            result.Resolve(id);
            return result;
        }

        public static T InstantiateResolve<T>(this T original, Transform parent) where T : Object
        {
            var result = Object.Instantiate(original, parent);
            result.Resolve();
            return result;
        }

        public static T InstantiateResolve<T>(this T original, Transform parent, int id) where T : Object
        {
            var result = Object.Instantiate(original, parent);
            result.Resolve(id);
            return result;
        }

        public static T InstantiateResolve<T>(this T original, Transform parent, bool worldPositionStays) where T : Object
        {
            var result = Object.Instantiate(original, parent, worldPositionStays);
            result.Resolve();
            return result;
        }

        public static T InstantiateResolve<T>(this T original, Transform parent, bool worldPositionStays, int id) where T : Object
        {
            var result = Object.Instantiate(original, parent, worldPositionStays);
            result.Resolve(id);
            return result;
        }

        private static void ResolveComponent<T>(GameObject obj) where T : class
        {
            if (!obj.TryGetComponent<T>(out var component))
            {
                throw new UniDIException($"GameObject {obj.name} does not contain component of type {typeof(T).Name} to resolving.");
            }
            component.Resolve();
        }

        private static void ResolveComponent<T>(GameObject obj, int id) where T : class
        {
            if (!obj.TryGetComponent<T>(out var component))
            {
                throw new UniDIException($"GameObject {obj.name} does not contain component of type {typeof(T).Name} to resolving.");
            }
            component.Resolve(id);
        }
    }
}
