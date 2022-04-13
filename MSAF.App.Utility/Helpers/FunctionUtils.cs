using Microsoft.AspNetCore.Http;
using MSAF.App.Utility.CommonModels;
using Newtonsoft.Json;
using System.Net;
using System.Text;


namespace MSAF.App.Utility.Helpers
{
    public static class FunctionUtils
    {
        public static HttpResponseMessage HandleSuccess<T>(HttpRequest req, T data)
        { 
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")
            };
        }

        public static HttpResponseMessage HandleException(HttpRequest req, string errorMessage)
        {
            var data = new ErrorResponse()
            {
                ErrorMessage = errorMessage
            };

            return new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")
            };
        }
    }
}
