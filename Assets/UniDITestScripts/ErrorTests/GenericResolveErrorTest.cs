using UniDI;
using UniDI.Test;
using UnityEngine;

namespace Assets.UniDITestScripts.ErrorTests
{
    public class GenericResolveErrorTest : MonoBehaviour
    {
        private void Start()
        {
            var errorConsumer = new ErrorConsumer();
            errorConsumer.Resolve();
        }
    }
}
