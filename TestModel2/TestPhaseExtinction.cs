using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Upgrades;
using Xunit;

namespace TestModel
{
    public class TestPhaseExtinction
    {
        [Fact]
        public void TestNullArgs()
        {
            Assert.Throws<ArgumentNullException>(() => new PhaseExtinction(null));
        }

        [Fact]
        public void TestExtinctionSimpleDie()
        {
            IPlayer player1 = new Player("p1");
            var card = new Card(new UpgradeCarnivorous(), new UpgradeBurrowing());
            player1.AddCard(card);
            player1.AddAnimal(card);
            var phase = new PhaseExtinction(new List<IPlayer>(){player1});

            phase.StartExtinct();

            Assert.Empty(player1.Animals);
        }

        [Fact]
        public void TestExtinctionSimpleSurvive()
        {
            IPlayer player1 = new Player("p1");
            var card = new Card(new UpgradeCarnivorous(), new UpgradeBurrowing());
            player1.AddCard(card);
            player1.AddAnimal(card);
            player1.Animals.First().AddFood(new FoodToken(true), null);
            var phase = new PhaseExtinction(new List<IPlayer>() { player1 });

            phase.StartExtinct();

            Assert.Single(player1.Animals);
        }
    }
}