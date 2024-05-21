using Assets.Core;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace Assets
{
    public class Usage : MonoBehaviour
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

            var _fieldsConsumers = _fieldsConsumer
                ? Enumerable.Repeat(new FieldsConsumer(), _instancesCount).ToList()
                : new List<FieldsConsumer>();
            var _properiesConsumers = _propertiesConsumer
                ? Enumerable.Repeat(new PropertiesConsumer(), _instancesCount).ToList()
                : new List<PropertiesConsumer>();
            var _methodConsumers = _methodConsumer
                ? Enumerable.Repeat(new MethodConsumer(), _instancesCount).ToList()
                : new List<MethodConsumer>();
            var _allConsumers = _allConsumer
                ? Enumerable.Repeat(new AllConsumer(), _instancesCount).ToList()
                : new List<AllConsumer>();

            Stopwatch timer = new Stopwatch();
            int accumulator = 0;
            timer.Start();

            foreach (var item in _fieldsConsumers)
            {
                item.Resolve();
                accumulator += item.GetSumField();
            }

            foreach (var item in _properiesConsumers)
            {
                item.Resolve();
                accumulator += item.GetSumProp();
            }

            foreach (var item in _methodConsumers)
            {
                item.Resolve();
                accumulator += item.GetSumMethod();
            }

            foreach (var item in _allConsumers)
            {
                item.Resolve();
                accumulator += item.GetSumAll();
            }

            timer.Stop();
            UnityEngine.Debug.Log($"{accumulator} accumulated in: {timer.ElapsedMilliseconds} ms." );
        }
    }
}
