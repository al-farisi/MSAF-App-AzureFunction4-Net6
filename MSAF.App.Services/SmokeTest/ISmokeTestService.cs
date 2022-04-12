using MSAF.App.DAL.SmokeTest;

namespace MSAF.App.Services.SmokeTest
{
    public interface ISmokeTestService
    {
        Task<SmokeTestData> GetTestDataAsync(string data);
    }
}
