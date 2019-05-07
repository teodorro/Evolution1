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
        private IAnimal _victim;
        
        private void OnVictimChoose(object sender, VictimChooseEventArgs args)
        {
            if (AttackPossibilityChecker.Instance.CanAttack(args.Fight.Attacker, _victim))
                args.Fight.Start(_victim);
        }


        [Fact]
        public void TestCarnivorousSimple()
        {
            GameConstructor.Instance.ArrangeTwoAnimals(
                out var game,
                out var player1,
                new List<UpgradeSingle>() { new UpgradeCarnivorous() },
                out var player2,
                new List<UpgradeSingle>());
            _victim = player2.Animals.First();
            game.VictimChoose += OnVictimChoose;

            (player1.Animals.First().Upgrades.First(x => x.UpgradeType == UpgradeType.Carnivorous) as UpgradeCarnivorous).Use();

            Assert.Empty(player2.Animals);
            Assert.Equal(2, player1.Animals.First().FoodGot);
        }

        [Fact]
        public void TestCarnivorousTwice()
        {
            GameConstructor.Instance.ArrangeThreeAnimals(
                out var game,
                out var player1,
                new List<UpgradeSingle>() { new UpgradeCarnivorous(), new UpgradeHighBodyWeight() },
                out var player2,
                new List<UpgradeSingle>(),
                new List<UpgradeSingle>(),
                new List<UpgradePair>());
            _victim = player2.Animals.First();
            game.VictimChoose += OnVictimChoose;

            (player1.Animals.First().Upgrades.First(x => x.UpgradeType == UpgradeType.Carnivorous) as UpgradeCarnivorous).Use();

            _victim = player2.Animals.First();
            (player1.Animals.First().Upgrades.First(x => x.UpgradeType == UpgradeType.Carnivorous) as UpgradeCarnivorous).Use();

            Assert.Empty(player2.Animals);
            Assert.Equal(2, player1.Animals.First().FoodGot);

        }



    }
}