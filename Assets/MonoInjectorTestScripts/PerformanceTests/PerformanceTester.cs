using MonoInjector;
using MonoInjector.Test;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Assets.MonoInjectorTestScripts
{
    public class PerformanceTester : MonoBehaviour
    {
        public void Test(
            List<FieldsConsumer> fieldsConsumers, 
            List<PropertiesConsumer> properiesConsumers, 
            List<MethodConsumer> methodConsumers,
            List<AllConsumer> allConsumers,
            Stopwatch timer)
        {
            timer = new Stopwatch();
            timer.Start();

            foreach (var item in fieldsConsumers)
            {
                item.Resolve();
            }

            foreach (var item in properiesConsumers)
            {
                item.Resolve();
            }

            foreach (var item in methodConsumers)
            {
                item.Resolve();
            }

            foreach (var item in allConsumers)
            {
                item.Resolve();
            }

            timer.Stop();
            UnityEngine.Debug.Log($"Resolved in {timer.ElapsedMilliseconds} ms.");
        }
    }
}
