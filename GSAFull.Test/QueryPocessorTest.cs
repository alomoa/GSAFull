using GSAFull.Data;
using GSAFull.utils;

namespace Tests
{
    internal class QueryPocessorTest
    {
        [Test]
        public void ShouldReturnCumulativeCapitals()
        {
            //Arrange
            Strategy strategy = new Strategy();
            List<Capital> capitals = new List<Capital>()
            {
                new Capital(){ Date=new DateTime(2010, 1, 1), Amount= 1000 },
                new Capital(){ Date=new DateTime(2010, 2, 1), Amount= 2000 },
                new Capital(){ Date=new DateTime(2010, 3, 1), Amount= 500 },
                new Capital(){ Date=new DateTime(2010, 4, 1), Amount= 600 },
            };
            strategy.Capitals = capitals;

            List<Strategy> strategies = new List<Strategy>() { strategy };

            //Act
            var result = QueryProcessor.CumulateStrategyCapitals(strategies);

            //Assert
            Assert.That(result[0].Capitals.ElementAt(0).Amount, Is.EqualTo(1000));
            Assert.That(result[0].Capitals.ElementAt(1).Amount, Is.EqualTo(3000));
            Assert.That(result[0].Capitals.ElementAt(2).Amount, Is.EqualTo(3500));
            Assert.That(result[0].Capitals.ElementAt(3).Amount, Is.EqualTo(4100));
        }

        [Test]
        public void ShouldReturnCumulativePnls()
        {
            //Arrange
            Strategy strategy = new Strategy();
            List<Pnl> capitals = new List<Pnl>()
            {
                new Pnl(){ Date=new DateTime(2010, 1, 1), Amount= 1000 },
                new Pnl(){ Date=new DateTime(2010, 1, 2), Amount= -200 },
                new Pnl(){ Date=new DateTime(2010, 2, 1), Amount= 2000 },
                new Pnl(){ Date=new DateTime(2010, 2, 2), Amount= -3000 },
                new Pnl(){ Date=new DateTime(2010, 3, 1), Amount= 500 },
                new Pnl(){ Date=new DateTime(2010, 3, 2), Amount= -2500 },
                new Pnl(){ Date=new DateTime(2010, 4, 1), Amount= 600 },
                new Pnl(){ Date=new DateTime(2010, 4, 2), Amount= -100 },
            };
            strategy.Pnls = capitals;

            List<Strategy> strategies = new List<Strategy>() { strategy };

            //Act
            var result = QueryProcessor.CumulateStrategyPnls(strategies);
            var keys = result.Keys.OrderBy(k => k.Date).ToList();

            //Assert
            Assert.That(result[keys.ElementAt(0)], Is.EqualTo(1000));
            Assert.That(result[keys.ElementAt(1)], Is.EqualTo(800));
            Assert.That(result[keys.ElementAt(2)], Is.EqualTo(2800));
            Assert.That(result[keys.ElementAt(3)], Is.EqualTo(-200));
            Assert.That(result[keys.ElementAt(4)], Is.EqualTo(300));
            Assert.That(result[keys.ElementAt(5)], Is.EqualTo(-2200));
            Assert.That(result[keys.ElementAt(6)], Is.EqualTo(-1600));
            Assert.That(result[keys.ElementAt(7)], Is.EqualTo(-1700));


        }
    }
}
