#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : checkhost.cs
 * 
 * Contents	: Implementation of the logic to check user's license
 * 
 * Author	: Sergey Fasonov
 * 
 */
#endregion

using System;
using castleshield.securemail.core.tasks;
using cfsdrive.logic.services.rest;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

namespace cfsdrive.logic.tasks
{
    public class CheckHost : BaseTask
    {
        private string _result;
        private string _tenantUrl;

        public CheckHost(string tenantUrl, Action<ITaskResult> completionCallback) :
            base("CheckHost", completionCallback)
        {
            _tenantUrl = tenantUrl;
        }

        protected override void OnRun()
        {
            Uri uriResult;
            if (!ValidHttpURL(_tenantUrl, out uriResult))
            {
                _result = string.Empty;
            }
            else
            {
                _tenantUrl = uriResult?.AbsoluteUri.Remove(uriResult.AbsoluteUri.Length - 1, 1);
                Ping pingSender = new Ping();
                PingReply result = pingSender.SendPingAsync(uriResult?.Host).Result;
                Api.Instance.Logger.LogInfo($"Check {_tenantUrl} availability: {result.Buffer.Length} bytes from {result.Address}, status={result.Status} time={result.RoundtripTime}ms");
                _result = _tenantUrl;
            }
        }

        protected override void ExecutionFinished(ITaskResult result)
        {
            try
            {
                if (result.Success)
                {
                    result.Result = _result;
                }

                _completionCallback?.Invoke(result);
            }
            catch (Exception ex)
            {
                Api.Instance.Logger.LogError(ex, $"The exception occured in the ExecutionFinished of task {Name}");
            }

            Api.Instance.Logger.LogInfo($"Task {Name} runs the continuous task.");
        }

        private bool ValidHttpURL(string s, out Uri resultURI)
        {
            if (!Regex.IsMatch(s, @"^https?:\/\/", RegexOptions.IgnoreCase))
                s = "https://" + s;

            if (Uri.TryCreate(s, UriKind.Absolute, out resultURI))
                return (resultURI.Scheme == Uri.UriSchemeHttp ||
                        resultURI.Scheme == Uri.UriSchemeHttps);

            return false;
        }
    }
}
