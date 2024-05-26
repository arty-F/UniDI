using UnityEngine;

namespace MonoInjector
{
    public static class Extensions
    {
        public static void Resolve<T>(this T consumer) where T : class
        {
            MonoInjectorContext.Instance.Resolve(consumer);
        }

        public static void Inject<T>(this T injected)
        {
            MonoInjectorContext.Instance.Inject(injected);
        }

        public static void Inject<C>(this GameObject injected)
        {
            if (!injected.TryGetComponent<C>(out var component))
            {
                throw new MonoInjectorException($"GameObject {injected.name} does not contain component of type {typeof(C).Name} to injection.");
            }
            component.Inject();
        }

        public static GameObject InstantiateResolve<C>(this GameObject original, Vector3 position, Quaternion rotation) where C : class
        {
            var result = Object.Instantiate(original, position, rotation);
            ResolveComponent<C>(result);
            return result;
        }

        public static GameObject InstantiateResolve<C>(this GameObject original, Vector3 position, Quaternion rotation, Transform parent) where C : class
        {
            var result = Object.Instantiate(original, position, rotation, parent);
            ResolveComponent<C>(result);
            return result;
        }

        public static GameObject InstantiateResolve<C>(this GameObject original) where C : class
        {
            var result = Object.Instantiate(original);
            ResolveComponent<C>(result);
            return result;
        }

        public static GameObject InstantiateResolve<C>(this GameObject original, Transform parent) where C : class
        {
            var result = Object.Instantiate(original, parent);
            ResolveComponent<C>(result);
            return result;
        }

        public static GameObject InstantiateResolve<C>(this GameObject original, Transform parent, bool instantiateInWorldSpace) where C : class
        {
            var result = Object.Instantiate(original, parent, instantiateInWorldSpace);
            ResolveComponent<C>(result);
            return result;
        }

        public static T InstantiateResolve<T>(this T original) where T : Object
        {
            var result = Object.Instantiate(original);
            result.Resolve();
            return result;
        }

        public static T InstantiateResolve<T>(this T original, Vector3 position, Quaternion rotation) where T : Object
        {
            var result = Object.Instantiate(original, position, rotation);
            result.Resolve();
            return result;
        }

        public static T InstantiateResolve<T>(this T original, Vector3 position, Quaternion rotation, Transform parent) where T : Object
        {
            var result = Object.Instantiate(original, position, rotation, parent);
            result.Resolve();
            return result;
        }

        public static T InstantiateResolve<T>(this T original, Transform parent) where T : Object
        {
            var result = Object.Instantiate(original, parent);
            result.Resolve();
            return result;
        }

        public static T InstantiateResolve<T>(this T original, Transform parent, bool worldPositionStays) where T : Object
        {
            var result = Object.Instantiate(original, parent, worldPositionStays);
            result.Resolve();
            return result;
        }

        private static void ResolveComponent<C>(GameObject obj) where C : class
        {
            if (!obj.TryGetComponent<C>(out var component))
            {
                throw new MonoInjectorException($"GameObject {obj.name} does not contain component of type {typeof(C).Name} to resolving.");
            }
            component.Resolve();
        }
    }
}
