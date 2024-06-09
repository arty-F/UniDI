using System;
using UniDI;
using UniDI.Test;
using UnityEngine;

namespace UniDi.Test
{
    public class ReleaseDependencyTester : MonoBehaviour
    {
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

            var test1 = new ReleaseInjectionConsumer();
            test1.Resolve();
            Debug.Log($"{test1.GetSum()} - all is ok.");

            var test2 = new ReleaseInjectionConsumer();
            injected1.ReleaseDependency();
            try
            {
                test2.Resolve();
                test2.GetSum();
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }

            var test3 = new ReleaseInjectionConsumer();
            injected1.Inject(1);
            test3.Resolve(1);
            Debug.Log($"{test3.GetSum()} - all is ok.");

            var test4 = new ReleaseInjectionConsumer();
            try
            {
                test4.Resolve();
                test4.GetSum();
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }

            var test5 = new ReleaseInjectionConsumer();
            injected1.ReleaseDependency(2);
            test5.Resolve(1);
            Debug.Log($"{test5.GetSum()} - all is ok.");

            var test6 = new ReleaseInjectionConsumer();
            injected1.ReleaseDependency(1);
            try
            {
                test6.Resolve(1);
                test6.GetSum();
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }

            injected1.ReleaseDependency(1);
            injected1.ReleaseDependency(1);
        }
    }
}
