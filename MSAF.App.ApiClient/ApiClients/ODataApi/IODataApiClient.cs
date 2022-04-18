namespace MSAF.App.ApiClient.ApiClients.ODataApi
{
    public interface IODataApiClient
    {
        Task<List<ODataApiResponse>> GetWeatherForecastBySummary(string summary);
    }
}
