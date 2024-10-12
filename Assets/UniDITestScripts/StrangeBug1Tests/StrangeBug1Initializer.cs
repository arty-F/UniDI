using System.Threading;
using UniDI;
using UnityEngine;

namespace Assets.UniDITestScripts.StrangeBug1Tests
{
    public class StrangeBug1Initializer : MonoBehaviour
    {
        [SerializeField]
        private StrangeBug1Consumer _consumer;

        private void Start()
        {
            var cts = new CancellationTokenSource();
            cts.Inject();

            _consumer.Resolve();
        }
    }
}
