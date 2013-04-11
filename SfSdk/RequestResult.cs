using System;
using System.Collections.Generic;
using SfSdk.Data;
using SfSdk.Enums;
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
                var fail = (SfFail) int.Parse(responseString.Substring(1, 3));
                string[] failArgs = responseString.Substring(4).Split(';');
                ProcessFail(fail, failArgs);
                return;
            }

            var success = (SfSuccess) int.Parse(responseString.Substring(0, 3));
            string[] successArgs = responseString.Substring(3).Split(';');
            ProcessSuccess(success, successArgs);
        }

        private void ProcessSuccess(SfSuccess success, string[] args)
        {
            string[] savegameParts;
            switch (success)
            {
                case SfSuccess.LoginSuccess:
                case SfSuccess.LoginSuccessBought:
                    if (args.Length < 3) throw new NotImplementedException();
                    string sessionId = args[2];
                    savegameParts = ("0/" + args[0]).Split('/');
                    Response = new LoginResponse(savegameParts, sessionId);
                    break;
                case SfSuccess.LogoutSuccess:
                    Response = new LogoutResponse(true);
                    break;
                case SfSuccess.Character:
                case SfSuccess.SearchedPlayerFound:
                    savegameParts = ("0/" + args[0]).Split('/');
                    string guild = args[2];
                    string comment = args[1];
                    Response = new CharacterResponse(savegameParts, guild, comment); // TODO character response
                    break;
                case SfSuccess.HallOfFame:
                    string searchString = null;
                    if (args.Length > 1)
                        searchString = args[1];
                    string[] tmp = args[0].Split('/');
                    break;
                case SfSuccess.SearchedPlayerNotFound:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException(string.Format("Success: {0}", success));
            }
        }

        private void ProcessFail(SfFail fail, string[] args)
        {
            switch (fail)
            {
                case SfFail.LoginFailed:
                case SfFail.SessionIdExpired:
                case SfFail.ServerDown:
                    Errors.Add(fail.ToString());
                    break;
                default:
                    throw new NotImplementedException(fail.ToString());
            }
        }
    }
}