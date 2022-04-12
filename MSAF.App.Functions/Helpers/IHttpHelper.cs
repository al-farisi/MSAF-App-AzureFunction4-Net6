using Microsoft.Azure.Functions.Worker.Http;

namespace MSAF.App.Functions.Helpers
{
    public interface IHttpHelper
    {
        public Task<HttpResponseData> CreateSuccessfulHttpResponse(HttpRequestData req, object data);
    }
}
