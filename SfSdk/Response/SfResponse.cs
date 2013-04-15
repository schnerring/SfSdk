using System;
using System.Collections.Generic;
using SfSdk.Constants;

namespace SfSdk.Response
{
    /// <summary>
    ///     Provides easier processing of response strings from the S&amp;F servers.
    /// </summary>
    internal class SfResponse
    {
        /// <summary>
        ///     Creates a new <see cref="SfResponse" />.
        /// </summary>
        /// <param name="responseString">The response string of the web request.</param>
        public SfResponse(string responseString)
        {
            Errors = new List<string>();
            ParseResponseString(responseString);
        }

        /// <summary>
        ///     Contains errors as <see cref="string" /> if the request failed.
        /// </summary>
        public List<string> Errors { get; private set; }

        /// <summary>
        ///     Contains a response as <see cref="IResponse" />.
        /// </summary>
        public IResponse Response { get; private set; }

        private void ParseResponseString(string responseString)
        {
            // TODO response string parser class

            if (responseString == null)
                throw new ArgumentNullException("responseString");
            if (string.IsNullOrEmpty(responseString))
                throw new ArgumentException("Response string must not be empty.", "responseString");

            if (responseString.StartsWith("+"))
            {
//                throw new ArgumentException("Response string starts with \"+\".", "responseString");
                responseString = responseString.Substring(1);
            }

            if (responseString.StartsWith("E"))
            {
                if (responseString.Length < 4)
                    throw new ArgumentException(
                        "Error response string (starting with \"E\") must have a minimum length of 4.", "responseString");

                string errorString = responseString.Substring(1, 3);
                int errorCode;

                if (!int.TryParse(errorString, out errorCode))
                    throw new ArgumentException("Error code must be of type int.", "responseString");

                var error = (SF) (-errorCode);
                string[] errorArgs = responseString.Substring(4).Split(';');
                ProcessError(error, errorArgs);
                return;
            }

            if (responseString.Length < 3)
                throw new ArgumentException("Success response string must have a minimum length of 3.", "responseString");

            string successString = responseString.Substring(0, 3);
            int successCode;

            if (!int.TryParse(successString, out successCode))
                throw new ArgumentException("Success code must be of type int.", "responseString");

            var success = (SF) successCode;
            string[] successArgs = responseString.Substring(3).Split(';');
            ProcessSuccess(success, successArgs);
        }

        private void ProcessSuccess(SF success, string[] args)
        {
            switch (success)
            {
                case SF.RespLoginSuccess:
                case SF.RespLoginSuccessBought:
                    Response = new LoginResponse(args);
                    break;
                case SF.RespLogoutSuccess:
                    Response = new LogoutResponse(args);
                    break;
                case SF.ActScreenChar:
                case SF.RespPlayerScreen:
                    Response = new CharacterResponse(args);
                    break;
                case SF.ActScreenEhrenhalle:
                    string searchString = null;
                    if (args.Length > 1)
                        searchString = args[1];
                    string[] tmp = args[0].Split('/');
                    break;
                default:
                    throw new ArgumentOutOfRangeException("success");
            }
        }

        private void ProcessError(SF error, string[] args)
        {
            switch (error)
            {
                case SF.ErrLoginFailed:
                case SF.ErrSessionIdExpired:
                case SF.ErrServerDown:
                    Errors.Add(error.ToString());
                    break;
                default:
                    throw new ArgumentOutOfRangeException("error");
            }
        }
    }
}