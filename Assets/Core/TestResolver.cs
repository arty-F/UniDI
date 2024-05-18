namespace Assets
{
    public class TestResolver
    {
        public void Resolve(TestClass consumer, InjectedClass injected)
        {
            consumer._field = injected;
            consumer.Property = injected;
            consumer.Method(injected);
        }
    }
}
