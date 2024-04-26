using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ZenCore.Controllers;
using ZenCore.DataAccess;
using ZenCore.Entities;
using ZenCore.Models;
using ZenCore.Services;

namespace ZenCore.Tests
{
    public class TransactionsControllerTests : IDisposable
    {
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock = new();
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly Mock<IReportingService> _reportingServiceMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly TransactionsController _sut;
        public TransactionsControllerTests()
        {
            _sut = new(_transactionRepositoryMock.Object, _userRepositoryMock.Object, _reportingServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task ShouldGetTransactionById()
        {
            var transactionId = new Guid();
            var transaction = new Transaction { Id = transactionId, Amount = 10, Currency = "EUR", CreatedAt = DateTime.UtcNow };
            _transactionRepositoryMock.Setup(repo => repo.GetTransactionByIdAsync(transactionId)).ReturnsAsync(transaction);
            _mapperMock.Setup(mapper => mapper.Map<TransactionDto>(transaction)).Returns(new TransactionDto(transaction.Amount, transaction.Currency, transactionId, transaction.CreatedAt));

            var result = await _sut.GetTransactionByIdAsync(transactionId);

            _transactionRepositoryMock.Verify(x => x.GetTransactionByIdAsync(It.Is<Guid>(g => g.Equals(transactionId))), Times.Once);
            _mapperMock.Verify(x => x.Map<TransactionDto>(It.Is<Transaction>(t => t.Amount == transaction.Amount
            && t.Currency == transaction.Currency
            && t.CreatedAt == transaction.CreatedAt)), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var transactionDto = Assert.IsType<TransactionDto>(okResult.Value);

            Assert.NotNull(okResult);
            Assert.Equal(transaction.Amount, transactionDto.Amount);
            Assert.Equal(transaction.Currency, transactionDto.Currency);
            Assert.Equal(transaction.CreatedAt, transactionDto.CreatedAt);
        }

        [Fact]
        public async Task ShouldCreateNewTransaction()
        {
            var userId = new Guid();
            var transactionDto = new TransactionDto(10, "EUR", userId, DateTime.UtcNow);
            var transaction = new Transaction { Id = new Guid(), Amount = transactionDto.Amount, Currency = transactionDto.Currency, UserId = transactionDto.UserId, CreatedAt = transactionDto.CreatedAt};
            _mapperMock.Setup(mapper => mapper.Map<Transaction>(transactionDto)).Returns(transaction);

            var result = await _sut.CreateTransactionAsync(transactionDto);

            _transactionRepositoryMock.Verify(x => x.CreateTransactionAsync(It.Is<Transaction>(t => t.Amount == transaction.Amount
            && t.Currency == transaction.Currency
            && t.CreatedAt == transaction.CreatedAt
            && t.UserId == transaction.UserId)), Times.Once);

            _mapperMock.Verify(x => x.Map<Transaction>(It.Is<TransactionDto>(t => t.Amount == transaction.Amount
            && t.Currency == transaction.Currency
            && t.CreatedAt == transaction.CreatedAt
            && t.UserId == transaction.UserId)), Times.Once);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var transactionResult = Assert.IsType<Guid>(okResult.Value);

            Assert.NotNull(okResult);
            Assert.Equal(transactionResult, transaction.Id);
        }

        [Fact]
        public async Task ShouldDeleteTransaction()
        {
            var transactionId = new Guid();

            var result = await _sut.DeleteTransactionAsync(transactionId);

            _transactionRepositoryMock.Verify(x => x.DeleteTransactionAsync(It.Is<Guid>(g => g.Equals(transactionId))), Times.Once);

            var okResult = Assert.IsType<OkResult>(result);
            Assert.NotNull(okResult);
        }

        [Fact]
        public async Task ShouldFindAndUpdateTransaction()
        {
            var transactionId = new Guid();
            var transactionDto = new TransactionForUpdateDto(transactionId, 10, "EUR", DateTime.UtcNow);
            var transaction = new Transaction { Id = transactionId, Amount = 5, Currency = "USD" };
            var updatedTransaction = new Transaction { Id = transactionId, Amount = transactionDto.Amount, Currency = transactionDto.Currency };
            _mapperMock.Setup(mapper => mapper.Map<Transaction>(transactionDto)).Returns(transaction);
            _transactionRepositoryMock.Setup(repo => repo.UpdateTransactionAsync(transaction)).ReturnsAsync(updatedTransaction);

            var result = await _sut.UpdateTransactionAsync(transactionDto);

            _mapperMock.Verify(x => x.Map<Transaction>(It.Is<TransactionForUpdateDto>(u => u.Id == transactionId
            && u.Amount == updatedTransaction.Amount
            && u.Currency == updatedTransaction.Currency)), Times.Once);

            _transactionRepositoryMock.Verify(x => x.UpdateTransactionAsync(It.Is<Transaction>(u => u.Id == transactionId
            && u.Amount == transaction.Amount
            && u.Currency == transaction.Currency)), Times.Once);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var transactionResult = Assert.IsType<Transaction>(okResult.Value);

            Assert.NotNull(okResult);
            Assert.Equal(updatedTransaction.Amount, transactionResult.Amount);
            Assert.Equal(updatedTransaction.Currency, transactionResult.Currency);
        }

        [Fact]
        public async Task ShouldNotFindAndUpdateTransaction()
        {
            var transactionId = new Guid();
            var transactionDto = new TransactionForUpdateDto(transactionId, 10, "EUR", DateTime.UtcNow);
            _mapperMock.Setup(mapper => mapper.Map<Transaction>(transactionDto)).Returns(new Transaction());
            _transactionRepositoryMock.Setup(repo => repo.UpdateTransactionAsync(It.IsAny<Transaction>())).ReturnsAsync((Transaction)null);

            var result = await _sut.UpdateTransactionAsync(transactionDto);

            _mapperMock.Verify(x => x.Map<Transaction>(It.IsAny<TransactionForUpdateDto>()), Times.Once);

            _transactionRepositoryMock.Verify(x => x.UpdateTransactionAsync(It.IsAny<Transaction>()), Times.Once);

            Assert.IsType<NotFoundResult>(result);
        }

        public void Dispose()
        {
            _transactionRepositoryMock.VerifyNoOtherCalls();
            _userRepositoryMock.VerifyNoOtherCalls();
            _reportingServiceMock.VerifyNoOtherCalls();
            _mapperMock.VerifyNoOtherCalls();
        }
    }
}
