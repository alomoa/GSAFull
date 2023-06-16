namespace GSAFull.utils.Objects
{
    public class CumulativeStrategyPnls
    {
        public CumulativeStrategyPnls()
        {
            Pnls = new List<CumulativePnl>();
        }
        public string StratName { get; set; }
        public List<CumulativePnl> Pnls;
    }
}
