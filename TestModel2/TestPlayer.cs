using System;
using System.Linq;
using Model;
using Model.Upgrades;
using Xunit;
using Xunit.Sdk;

namespace TestModel
{
    public class TestPlayer
    {
        private Card GetCardCarnivorousNull() => new Card(new UpgradeCarnivorous(), null);
        


        private Player GetPlayer1(out Card c1, out Card c2, out Card c3)
        {
            var player = new Player();
            c1 = new Card(new UpgradeCarnivorous(), new UpgradePoisonous());
            c2 = new Card(new UpgradeFat(), new UpgradeBurrowing());
            c3 = new Card(new UpgradeFat(), new UpgradeScavenger());
            player.AddCard(c1);
            player.AddCard(c2);
            player.AddCard(c3);
            return player;
        }

        private Player GetPlayer2(out Card c1, out Card c2, out Card c3, out Card c4, out Card c5)
        {
            var player = new Player();
            c1 = new Card(new UpgradeCarnivorous(), new UpgradePoisonous());
            c2 = new Card(new UpgradeFat(), new UpgradeBurrowing());
            c3 = new Card(new UpgradeFat(), new UpgradeScavenger());
            c4 = new Card(new UpgradeCommunication(), new UpgradeScavenger());
            c5 = new Card(new UpgradeCommunication(), new UpgradeScavenger());
            player.AddCard(c1);
            player.AddCard(c2);
            player.AddCard(c3);
            player.AddCard(c4);
            player.AddCard(c5);
            return player;
        }


        [Fact]
        public void TestPlayerSomeCards()
        {
            var player = new Player();

            player.AddCard(GetCardCarnivorousNull());

            Assert.NotEmpty(player.Cards);
        }

        [Fact]
        public void TestPlayerNoCards()
        {
            var player = new Player();

            Assert.Throws<ArgumentNullException>(() => player.AddCard(null));
        }

        [Fact]
        public void TestAddCard()
        {
            var player = new Player();
            
            player.AddCard(GetCardCarnivorousNull());

            Assert.Single(player.Cards);
        }

        [Fact]
        public void TestAddCardNull()
        {
            var player = new Player();

            Assert.Throws<ArgumentNullException>(() => player.AddCard(null));
        }

        [Fact]
        public void TestAddAnimalNoCards()
        {
            var player = new Player();
            Assert.Throws<NoCardsException>(() => player.AddAnimal(GetCardCarnivorousNull()));
        }

        [Fact]
        public void TestAddParasiteSimple()
        {
            TwoPlayers(out var player1, out var player2);

            player1.AddParasite(player2, player2.Animals.First(), player1.Cards.First());

            Assert.Contains(player2.Animals.First().Upgrades, x => x.UpgradeType == UpgradeType.Parasite);
        }

        private void TwoPlayers(out Player player1, out Player player2)
        {
            player1 = new Player();
            player2 = new Player();
            player1.AddCard(new Card(new UpgradeCarnivorous(), new UpgradeParasite()));
            player1.AddCard(new Card(new UpgradeCarnivorous(), new UpgradeParasite()));
            player2.AddCard(new Card(new UpgradeCarnivorous(), new UpgradeParasite()));
            player1.AddAnimal(player1.Cards.First());
            player2.AddAnimal(player2.Cards.First());
        }

        [Fact]
        public void TestAddParasiteOwnAnimal()
        {
            TwoPlayers(out var player1, out var player2);

            Assert.Throws<NoParasiteToYourAnimalsException>(() =>
                player1.AddParasite(player1, player1.Animals.First(), player1.Cards.First()));
        }

        [Fact]
        public void TestAddTwoParasites()
        {
            TwoPlayers(out var player1, out var player2);
            player1.AddCard(new Card(new UpgradeCarnivorous(), new UpgradeParasite()));

            player1.AddParasite(player2, player2.Animals.First(), player1.Cards.First());
            player1.AddParasite(player2, player2.Animals.First(), player1.Cards.First());

            Assert.Equal(2, player2.Animals.First().Upgrades.Count(x => x.UpgradeType == UpgradeType.Parasite));
        }

        [Fact]
        public void TestAddParasiteWrongCard()
        {
            TwoPlayers(out var player1, out var player2);
            player1.AddCard(new Card(new UpgradeCarnivorous(), new UpgradeBurrowing()));

            Assert.Throws<CardUpgradeIncostintenceException>(() => 
                player1.AddParasite(player2, player2.Animals.First(), player1.Cards.First(x => x.Upgrade1.UpgradeType != UpgradeType.Parasite && x.Upgrade2.UpgradeType != UpgradeType.Parasite)));
        }

    }
}