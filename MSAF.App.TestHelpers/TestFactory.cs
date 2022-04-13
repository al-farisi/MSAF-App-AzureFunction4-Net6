using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MSAF.App.TestHelpers
{
    public static class TestFactory
    {
        public static ILogger CreateLogger(LoggerTypes type = LoggerTypes.Null)
        {
            ILogger logger;

            if (type == LoggerTypes.List)
            {
                logger = new ListLogger();
            }
            else
            {
                logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");
            }

            return logger;
        }

        public static HttpRequest CreateHttpRequest(Dictionary<string, string> queryStrings, Dictionary<string, string> headers, object bodyContent)
        {
            var context = new DefaultHttpContext();
            var request = context.Request;
            if (queryStrings != null && queryStrings.Count() >= 0)
            {
                QueryString query = new QueryString();
                foreach (var queryString in queryStrings)
                {
                    request.QueryString += query.Add(queryString.Key, queryString.Value);
                }
            }
            if (headers != null && headers.Count() >= 0)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }
            if (bodyContent != null)
            {
                request.Body = Serialize(bodyContent);
                request.ContentType = "application/json";
            }
            return request;
        }

        //public static HttpRequestData CreateHttpRequestData(Dictionary<string, string> queryStrings, Dictionary<string, string> headers, object bodyContent)
        //{
        //    var context = new DefaultHttpContext();
        //    var request = context.Request;
        //    if (queryStrings != null && queryStrings.Count() >= 0)
        //    {
        //        QueryString query = new QueryString();
        //        foreach (var queryString in queryStrings)
        //        {
        //            request.QueryString += query.Add(queryString.Key, queryString.Value);
        //        }
        //    }
        //    if (headers != null && headers.Count() >= 0)
        //    {
        //        foreach (var header in headers)
        //        {
        //            request.Headers.Add(header.Key, header.Value);
        //        }
        //    }
        //    if (bodyContent != null)
        //    {
        //        request.Body = Serialize(bodyContent);
        //        request.ContentType = "application/json";
        //    }
        //    return request;
        //}

        public static Stream Serialize(object data)
        {
            var stream = new MemoryStream();
            var streamWriter = new StreamWriter(stream);
            var jsonTextWriter = new JsonTextWriter(streamWriter);
            var jsonSerializer = new JsonSerializer();
            jsonSerializer.Serialize(jsonTextWriter, data);
            jsonTextWriter.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
        private static Dictionary<string, StringValues> CreateDictionary(string key, string value)
        {
            var qs = new Dictionary<string, StringValues>
            {
                { key, value }
            };
            return qs;
        }
    }
}
