namespace Assets.Core
{
    public static class Extensions
    {
        public static void Resolve<T>(this T consumer) where T : class
        {
            UniDiContext.Instance.Resolve(consumer);
        }
    }
}
