namespace UniDI.Test
{
    public class ReleaseInjectionConsumer
    {
        [Inject]
        private InjectedClass1 _field1;
        [Inject]
        public InjectedClass2 Property2 { get; private set; }

        private InjectedClass3 _field3;
        private InjectedClass4 _field4;

        [Inject]
        private void Init(InjectedClass3 p3, InjectedClass4 p4)
        {
            _field3 = p3;
            _field4 = p4;
        }

        public int GetSum() => _field1.Value + Property2.Value + _field3.Value + _field4.Value;
    }
}
