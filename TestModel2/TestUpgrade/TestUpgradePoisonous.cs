using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Upgrades;
using Xunit;

namespace TestModel.TestUpgrade
{
    public class TestUpgradePoisonous
    {
        private IAnimal _victim;

        private void OnVictimChoose(object sender, VictimChooseEventArgs args)
        {
            if (AttackPossibilityChecker.Instance.CanAttack(args.Fight.Attacker, _victim))
                args.Fight.Start(_victim);
        }

        [Fact]
        public void TestPoisonousSimple()
        {
            GameConstructor.Instance.ArrangeTwoAnimals(
                out var game,
                out var player1,
                new List<UpgradeSingle>() { new UpgradeCarnivorous() },
                out var player2,
                new List<UpgradeSingle>() { new UpgradePoisonous() });
            _victim = player2.Animals.First();
            game.VictimChoose += OnVictimChoose;

            (player1.Animals.First().Upgrades.First(x => x.UpgradeType == UpgradeType.Carnivorous) as UpgradeCarnivorous).Use();

            var phaseEx = new PhaseExtinction(new List<IPlayer>(){player1, player2});
            phaseEx.StartExtinct();

            Assert.Empty(player1.Animals);
        }

        [Fact]
        public void TestPoisonousNotDie()
        {
            GameConstructor.Instance.ArrangeTwoAnimals(
                out var game,
                out var player1,
                new List<UpgradeSingle>() ,
                out var player2,
                new List<UpgradeSingle>() { new UpgradePoisonous() });
            player1.TryFeedAnimalEvent += (sender, args) => args.Player.Feed(args.Animal);
            player2.TryFeedAnimalEvent += (sender, args) => args.Player.Feed(args.Animal);

            player1.TryFeed(player1.Animals.First());
            player2.TryFeed(player2.Animals.First());

            var phaseEx = new PhaseExtinction(new List<IPlayer>() { player1, player2 });
            phaseEx.StartExtinct();

            Assert.Single(player1.Animals);
            Assert.Single(player1.Animals);
        }
    }
}