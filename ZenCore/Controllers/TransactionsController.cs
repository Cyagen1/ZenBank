using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ZenCore.DataAccess;
using ZenCore.Entities;
using ZenCore.Models;
using ZenCore.Services;

namespace ZenCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IReportingService _reportingService;
        private readonly IMapper _mapper;

        public TransactionsController(ITransactionRepository transactionRepository, IUserRepository userRepository, IReportingService reportingService, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
            _reportingService = reportingService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransactionByIdAsync([FromRoute] Guid id)
        {
            var transaction = await _transactionRepository.GetTransactionByIdAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<TransactionDto>(transaction));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransactionAsync([FromBody] TransactionDto transactionDto)
        {
            var transaction = _mapper.Map<Transaction>(transactionDto);
            transaction.Id = new Guid();
            await _transactionRepository.CreateTransactionAsync(transaction);
            return Ok(transaction.Id);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTransactionAsync([FromBody] TransactionForUpdateDto transactionDto)
        {
            var updatedTransaction = await _transactionRepository.UpdateTransactionAsync(_mapper.Map<Transaction>(transactionDto));
            if (updatedTransaction == null)
            {
                return NotFound();
            }
            return Ok(updatedTransaction);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTransactionAsync([FromQuery] Guid transacitonId)
        {
            await _transactionRepository.DeleteTransactionAsync(transacitonId);
            return Ok();
        }

        [HttpPost("report")]
        public IActionResult SendReports()
        {
            Task.Run(async () =>
            {
                var users = await _userRepository.GetAllUsersAsync();
                foreach (var user in users)
                {
                    var transactions = _transactionRepository.FindUserTransactionsForToday(user.Id);
                    await _reportingService.SendReportsForAllUsersAsync(user, transactions);
                }
            });

            return Ok();
        }
    }
}
