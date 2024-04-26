using ZenReporting.Contracts;

namespace ZenCore.Services.ZenReporting
{
    public class ZenReportingHttpClient : IZenReportingClient
    {
        private readonly HttpClient _httpClient;
        private readonly ZenReportingSettings _zenReportingSettings;

        public ZenReportingHttpClient(HttpClient httpClient, ZenReportingSettings zenReportingSettings)
        {
            ArgumentNullException.ThrowIfNull(httpClient);
            ArgumentNullException.ThrowIfNull(zenReportingSettings);
            _httpClient = httpClient;
            _zenReportingSettings = zenReportingSettings;
        }

        public async Task SendReportAsync(Report report)
        {
            var response = await _httpClient.PostAsJsonAsync(new Uri(_zenReportingSettings.BaseUri, _zenReportingSettings.ReportsPath), report);
            response.EnsureSuccessStatusCode();
        }
    }
}
