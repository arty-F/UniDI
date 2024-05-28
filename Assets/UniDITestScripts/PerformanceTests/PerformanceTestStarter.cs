using Assets.UniDITestScripts;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace UniDI.Test
{
    public class PerformanceTestStarter : MonoBehaviour
    {
        [SerializeField]
        private int _instancesCount;
        [SerializeField]
        private bool _fieldsConsumer;
        [SerializeField]
        private bool _propertiesConsumer;
        [SerializeField]
        private bool _methodConsumer;
        [SerializeField]
        private bool _allConsumer;
        [SerializeField]
        private PerformanceTester _tester;

        private List<FieldsConsumer> _fieldsConsumers;
        private List<PropertiesConsumer> _properiesConsumers;
        private List<MethodConsumer> _methodConsumers;
        private List<AllConsumer> _allConsumers;
        private Stopwatch _stopwatch;

        private void Start()
        {
            var injected1 = new InjectedClass1();
            var injected2 = new InjectedClass2();
            var injected3 = new InjectedClass3();
            var injected4 = new InjectedClass4();
            injected1.Inject();
            injected2.Inject();
            injected3.Inject();
            injected4.Inject();

            _fieldsConsumers = _fieldsConsumer
                ? Enumerable.Repeat(new FieldsConsumer(), _instancesCount).ToList()
                : new List<FieldsConsumer>();
            _properiesConsumers = _propertiesConsumer
                ? Enumerable.Repeat(new PropertiesConsumer(), _instancesCount).ToList()
                : new List<PropertiesConsumer>();
            _methodConsumers = _methodConsumer
                ? Enumerable.Repeat(new MethodConsumer(), _instancesCount).ToList()
                : new List<MethodConsumer>();
            _allConsumers = _allConsumer
                ? Enumerable.Repeat(new AllConsumer(), _instancesCount).ToList()
                : new List<AllConsumer>();
            _stopwatch = new Stopwatch();

            StartCoroutine(WaitFrameAndDoTest());
        }

        private IEnumerator WaitFrameAndDoTest()
        {
            yield return null;
            _tester.Test(_fieldsConsumers, _properiesConsumers, _methodConsumers, _allConsumers, _stopwatch);
        }
    }
}
