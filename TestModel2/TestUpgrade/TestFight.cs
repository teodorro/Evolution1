using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Model;
using Model.Upgrades;
using Xunit;

namespace TestModel.TestUpgrade
{
    public class TestFight
    {
        private Animal _victim;
        private Game _game;




        [Fact]
        public void TestFightSimple()
        {
            GameConstructor.Instance.ArrangeTwoAnimals(
                out var game,
                out var player1, 
                new List<UpgradeSingle>() { new UpgradeCarnivorous() }, 
                out var player2, 
                new List<UpgradeSingle>());
            var fight = new Fight(player1.Animals.First(), new List<IPlayer>() {player1, player2});

            fight.FightOver += (sender, args) => { Assert.True(args.Killed); };

            fight.Start(player2.Animals.First());
        }

        [Fact]
        public void TestFightDefenseIsCalled()
        {
            GameConstructor.Instance.ArrangeTwoAnimals(
                out var game,
                out var player1,
                new List<UpgradeSingle>() { new UpgradeCarnivorous() },
                out var player2,
                new List<UpgradeSingle>() {new UpgradeTailLoss()});
            var fight = new Fight(player1.Animals.First(), new List<IPlayer>() { player1, player2 });
            var a = false;
            fight.DefenceChoose += (sender, args) => { a = true; };

            fight.Start(player2.Animals.First());

            Assert.True(a);
        }

//        [Fact]
//        public void TestPairCardsDisappear()
//        {
//            ArrangeThreeAnimals(
//                out var player1,
//                new List<UpgradeSingle>() { new UpgradeCarnivorous() },
//                out var player2,
//                new List<UpgradeSingle>(),
//                new List<UpgradeSingle>(), 
//                new List<UpgradePair>(){new UpgradeCommunication()});
//            var fight = new Fight(player1.Animals.First(), new List<IPlayer>() { player1, player2 });
//
//            fight.FightOver += (sender, args) =>
//            {
//                Assert.True(player2.Animals.First().Upgrades.All(x => !x.GetType().IsSubclassOf(typeof(UpgradePair))));
//                Assert.True(player2.Animals.Last().Upgrades.All(x => !x.GetType().IsSubclassOf(typeof(UpgradePair))));
//            };
//
//            fight.Start(player2.Animals.First());
//
//        }


    }
}