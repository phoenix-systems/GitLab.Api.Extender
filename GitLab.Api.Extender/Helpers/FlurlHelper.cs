using Flurl.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace GitLab.Api.Extender.Helpers
{
    public class FlurlHelper
    {
        public static async Task<T> GetJsonAsync<T>(string url, object data, object headers = null, int tryCount = 3, int tryDelayMs = 100)
        {
            return await Retry.Try(() => url.WithHeaders(headers).SetQueryParams(data).GetJsonAsync<T>(), NeedToRetryException, tryCount, tryDelayMs);
        }

        public static async Task PostJsonAsync(string url, object data, object headers = null, int tryCount = 3, int tryDelayMs = 100, int timeoutSec = 60)
        {
            await Retry.Try(() => url.WithTimeout(TimeSpan.FromSeconds(timeoutSec)).WithHeaders(headers).PostJsonAsync(data), NeedToRetryException, tryCount, tryDelayMs);
        }

        private static bool NeedToRetryException(Exception ex)
        {
            if (!(ex is FlurlHttpException flurlException))
            {
                return false;
            }

            var isTimeout = flurlException is FlurlHttpTimeoutException;
            if (isTimeout)
            {
                return true;
            }

            if (flurlException.Call.HttpStatus == HttpStatusCode.ServiceUnavailable ||
                flurlException.Call.HttpStatus == HttpStatusCode.InternalServerError ||
                flurlException.Call.HttpStatus == HttpStatusCode.TooManyRequests)
            {
                return true;
            }

            return false;
        }
    }
}