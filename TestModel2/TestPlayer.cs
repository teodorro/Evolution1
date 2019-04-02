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
            c3 = new Card(new UpgradeFat(), new UpgradeScavanger());
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
            c3 = new Card(new UpgradeFat(), new UpgradeScavanger());
            c4 = new Card(new UpgradeCommunication(), new UpgradeScavanger());
            c5 = new Card(new UpgradeCommunication(), new UpgradeScavanger());
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

//        [Fact]
//        public void TestAddAnimal()
//        {
//            var player = new Player();
//            player.AddCard(GetCardCarnivorousNull());
//
//            player.AddAnimal(player.Cards.First());
//
//            Assert.Single(player.Animals);
//            Assert.Empty(player.Cards);
//        }
//
//        [Fact]
//        public void TestRemoveAnimal()
//        {
//            var player = new Player();
//            player.AddCard(GetCardCarnivorousNull());
//            player.AddAnimal(player.Cards.First());
//
//            player.RemoveAnimal(player.Animals.First());
//
//            Assert.Empty(player.Animals);
//        }
//
//        [Fact]
//        public void TestRemoveWrongAnimal()
//        {
//            var player = new Player();
//            player.AddCard(GetCardCarnivorousNull());
//            player.AddAnimal(player.Cards.First());
//
//            Assert.Throws<PlayerAnimalIncostintenceException>(() => player.RemoveAnimal(new Animal()));
//            Assert.Throws<PlayerAnimalIncostintenceException>(() => player.RemoveAnimal(null));
//        }
//
//        [Fact]
//        public void TestAddUpgrade()
//        {
//            var player = new Player();
//            player.AddCard(GetCardCarnivorousNull());
//            player.AddCard(GetCardCarnivorousNull());
//            player.AddAnimal(player.Cards.First());
//
//            var animal = player.Animals.First();
//            var first = player.Cards.First();
//            player.AddUpgrade(animal, first, first.Upgrade1);
//
//            Assert.Contains(first.Upgrade1, player.Animals.First().Upgrades);
//        }
//
//        [Fact]
//        public void TestAddUpgradeNull()
//        {
//            var player = new Player();
//            player.AddCard(GetCardCarnivorousNull());
//            player.AddCard(GetCardCarnivorousNull());
//            player.AddAnimal(player.Cards.First());
//
//            var animal = player.Animals.First();
//            var first = player.Cards.First();
//
//            Assert.Throws<ArgumentNullException>(() => player.AddUpgrade(animal, first, null));
//            Assert.Throws<ArgumentNullException>(() => player.AddUpgrade(animal, null, first.Upgrade1));
//            Assert.Throws<ArgumentNullException>(() => player.AddUpgrade(null, first, first.Upgrade1));
//        }
//
//        [Fact]
//        public void TestAddUpgradeWrongUpgr()
//        {
//            var player = new Player();
//            player.AddCard(GetCardCarnivorousNull());
//            player.AddCard(GetCardCarnivorousNull());
//            player.AddAnimal(player.Cards.First());
//
//            var animal = player.Animals.First();
//            var first = player.Cards.First();
//
//            Assert.Throws<CardUpgradeIncostintenceException>(() => player.AddUpgrade(animal, first, new UpgradeBurrowing()));
//        }
//
//        [Fact]
//        public void TestAddUpgradeUpgrDup()
//        {
//            var player = new Player();
//            player.AddCard(GetCardCarnivorousNull());
//            player.AddCard(GetCardCarnivorousNull());
//            player.AddCard(GetCardCarnivorousNull());
//
//            var animal = player.AddAnimal(player.Cards.First());
//            var first = player.Cards.First();
//            
//            player.AddUpgrade(animal, first, first.Upgrade1);
//            Assert.Throws<AnimalUpgradeIncostintenceException>(() => player.AddUpgrade(animal, first, first.Upgrade1));
//        }




        [Fact]
        public void TestCanBeUpgradedSimple()
        {
            var player = GetPlayer1(out var c1, out var c2, out var c3);

            var animal = player.AddAnimal(c1);
            player.AddUpgrade(animal, c2, c2.Upgrade2);

            Assert.True(player.CanBeUpgraded(animal, c3.Upgrade1 as UpgradeSingle));
        }

        [Fact]
        public void TestCanBeUpgradedFatTwice()
        {
            var player = GetPlayer1(out var c1, out var c2, out var c3);

            var animal = player.AddAnimal(c1);
            player.AddUpgrade(animal, c2, c2.Upgrade1);

            Assert.True(player.CanBeUpgraded(animal, c3.Upgrade1 as UpgradeSingle));
        }

        [Fact]
        public void TestCanBeUpgradedParasite()
        {
            var player = GetPlayer1(out var c1, out var c2, out var c3);

            var animal = player.AddAnimal(c1);
            player.AddUpgrade(animal, c2, c2.Upgrade1);

            Assert.False(player.CanBeUpgraded(animal, c3.Upgrade1 as UpgradeSingle));
        }

        [Fact]
        public void TestCanBeUpgradedScavengerFalse()
        {
            var player = GetPlayer1(out var c1, out var c2, out var c3);

            var animal = player.AddAnimal(c2);
            player.AddUpgrade(animal, c1, c1.Upgrade1);

            Assert.False(player.CanBeUpgraded(animal, c3.Upgrade2 as UpgradeSingle));
        }

        [Fact]
        public void TestCanBeUpgradedCarnivorousFalse()
        {
            var player = GetPlayer1(out var c1, out var c2, out var c3);

            var animal = player.AddAnimal(c2);
            player.AddUpgrade(animal, c3, c3.Upgrade2);

            Assert.False(player.CanBeUpgraded(animal, c1.Upgrade1 as UpgradeSingle));
        }

//        [Fact]
//        public void TestCanBeUpgradedPairTrue()
//        {
//            var player = GetPlayer2(out var c1, out var c2, out var c3, out var c4, out var c5);
//
//            var animal1 = player.AddAnimal(c1);
//            var animal2 = player.AddAnimal(c1);
//            player.AddUpgrade(animal, c3, c3.Upgrade2);
//
//            Assert.False(player.CanBeUpgraded(animal, c1.Upgrade1));
//        }
//
//        [Fact]
//        public void TestCanBeUpgradedPairDup()
//        {
//
//        }
//
//        [Fact]
//        public void TestCanBeUpgradedPairDistance()
//        {
//
//        }
    }
}