using GSAFull.Data;
using GSAFull.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GSAFull.utils
{
    // TODO: Single responsiblity principle. Decouple the code! 
    public class DatabaseQuerier
    {

        private readonly StrategyContext _dbContext;

        public DatabaseQuerier(StrategyContext dbContext)
        {
            _dbContext = dbContext;
            var result = _dbContext.Strategies;
        }

        public bool DoesExist(string[] strategies)
        {
            foreach (var strategy in strategies)
            {
               var result = _dbContext.Strategies.Where(x => x.StratName == strategy).ToList();
               if(result.IsNullOrEmpty())
                {
                    return false;
                }
            }
            return true;
        }

        public List<GSAFull.Data.Strategy> GetStrategiesWithCapitals(string[] strategyNames)
        {
            var strategies = _dbContext.Strategies.Where(x => strategyNames.Contains(x.StratName))
                .Include(x => x.Capitals)
                .ToList();

            foreach (var strategy in strategies)
            {
                strategy.Capitals = strategy.Capitals.OrderBy(x => x.Date).ToList();
            }

            return strategies;
        }

        public List<GSAFull.Data.Strategy> GetStrategiesWithPnlsFromRegion(string region)
        {
            var strategies = new List<GSAFull.Data.Strategy>();

            strategies = _dbContext.Strategies.Where(x => x.Region == region)
                .Include(x => x.Pnls)
                .ToList();

            return strategies;
        }

        public void SaveToDb(List<models.Strategy> strategies)
        {
            _dbContext.RemoveRange(_dbContext.Capitals);
            _dbContext.RemoveRange(_dbContext.Pnls);
            _dbContext.RemoveRange(_dbContext.Strategies);
            _dbContext.SaveChanges();

            foreach (models.Strategy strategy in strategies)
            {
                GSAFull.Data.Strategy dbStrategies = new();
                dbStrategies.StratName = strategy.StratName;
                dbStrategies.Region = strategy.Region;

                _dbContext.Strategies.Add(dbStrategies);
                _dbContext.SaveChanges();

                List<Data.Pnl> dbPnls = new();
                List<Data.Capital> dbCapitals = new();

                foreach (var pnl in strategy.Pnl)
                {
                    var dbPnl = new Data.Pnl()
                    {
                        StrategyId = dbStrategies.StrategyId,
                        Date = pnl.Date,
                        Amount = pnl.Amount,
                    };

                    dbPnls.Add(dbPnl);
                }

                foreach (var capital in strategy.Capital)
                {
                    var dbCapital = new Data.Capital()
                    {
                        StrategyId = dbStrategies.StrategyId,
                        Date = capital.Date,
                        Amount = capital.Amount
                    };

                    dbCapitals.Add(dbCapital);
                }

                dbStrategies.Capitals = dbCapitals;
                dbStrategies.Pnls = dbPnls;
                _dbContext.SaveChanges();
            }
        }

        internal void ResetDatabase()
        {
            _dbContext.RemoveRange(_dbContext.Capitals);
            _dbContext.RemoveRange(_dbContext.Pnls);
            _dbContext.RemoveRange(_dbContext.Strategies);
            _dbContext.SaveChanges();
        }
    }
}