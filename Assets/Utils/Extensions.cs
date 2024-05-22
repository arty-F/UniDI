using UnityEngine;

namespace Assets.Core
{
    public static class Extensions
    {
        public static void Resolve<T>(this T consumer) where T : class
        {
            DIUnityContext.Instance.Resolve(consumer);
        }

        public static void Inject<T>(this T injected)
        {
            DIUnityContext.Instance.Inject(injected);
        }

        public static Object InstantiateResolve(this Object original, Vector3 position, Quaternion rotation)
        {
            var result = Object.Instantiate(original, position, rotation);
            result.Resolve();
            return result;
        }

        public static Object InstantiateResolve(this Object original, Vector3 position, Quaternion rotation, Transform parent)
        {
            var result = Object.Instantiate(original, position, rotation, parent);
            result.Resolve();
            return result;
        }

        public static Object InstantiateResolve(this Object original)
        {
            var result = Object.Instantiate(original);
            result.Resolve();
            return result;
        }

        public static Object InstantiateResolve(this Object original, Transform parent)
        {
            var result = Object.Instantiate(original, parent);
            result.Resolve();
            return result;
        }

        public static Object InstantiateResolve(this Object original, Transform parent, bool instantiateInWorldSpace)
        {
            var result = Object.Instantiate(original, parent, instantiateInWorldSpace);
            result.Resolve();
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
    }
}
