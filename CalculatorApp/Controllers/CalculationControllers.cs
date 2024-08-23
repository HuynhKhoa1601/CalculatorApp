using Microsoft.AspNetCore.Mvc;
using CalculatorApp.Models;
using CalculatorApp.Interfaces;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text;

namespace CalculatorApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CalculatorController : ControllerBase
    {
        private readonly ICalculationServices _calculationService;
        private readonly ILogger<CalculatorController> _logger;

        public CalculatorController(ICalculationServices calculationService, ILogger<CalculatorController> logger)
        {
            _calculationService = calculationService;
            _logger = logger;
        }


        [HttpPost("calculate/xml")]
        public async Task<IActionResult> CalculateXml()
        {
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                try
                {
                    string input = await reader.ReadToEndAsync();
                    var result = _calculationService.CalculateFromXml(input);
                    _calculationService.RegisterOperation("abcde", (IEnumerable<string> x) => 1.1);
                    return Ok(new { Result = result });
                }
                catch (Exception)
                {
                    return StatusCode(500, "An error occurred while processing the XML input.");
                }
            }
        }
    }
}
