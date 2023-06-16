using GSAFull.Data;
using GSAFull.utils.Objects;

namespace GSAFull.utils
{
    public class QueryProcessor
    {
        public static Dictionary<DateTime, decimal> CumulateStrategyPnls(List<Strategy> strategies)
        {
            var pnlDict = new Dictionary<DateTime, decimal>();

            foreach (var strategy in strategies)
            {
                var cumulativeStrategy = new GSAFull.Data.Strategy();
                cumulativeStrategy.StratName = strategy.StratName;

                var currentTotal = 0.0M;
                foreach (var pnl in strategy.Pnls)
                {
                    var currentDate = pnl.Date;
                    currentTotal += pnl.Amount;
                    pnlDict[currentDate] = currentTotal;
                }
            }
            return pnlDict;
        }
        public static List<CumulativeStrategyCapitals> CumulateStrategyCapitals(List<Strategy> strategies)
        {
            var cumulativeCapitalStrategies = new List<CumulativeStrategyCapitals>();

            foreach (var strategy in strategies)
            {
                var cumulativeStrategy = new CumulativeStrategyCapitals();
                cumulativeStrategy.StratName = strategy.StratName;

                var total = 0.0M;
                foreach (var capital in strategy.Capitals)
                {
                    total = capital.Amount + total;

                    var cumulativeCapital = new CumulativeCapital() { Date = capital.Date, Amount = total };
                    cumulativeStrategy.Capitals.Add(cumulativeCapital);
                }
                cumulativeCapitalStrategies.Add(cumulativeStrategy);
            }
            return cumulativeCapitalStrategies;
        }
    }
}
