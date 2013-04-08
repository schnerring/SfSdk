using System.Collections.Generic;

namespace SfSdk
{
    internal class SfRequestResult
    {
        internal SfRequestResult()
        {
            Errors = new List<string>();
        }

        internal List<string> Errors { get; private set; }
        internal object Result { get; set; }
    }
}