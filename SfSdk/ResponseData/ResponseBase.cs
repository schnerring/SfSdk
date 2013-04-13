using System;

namespace SfSdk.ResponseData
{
    internal abstract class ResponseBase : IResponse
    {
        private readonly string[] _args;

        protected ResponseBase(string[] args)
        {
            if (args == null) throw new ArgumentNullException("args");
            _args = args;
        }

        public string[] Args
        {
            get { return _args; }
        }
    }
}