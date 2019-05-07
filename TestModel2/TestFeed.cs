using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Upgrades;
using Xunit;

namespace TestModel
{
    public class TestFeed
    {
        [Fact]
        public void TestFeedSimple()
        {
            GameConstructor.Instance.ArrangeTwoAnimals(
                out var game,
                out var player1,
                new List<UpgradeSingle>(),
                out var player2,
                new List<UpgradeSingle>());
            player1.TryFeedAnimalEvent += (sender, args) => args.Player.Feed(args.Animal);

            player1.TryFeed(player1.Animals.First());

            Assert.Equal(1, player1.Animals.First().FoodGot);
        }

        [Fact]
        public void TestFeedWrongAnimal()
        {
            GameConstructor.Instance.ArrangeTwoAnimals(
                out var game,
                out var player1,
                new List<UpgradeSingle>(),
                out var player2,
                new List<UpgradeSingle>());

            Assert.Throws<AnimalNotFoundException>(() => player1.TryFeed(player2.Animals.First()));
        }

        [Fact]
        public void TestFeedNullAnimal()
        {
            GameConstructor.Instance.ArrangeTwoAnimals(
                out var game,
                out var player1,
                new List<UpgradeSingle>(),
                out var player2,
                new List<UpgradeSingle>());

            Assert.Throws<ArgumentNullException>(() => player1.TryFeed(null));
        }
    }
}