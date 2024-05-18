using Assets.Core;
using UnityEngine;

namespace Assets
{


    public class UniDI : MonoBehaviour
    {
        private void Start()
        {
            /*var context = new DelegatesInjector();


            var injected = new InjectedClass();
            var test = new TestClass();

            context.Map(injected);
            //context.Map(5);

            test.Resolve();
            //context.Resolve(test, injected);

            //context.Resolve<TestClass, InjectedClass>(test);

            //var a = context.GetType().GetMethod("Resolve");
            //var g = a.MakeGenericMethod(new System.Type[] { test.GetType(), injected.GetType() });
            //g.Invoke(context, new object[] { test });


            Debug.Log(test.GetField());
            Debug.Log(test.GetProperty());
            Debug.Log(test.GetMethod());*/

            var injected = new InjectedClass();
            var test = new TestClass();

            UniDiContext.Instance.Inject(injected);
            test.Resolve();
            Debug.Log(test.GetField());
        }
    }
}
