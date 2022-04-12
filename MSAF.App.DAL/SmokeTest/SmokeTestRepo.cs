namespace MSAF.App.DAL.SmokeTest
{
    public class SmokeTestRepo : ISmokeTestRepo
    {
        public async Task<SmokeTestData> GetTestDataAsync(string data)
        {
            return new SmokeTestData()
            {
                Data = data,
                IsRepositoryOK = true,
                IsServiceOK = false
            };
        }
    }
}
