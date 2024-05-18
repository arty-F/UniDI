using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    internal class Class1
    {
        private void Start()
        {





            /*var context = new Context();

            
            var injected = new InjectedClass();
            var test = new TestClass();

            context.Bind(injected);

            var fields = test.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (var fieldInfo in fields)
            {
                if (fieldInfo.GetCustomAttribute<Inject>() != null)
                {
                    var setter = CreateSetter<TestClass, InjectedClass>(fieldInfo);
                    setter(test, injected);
                }
            }

            var properties = test.GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (var propertyInfo in properties)
            {
                if (propertyInfo.GetCustomAttribute<Inject>() != null)
                {
                    var setter = CreateSetter<TestClass, InjectedClass>(propertyInfo);
                    setter(test, injected);
                }
            }

            Debug.Log(test.GetProperty());*/
        }


    }
}
