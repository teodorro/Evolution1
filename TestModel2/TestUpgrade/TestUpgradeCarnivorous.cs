using System;
using System.Collections.Generic;
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

            
            // one attack another
            // check attacker can attack
            // check victim can defend
            // Use defence cards
            // send result

            // create two players
            var player1 = new Player("player1");
            var player2 = new Player("player2");
            var players = new List<IPlayer>() { player1, player2 };
            var cards = new List<Card>()
            {
                new Card(new UpgradeBurrowing(), new UpgradeCarnivorous()),
                new Card(new UpgradeBurrowing(), new UpgradeCarnivorous()),
                new Card(new UpgradeBurrowing(), new UpgradeCommunication())
            };
            _game = GameFactory.Instance.GetTestGame(players, cards);


            // animals for players
            player1.AddCard(cards[0]);
            player1.AddCard(cards[1]);
            player2.AddCard(cards[2]);
            player1.AddAnimal(player1.Cards.First());
            player1.AddUpgrade(player1.Animals.First(), player1.Cards.First(), (UpgradeSingle)(
                player1.Cards.First().Upgrade1.UpgradeType == UpgradeType.Carnivorous
                    ? player1.Cards.First().Upgrade1
                    : player1.Cards.First().Upgrade2));
            player2.AddAnimal(player2.Cards.First());

            _victim = player2.Animals.First();
            _game.VictimChoose += OnVictimChoose;

            (player1.Animals.First().Upgrades.First(x => x.UpgradeType == UpgradeType.Carnivorous) as UpgradeCarnivorous).Use();

            Assert.Empty(player2.Animals);
            Assert.Equal(2, player1.Animals.First().FoodGot);

        }

        private void OnVictimChoose(object sender, VictimChooseEventArgs args)
        {
            if (AttackPossibilityChecker.Instance.CheckCanAttack(args.Fight.Attacker, _victim))
            {
                args.Fight.Start(_victim);
            }

        }





    }
}