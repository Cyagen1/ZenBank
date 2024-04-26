using Microsoft.AspNetCore.Mvc;
using ZenReporting.Contracts;
using ZenReporting.Services;

namespace ZenReporting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportingController : ControllerBase
    {
        private readonly IPdfService _pdfService;
        private readonly IMailService _mailService;

        public ReportingController(IPdfService pdfService, IMailService mailService)
        {
            _pdfService = pdfService;
            _mailService = mailService;
        }

        [HttpPost("mail")]
        public IActionResult SendPdfToMail([FromBody]Report report)
        {
            using var stream = _pdfService.CreatePdf(report);
            _mailService.SendMail(stream, report.User);
            return Ok();
        }
    }
}
