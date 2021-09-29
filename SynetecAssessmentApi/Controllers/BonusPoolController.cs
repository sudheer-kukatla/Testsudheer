using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SynetecAssessmentApi.Dtos;
using SynetecAssessmentApi.Services;
using System.Threading.Tasks;

namespace SynetecAssessmentApi.Controllers
{
    [Route("api/[controller]")]
    public class BonusPoolController : Controller
    {
        private readonly IBonusPoolService _bonusPoolService;
        private readonly ILogger _logger;
        public BonusPoolController(ILogger<BonusPoolController> logger, IBonusPoolService bonusPoolService)
        {
            _bonusPoolService = bonusPoolService;
            _logger = logger;
        }

        

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bonusPoolService = new BonusPoolService();

            return Ok(await bonusPoolService.GetEmployeesAsync());
        }

        [HttpPost()]
        public async Task<IActionResult> CalculateBonus([FromBody] CalculateBonusDto request)
        {
            try
            {
                _logger.LogInformation($"Request body message:{JsonConvert.SerializeObject(request)}");
                return Ok(await _bonusPoolService.CalculateAsync(
               request.TotalBonusPoolAmount,
               request.SelectedEmployeeId));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Error Message: {ex.Message}, Stacktrace:{ex.StackTrace}");
                return BadRequest(ex.Message);
            }
           
        }
    }
}
