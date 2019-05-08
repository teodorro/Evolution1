using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Upgrades;
using Xunit;

namespace TestModel.TestUpgrade
{
    public class TestUpgradeFat
    {
        [Fact]
        public void TestFillFat()
        {
            var player1 = new Player("player1");
            player1.Animals.AddAnimal();
            var animal = player1.Animals.First();
            animal.AddUpgrade(new UpgradeFat());
            player1.TryFeedAnimalEvent += (sender, args) => args.Player.Feed(args.Animal);

            player1.TryFeed(player1.Animals.First());
            player1.TryFeed(player1.Animals.First());

            Assert.True(((UpgradeFat)animal.Upgrades.First()).Full);
        }

        [Fact]
        public void TestNoFreeFat()
        {
            var player1 = new Player("player1");
            player1.Animals.AddAnimal();
            var animal = player1.Animals.First();
            animal.AddUpgrade(new UpgradeFat());
            player1.TryFeedAnimalEvent += (sender, args) => args.Player.Feed(args.Animal);

            player1.TryFeed(player1.Animals.First());
            player1.TryFeed(player1.Animals.First());
            
            Assert.Throws<AnimalAlreadyFedException>(() => player1.TryFeed(player1.Animals.First()));
        }

        [Fact]
        public void TestUseFat()
        {
            var player1 = new Player("player1");
            player1.Animals.AddAnimal();
            var animal = player1.Animals.First();
            animal.AddUpgrade(new UpgradeFat(){Full = true});
            var phaseEx = new PhaseExtinction(new List<IPlayer>(){player1});
            
            phaseEx.StartExtinct();

            Assert.Single(player1.Animals);
        }

        [Fact]
        public void TestTryUseEmptyFat()
        {

        }
    }
}