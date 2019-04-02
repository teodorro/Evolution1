using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model;
using Xunit;

namespace TestModel
{
    public class TestGame
    {
        [Fact]
        public void TestGameCreate()
        {
            var players = new List<IPlayer>() {new Player()};
            var game = GameFactory.Instance.GetGame(players);

            Assert.NotEmpty(game.CardsRemain);
            Assert.NotEmpty(game.Players);
        }

        /// <summary>
        /// Actually works not always
        /// </summary>
        [Fact]
        public void TestMixedCards()
        {
            var players = new List<IPlayer>() { new Player() };
            var game1 = GameFactory.Instance.GetGame(players);
            Task.Delay(25).Wait();
            var game2 = GameFactory.Instance.GetGame(players);
            var u111 = game1.CardsRemain.First().Upgrade1.UpgradeType;
            var u112 = game1.CardsRemain.First().Upgrade2?.UpgradeType;
            var u121 = game1.CardsRemain.Last().Upgrade1.UpgradeType;
            var u122 = game1.CardsRemain.Last().Upgrade2?.UpgradeType;
            var u211 = game2.CardsRemain.First().Upgrade1.UpgradeType;
            var u212 = game2.CardsRemain.First().Upgrade2?.UpgradeType;
            var u221 = game2.CardsRemain.Last().Upgrade1.UpgradeType;
            var u222 = game2.CardsRemain.Last().Upgrade2?.UpgradeType;
            Assert.False(u111 == u211 && u112 == u212 && u121 == u221 && u122 == u222);
        }
    }
}
