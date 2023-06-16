using GSAFull.Data;

namespace GSAFull.utils
{
    public class ConsoleHelpers
    {

        DatabaseQuerier _databaseQuerier;
        StrategyReader _strategyReader;

        public ConsoleHelpers(DatabaseQuerier databaseQuerier, StrategyReader strategyReader)
        {
            _databaseQuerier = databaseQuerier;
            _strategyReader = strategyReader;   
        }

        public void ProcessCommands()
        {
            while (true)
            {
                var command = Console.ReadLine().ToLower();
                ProcessCommands(command);
            }
        }


        public void ProcessCommands(string command)
        {
            var commandParts = command.Split(' ');

            if (commandParts[0] == "capital")
            {
                var strategies = commandParts.Skip(1).ToArray();

                ProcessCapital(strategies);

            }
            else if (commandParts[0] == "cumulative-pnl")
            {
                var region = commandParts[1];

                ProcessCumulativePnL(region);
            }
            else if (commandParts[0] == "load-data")
            {
                ProcessLoadCSV();
                Console.WriteLine("Data loaded");
            }
            else
            {
                Console.WriteLine("Invalid command.");
            }
        }

        public void ProcessLoadCSV()
        {
            _databaseQuerier.ResetDatabase();
            var strategies = _strategyReader.Execute();
            _databaseQuerier.SaveToDb(strategies);
        }

        public void ProcessCapital(string[] strategies)
        {
            
            var strategiesFromDB = _databaseQuerier.GetStrategiesWithCapitals(strategies);

            var results = QueryProcessor.CumulateStrategyCapitals(strategiesFromDB);

            for (int i = 0; i < results[0].Capitals.Count(); i++)
            {
                foreach (var result in results)
                {
                    var capital = result.Capitals.ElementAt(i);

                    Console.WriteLine($"strategy: {result.StratName}, date: {capital.Date.ToString("yyyy-MM-dd")}, capital: {capital.Amount.ToString("0")}");
                }
            }
        }

        public void ProcessCumulativePnL(string region)
        {
            string[] regions = { "AP", "EU", "US" };
            if (!regions.Contains(region.ToUpper())){
                Console.WriteLine($"Region {region} does not exist");
            }

            var strategies = _databaseQuerier.GetStrategiesWithPnlsFromRegion(region);

            var result = QueryProcessor.CumulateStrategyPnls(strategies);

            var keys = result.Keys;
            keys.OrderBy(x => x.Date).ToList();

            foreach (var key in keys)
            {
                Console.WriteLine($"date: {key.ToString("yyyy-MM-dd")} cumulativePnl:{result[key]}");
            }
        }
    }
}
