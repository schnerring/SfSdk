using System;
using System.Collections.Generic;
using System.Globalization;
using SfSdk.Constants;
using SfSdk.ResponseData;

namespace SfSdk
{
    internal class RequestResult
    {
        internal RequestResult(string responseString)
        {
            Errors = new List<string>();
            ParseResponseString(responseString);
        }

        public List<string> Errors { get; private set; }

        public IResponse Response { get; private set; }

        private void ParseResponseString(string responseString)
        {
            if (responseString.StartsWith("+"))
                responseString = responseString.Substring(1);

            if (responseString.StartsWith("E"))
            {
                var fail = (SF) (-int.Parse(responseString.Substring(1, 3)));
                string[] failArgs = responseString.Substring(4).Split(';');
                ProcessFail(fail, failArgs);
                return;
            }

            var success = (SF) int.Parse(responseString.Substring(0, 3));
            string[] successArgs = responseString.Substring(3).Split(';');
            ProcessSuccess(success, successArgs);
        }

        private void ProcessSuccess(SF success, string[] args)
        {
            string[] savegameParts;
            switch (success)
            {
                case SF.RespLoginSuccess:
                case SF.RespLoginSuccessBought:
                    Response = new LoginResponse(args);
                    break;
                case SF.RespLogoutSuccess:
                    Response = new LogoutResponse(args, logoutSucceeded: true);
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
//                case SF.ActScreenEhrenhalle:
//                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException(string.Format("Success: {0}", success));
            }
        }

        private void ProcessFail(SF fail, string[] args)
        {
            switch (fail)
            {
                case SF.ErrLoginFailed:
                case SF.ErrSessionIdExpired:
                case SF.ErrServerDown:
                    Errors.Add(fail.ToString(CultureInfo.InvariantCulture));
                    break;
                default:
                    throw new NotImplementedException(fail.ToString(CultureInfo.InvariantCulture));
            }
        }
    }
}