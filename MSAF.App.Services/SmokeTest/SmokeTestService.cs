using MSAF.App.DAL.SmokeTest;

namespace MSAF.App.Services.SmokeTest
{
    public class SmokeTestService : ISmokeTestService
    {
        private readonly ISmokeTestRepo _repo;

        public SmokeTestService(
                ISmokeTestRepo repo)
        {
            _repo = repo;
        }

        public async Task<SmokeTestData> GetTestDataAsync(string data)
        {
            var response = await _repo.GetTestDataAsync(data);
            response.IsServiceOK = true;

            return response;
        }
    }
}
