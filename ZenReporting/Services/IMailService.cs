using ZenReporting.Contracts;

namespace ZenReporting.Services
{
    public interface IMailService
    {
        void SendMail(MemoryStream stream, User user);
    }
}
