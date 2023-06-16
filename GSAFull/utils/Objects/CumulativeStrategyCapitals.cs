namespace GSAFull.utils.Objects
{
    public class CumulativeStrategyCapitals
    {
        public CumulativeStrategyCapitals()
        {
            Capitals = new List<CumulativeCapital>();
        }
        public string StratName { get; set; }
        public List<CumulativeCapital> Capitals { get; set; }
    }
}
