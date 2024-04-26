using ZenReporting.Contracts;

namespace ZenReporting.Services
{
    public interface IPdfService
    {
        MemoryStream CreatePdf(Report report);
    }
}
