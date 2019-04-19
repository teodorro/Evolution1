using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Model;
using Model.Upgrades;
using Xunit;

namespace TestModel.TestUpgrade
{
    public class TestUpgradeCarnivorous
    {
        private Animal _victim;
        private Game _game;

        [Fact]
        public void TestCarnivorousSimple()
        {
            ArrangeTwoAnimals(out var player1, new List<UpgradeSingle>(){new UpgradeCarnivorous()}, out var player2, new List<UpgradeSingle>());
            _victim = player2.Animals.First();
            _game.VictimChoose += OnVictimChoose;

            (player1.Animals.First().Upgrades.First(x => x.UpgradeType == UpgradeType.Carnivorous) as UpgradeCarnivorous).Use();

            Assert.Empty(player2.Animals);
            Assert.Equal(2, player1.Animals.First().FoodGot);

        }

        private void ArrangeTwoAnimals(out Player player1, List<UpgradeSingle> upgrades1, out Player player2, List<UpgradeSingle> upgrades2)
        {
            player1 = new Player("player1");
            player2 = new Player("player2");
            var players = new List<IPlayer>() {player1, player2};
            var cards = new List<Card>()
            {
                new Card(new UpgradeBurrowing(), new UpgradeBurrowing()),
                new Card(new UpgradeBurrowing(), new UpgradeBurrowing())
            };
            foreach (var upgrade in upgrades1)
                cards.Add(new Card(upgrade, new UpgradeBurrowing()));
            foreach (var upgrade in upgrades2)
                cards.Add(new Card(upgrade, new UpgradeBurrowing()));

            _game = GameFactory.Instance.GetTestGame(players, cards);

            var i = 0;
            player1.AddCard(cards[i++]);
            player1.AddAnimal(player1.Cards.First());
            player2.AddCard(cards[i++]);
            player2.AddAnimal(player2.Cards.First());

            for (var j = 0; j < upgrades1.Count; j++)
            {
                player1.AddCard(cards[i + j]);
                player1.AddUpgrade(player1.Animals.First(), player1.Cards.First(), player1.Cards.First().Upgrade1 as UpgradeSingle);
            }

            i += upgrades1.Count;
            for (var j = 0; j < upgrades2.Count; j++)
            {
                player2.AddCard(cards[i + j]);
                player2.AddUpgrade(player2.Animals.First(), player2.Cards.First(), player2.Cards.First().Upgrade1 as UpgradeSingle);
            }
            i += upgrades2.Count;

        }

        private void OnVictimChoose(object sender, VictimChooseEventArgs args)
        {
            if (AttackPossibilityChecker.Instance.CanAttack(args.Fight.Attacker, _victim))
            {
                args.Fight.Start(_victim);
            }

        }





    }
}