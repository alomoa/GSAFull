using GSAFull.Data;
using GSAFull.utils;

namespace GSAFull
{
    public class program
    {
        public static void Main(string[] args)
        {
            var strategyReader = new StrategyReader(new MyFileReader());

            DatabaseQuerier _databaseQuerier = new DatabaseQuerier(new StrategyContext());
            ConsoleHelpers consoleHelpers = new ConsoleHelpers(_databaseQuerier, strategyReader);

            Console.WriteLine("Please enter either load-data, capital [strategy name strategy name], cumulative-pnl [region] (US, EU, AP)");

            consoleHelpers.ProcessCommands();
        }


    }
}