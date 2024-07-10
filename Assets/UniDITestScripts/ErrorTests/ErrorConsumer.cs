using System.Collections.Generic;

namespace UniDI.Test
{
    partial class ErrorConsumer
    {
        private Dictionary<int, string> _dict;

        [Inject]
        private void Init(Dictionary<int, string> dict)
        {
            _dict = dict;
        }
    }
}
