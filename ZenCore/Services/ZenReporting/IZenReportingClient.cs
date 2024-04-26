using ZenReporting.Contracts;

namespace ZenCore.Services.ZenReporting
{
    public interface IZenReportingClient
    {
        Task SendReportAsync(Report report);
    }
}
