using Moq;
using ZenCore.Services;
using ZenCore.Services.ZenReporting;
using ZenReporting.Contracts;

namespace ZenCore.Tests
{
    public class ReportingServiceTests : IDisposable
    {
        private readonly Mock<IZenReportingClient> _zenReportingClientMock = new();
        private readonly ReportingService _sut;
        public ReportingServiceTests()
        {
            _sut = new(_zenReportingClientMock.Object);
        }

        [Fact]
        public async Task ShouldSendReportForAllUsers()
        {
            var user = new Entities.User { Name = "Test", Email = "test@mail.com" };
            var transactions = new List<Entities.Transaction>
            {
                new Entities.Transaction{ Amount = 100, Currency = "EUR", CreatedAt = DateTime.UtcNow},
                new Entities.Transaction{ Amount = 22.3m, Currency = "EUR", CreatedAt = DateTime.UtcNow},
                new Entities.Transaction{ Amount = -12.33m, Currency = "EUR", CreatedAt = DateTime.UtcNow}
            };

            var sum = transactions.Sum(x => x.Amount);

            await _sut.SendReportsForAllUsersAsync(user, transactions);

            _zenReportingClientMock.Verify(x => x.SendReportAsync(It.Is<Report>(
                r => r.User.Name == user.Name
                && r.Transactions.Count() == transactions.Count
                && r.User.Email == user.Email
                && r.TransactionsSum == sum
                && r.Date.Date == DateTime.UtcNow.Date)), Times.Once);
        }

        public void Dispose()
        {
            _zenReportingClientMock.VerifyNoOtherCalls();
        }
    }
}
