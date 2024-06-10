namespace UniDI.Test
{
    public class LocalConsumerClass
    {
        public LocalInjectedClass LocalInjectedClass;

        [Inject]
        private void Init(LocalInjectedClass localInjectedClass)
        {
            LocalInjectedClass = localInjectedClass;
        }
    }
}
