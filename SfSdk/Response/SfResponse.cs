using System;
using System.Collections.Generic;
using SfSdk.Constants;
using SfSdk.Logging;

namespace SfSdk.Response
{    /// <summary>
    ///     A S&amp;F response.
    /// </summary>
    internal interface ISfResponse
    {
        /// <summary>
        ///     Contains errors as <see cref="string" /> if the request failed.
        /// </summary>
        List<SF> Errors { get; }

        /// <summary>
        ///     Contains a response as <see cref="IResponse" />.
        /// </summary>
        IResponse Response { get; }
    }

    /// <summary>
    ///     Provides easier processing of response strings from a S&amp;F data source.
    /// </summary>
    internal class SfResponse : ISfResponse
    {
        private static readonly ILog Log = LogManager.GetLog(typeof(SfResponse));
        private readonly Uri _serverUri;

        /// <summary>
        ///     Creates a new <see cref="SfResponse" />.
        /// </summary>
        /// <param name="responseString">The response string of the request.</param>
        /// <param name="serverUri">The server <see cref="Uri"/>.</param>
        /// <exception cref="ArgumentNullException">When responseString or serverUri is null.</exception>
        /// <exception cref="ArgumentException">When response string is empty.</exception>
        public SfResponse(string responseString, Uri serverUri)
        {
            if (responseString == null)
                throw new ArgumentNullException("responseString");
            if (string.IsNullOrWhiteSpace(responseString))
                throw new ArgumentException("Response string must not be empty.", "responseString");
            if (serverUri == null)
                throw new ArgumentNullException("serverUri");

            _serverUri = serverUri;
            Errors = new List<SF>();
            ParseResponseString(responseString);
        }

        /// <summary>
        ///     Contains errors as <see cref="string" /> if the request failed.
        /// </summary>
        public List<SF> Errors { get; private set; }

        /// <summary>
        ///     Contains a response as <see cref="IResponse" />.
        /// </summary>
        public IResponse Response { get; private set; }

        private void ParseResponseString(string responseString)
        {
            if (responseString.StartsWith("+"))
            {
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
                    Response = new CharacterResponse(args);
                    break;
                case SF.RespPlayerScreen:
                    Response = new CharacterResponse(args);
                    break;
                case SF.ActScreenEhrenhalle:
                    // TODO was hier?
//                    var lastGuildShown = string.Empty;
                case SF.RespScreenGildenhalle:
                    Response = new HallOfFameResponse(args);
                    break;
                case SF.ActGuildJoinAttack:
                    // TODO
                    break;
                case SF.RespAlbum:
                    Response = new ScrapbookResponse(args, _serverUri);
                    break;
                default:
                    var e = new ArgumentOutOfRangeException("success");
                    Log.Error(e);
                    throw e;
            }
        }

        private void ProcessError(SF error, string[] args)
        {
            switch (error)
            {
                case SF.ErrLoginFailed:
                case SF.ErrSessionIdExpired:
                case SF.ErrServerDown:
                case SF.ErrNoAlbum:
                    Errors.Add(error);
                    break;
                default:
                    var e = new ArgumentOutOfRangeException("error");
                    Log.Error(e);
                    throw e;
            }
        }
    }
}