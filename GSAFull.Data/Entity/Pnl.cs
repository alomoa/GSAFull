using System;
using System.Collections.Generic;

namespace GSAFull.Data
{
    public partial class Pnl
    {
        public int PnlId { get; set; }

        public int StrategyId { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public virtual Strategy Strategy { get; set; } = null!;
    }
}