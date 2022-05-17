using DTO.General.Api.Enum;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.Integration
{
    public class ApiDispatcher
    {
        internal readonly JsonSerializerSettings _serializerSettings;

        public ApiDispatcher(JsonSerializerSettings customSettings = null)
        {
            _serializerSettings = customSettings ?? new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        public async Task<string> DispatchWithResponseUnDeserializeAsync(
            string url,
            RequestMethodEnum method,
            object body = null,
            Tuple<HttpRequestHeader, string>[] headers = null,
            Tuple<string, string>[] customHeaders = null)
        {
            return await SendRequestAsync(url, method, body, headers, customHeaders);
        }

        public async Task<T> DispatchWithResponseAsync<T>(
            string url,
            RequestMethodEnum method,
            object body = null,
            Tuple<HttpRequestHeader, string>[] headers = null,
            Tuple<string, string>[] customHeaders = null)
        {
            var result = await SendRequestAsync(url, method, body, headers, customHeaders);
            return JsonConvert.DeserializeObject<T>(result, _serializerSettings);
        }

        private static HttpWebRequest CreateRequest(
            string url,
            RequestMethodEnum method,
            Tuple<HttpRequestHeader, string>[] headers = null,
            string contentType = null,
            Tuple<string, string>[] customHeaders = null)
        {
            if (url.StartsWith("https://"))
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            }

            var request = (HttpWebRequest)WebRequest.Create(url);
            string userAgent = null;
            if (headers != null)
                foreach (var header in headers)
                {
                    if (header.Item1 == HttpRequestHeader.ContentType) { contentType ??= header.Item2; continue; }
                    if (header.Item1 == HttpRequestHeader.UserAgent) { userAgent ??= header.Item2; continue; }
                    if (header.Item1 == HttpRequestHeader.Accept)
                        request.Accept = header.Item2;
                    else
                        request.Headers.Add(header.Item1, header.Item2);
                }

            if (customHeaders != null)
                foreach (var header in customHeaders)
                {
                    if (header.Item1.ToLower() == "content-type") { contentType ??= header.Item2; continue; }
                    if (header.Item1.ToLower() == "user-agent") { userAgent ??= header.Item2; continue; }
                    request.Headers[header.Item1] = header.Item2;
                }

            request.ContentType = contentType ?? "application/json; charset=UTF-8";
            request.Method = method.ToString();
            request.UserAgent = userAgent;

            return request;
        }

        private async Task<string> SendRequestAsync(
            string url,
            RequestMethodEnum method,
            object body,
            Tuple<HttpRequestHeader, string>[] headers,
            Tuple<string, string>[] customHeaders = null)
        {
            var request = CreateRequest(url, method, headers, null, customHeaders);
            await WriteRequestBodyAsync(request, body);

            try
            {
                using var response = (HttpWebResponse)(await request.GetResponseAsync());
                using var responseStream = response.GetResponseStream();
                using var streamReader = new StreamReader(responseStream);
                return streamReader.ReadToEnd();
            }
            catch (WebException e)
            {
                if (e.Response != null)
                {
                    if (((int)((HttpWebResponse)e.Response).StatusCode) == 429)
                    {
                        await Task.Delay(5000);
                        return await SendRequestAsync(url, method, body, headers);
                    }

                    if (((int)((HttpWebResponse)e.Response).StatusCode) == 404 || ((int)((HttpWebResponse)e.Response).StatusCode) == 422)
                        return string.Empty;
                }

                var error = UnwrapWebException(e);
                throw new Exception(error);
            }
        }

        protected async Task WriteRequestBodyAsync(
            HttpWebRequest request,
            object body)
        {
            if (body == null)
                return;

            string requestBody;
            if (request.ContentType.Contains("json"))
            {
                requestBody = JsonConvert.SerializeObject(
                body,
                Formatting.Indented,
                _serializerSettings);
            }
            else
                requestBody = body.ToString();

            using var requestStream = await request.GetRequestStreamAsync();
            var data = new UTF8Encoding().GetBytes(requestBody);
            requestStream.Write(data, 0, data.Length);
        }

        private static string UnwrapWebException(WebException ex)
        {
            try
            {
                var webResponse = ex.Response;
                if (webResponse == null)
                    return null;

                var reader = webResponse.GetResponseStream();
                var content = new StreamReader(reader).ReadToEnd();
                var responseJson = JsonConvert.DeserializeObject(content);
                return responseJson?.ToString();
            }
            catch
            {
                return ex.Message;
            }
        }
    }
}
