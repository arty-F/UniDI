using UniDI;
using UniDI.Test;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Assets.UniDITestScripts
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
            timer.Start();

            foreach (var item in fieldsConsumers)
            {
                item.Resolve(1);
            }

            foreach (var item in properiesConsumers)
            {
                item.Resolve(1);
            }

            foreach (var item in methodConsumers)
            {
                item.Resolve(1);
            }

            foreach (var item in allConsumers)
            {
                item.Resolve(1);
            }

            timer.Stop();
            UnityEngine.Debug.Log($"Resolved in {timer.ElapsedMilliseconds} ms.");
        }
    }
}
