namespace Assets
{
    public class TestClass
    {
        [Inject]
        private InjectedClass1 _field1;

        [Inject]
        private InjectedClass2 _field2;

        [Inject]
        private InjectedClass3 _field3;

        [Inject]
        public InjectedClass1 Property1 { get; private set; }

        [Inject]
        public InjectedClass2 Property2 { get; private set; }

        [Inject]
        public InjectedClass3 Property3 { get; private set; }

        private InjectedClass1 _injectedFromMethod1;
        private InjectedClass2 _injectedFromMethod2;
        private InjectedClass3 _injectedFromMethod3;

        [Inject]
        public void Method(InjectedClass1 injected1, InjectedClass2 injected2, InjectedClass3 injected3)
        {
            _injectedFromMethod1 = injected1;
            _injectedFromMethod2 = injected2;
            _injectedFromMethod3 = injected3;
        }

        public int GetSumField() => _field1.Value + _field2.Value + _field3.Value;
        public int GetSumProp() => Property1.Value + Property2.Value + Property3.Value;
        public int GetSumMethod() => _injectedFromMethod1.Value + _injectedFromMethod2.Value + _injectedFromMethod3.Value;
    }
}
