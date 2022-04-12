namespace MSAF.App.DAL.SmokeTest
{
    public interface ISmokeTestRepo
    {
        Task<SmokeTestData> GetTestDataAsync(string data);
    }
}
