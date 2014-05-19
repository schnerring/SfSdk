using System;

namespace SfSdk.Response
{
    /// <summary>
    ///     A base class of type <see cref="IResponse"/> containing the arguments of the response.
    /// </summary>
    internal abstract class ResponseBase : IResponse
    {
        private readonly string[] _args;

        /// <summary>
        ///     Creates a new base response.
        /// </summary>
        /// <param name="args">The response arguments.</param>
        /// <exception cref="ArgumentNullException">When arguments are null.</exception>
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