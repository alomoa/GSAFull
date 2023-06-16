using GSAFull.Data;
using GSAFull.utils;
using GSAFull.utils.Objects;
using Microsoft.AspNetCore.Mvc;


namespace GSAFull.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GSAController : ControllerBase
    {
        private readonly ILogger<GSAController> _logger;
        private readonly string[] _regions = { "AP", "EU", "US" };
        private DatabaseQuerier _databaseQuerier;
        public GSAController(ILogger<GSAController> logger, DatabaseQuerier databaseQuerier)
        {
            _logger = logger;
            _databaseQuerier = databaseQuerier;
        }

        [HttpGet]
        [Route("GetCumulativePnl")]
        public Dictionary<DateTime, decimal> GetCumulativePnl(string region) {

            if (!_regions.Contains(region.ToUpper())){
                throw new HttpRequestException("Invalid region name");
            }
            else
            {
                var strategies = _databaseQuerier.GetStrategiesWithPnlsFromRegion(region);
                var result = QueryProcessor.CumulateStrategyPnls(strategies);
                return result;
            }
        }

        [HttpPost]
        [Route("GetCumulativeCapital")]
        public List<CumulativeStrategyCapitals> GetCumulativeCapital(string[] strategies)
        {
            //Check if strategies exist
            var exists = _databaseQuerier.DoesExist(strategies);

            if (!exists)
            {
                throw new HttpRequestException("One of the strategies provided does not exist");
            }
            var result = _databaseQuerier.GetStrategiesWithCapitals(strategies);
            var cumulativeStrategies = QueryProcessor.CumulateStrategyCapitals(result);
            return cumulativeStrategies;

        }
    }
}
