using System.Threading;
using UniDI;
using UnityEngine;

namespace Assets.UniDITestScripts.StrangeBug1Tests
{
    public class StrangeBug1Consumer : MonoBehaviour
    {
        private CancellationTokenSource _cts;

        [Inject]
        private void Init(CancellationTokenSource cts)
        {
            _cts = cts;
        }
    }
}
