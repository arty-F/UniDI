using Assets.Core;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace Assets
{


    public class Usage : MonoBehaviour
    {
        private void Start()
        {
            /*var injected1 = new InjectedClass1();
            var injected2 = new InjectedClass2();
            var test = new TestClass();
            injected1.Inject();
            injected2.Inject();
            test.Resolve();
            Debug.Log(test.GetField1());
            Debug.Log(test.GetField2());*/

            var injected1 = new InjectedClass1();
            var injected2 = new InjectedClass2();
            var injected3 = new InjectedClass3();
            injected1.Inject();
            injected2.Inject();
            injected3.Inject();

            int instancesCount = 1000;
            var _testClasses = Enumerable.Repeat(new TestClass(), instancesCount).ToList();

            Stopwatch timer = new Stopwatch();
            int accumulator = 0;
            timer.Start();
            foreach (var testClass in _testClasses)
            {
                testClass.Resolve();
                accumulator += testClass.GetSum();
            }
            timer.Stop();
            UnityEngine.Debug.Log($"{accumulator} accumulated in: {timer.ElapsedMilliseconds} мс." );
        }
    }
}
