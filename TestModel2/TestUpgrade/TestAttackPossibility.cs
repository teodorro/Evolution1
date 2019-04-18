using Model;
using Model.Upgrades;
using Xunit;

namespace TestModel.TestUpgrade
{
    public class TestAttackPossibility
    {
        private void CreateAttackerAndVictim(out Animal attacker, out Animal victim)
        {
            attacker = new Animal(new Player("player1"));
            victim = new Animal(new Player("player2"));
            attacker.AddUpgrade(new UpgradeCarnivorous());
        }

        [Fact]
        public void TestAttackPossibleSimple()
        {
            CreateAttackerAndVictim(out var attacker, out var victim);

            var check = AttackPossibilityChecker.Instance.CheckCanAttack(attacker, victim);

            Assert.True(check);
        }

        [Fact]
        public void TestAttackPossibleSwimming1()
        {
            CreateAttackerAndVictim(out var attacker, out var victim);
            victim.AddUpgrade(new UpgradeSwimming());

            var check = AttackPossibilityChecker.Instance.CheckCanAttack(attacker, victim);

            Assert.False(check);
        }

        [Fact]
        public void TestAttackPossibleSwimming2()
        {
            CreateAttackerAndVictim(out var attacker, out var victim);
            victim.AddUpgrade(new UpgradeSwimming());
            attacker.AddUpgrade(new UpgradeSwimming());

            var check = AttackPossibilityChecker.Instance.CheckCanAttack(attacker, victim);

            Assert.True(check);
        }
    }
}