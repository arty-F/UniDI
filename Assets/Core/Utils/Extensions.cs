using System.Runtime.CompilerServices;

namespace Assets.Core
{
    public static class Extensions
    {
        public static void Resolve<T>(this T consumer) where T : class
        {
            UniDIContext.Instance.Resolve(consumer);
        }

        public static void Inject<T>(this T injected)
        {
            UniDIContext.Instance.Inject(injected);
        }

        //TODO Instantiate
    }
}
