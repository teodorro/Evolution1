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

            var check = AttackPossibilityChecker.Instance.CanAttack(attacker, victim);

            Assert.True(check);
        }

        [Fact]
        public void TestAttackPossibleSwimming1()
        {
            CreateAttackerAndVictim(out var attacker, out var victim);
            victim.AddUpgrade(new UpgradeSwimming());

            var check = AttackPossibilityChecker.Instance.CanAttack(attacker, victim);

            Assert.False(check);
        }

        [Fact]
        public void TestAttackPossibleSwimming2()
        {
            CreateAttackerAndVictim(out var attacker, out var victim);
            victim.AddUpgrade(new UpgradeSwimming());
            attacker.AddUpgrade(new UpgradeSwimming());

            var check = AttackPossibilityChecker.Instance.CanAttack(attacker, victim);

            Assert.True(check);
        }

        [Fact]
        public void TestAttackPossibleSymbiosys()
        {
            var attacker = new Animal(new Player("player1"));
            attacker.AddUpgrade(new UpgradeCarnivorous());

            var animals = new AnimalCollection(new Player("player2"));
            var a1 = animals.AddAnimal();
            var a2 = animals.AddAnimal();
            animals.AddUpgrade(a1, new UpgradeSymbiosys());

            var check1 = AttackPossibilityChecker.Instance.CanAttack(attacker, a1);
            var check2 = AttackPossibilityChecker.Instance.CanAttack(attacker, a2);

            Assert.True(check1);
            Assert.False(check2);

        }

        [Fact]
        public void TestAttackPossibleHeavyWeight1()
        {
            CreateAttackerAndVictim(out var attacker, out var victim);
            victim.AddUpgrade(new UpgradeHighBodyWeight());

            var check = AttackPossibilityChecker.Instance.CanAttack(attacker, victim);

            Assert.False(check);
        }

        [Fact]
        public void TestAttackPossibleHeavyWeight2()
        {
            CreateAttackerAndVictim(out var attacker, out var victim);
            victim.AddUpgrade(new UpgradeHighBodyWeight());

            var check = AttackPossibilityChecker.Instance.CanAttack(attacker, victim);

            Assert.False(check);
        }

        [Fact]
        public void TestAttackPossibilityCamouflage1()
        {
            CreateAttackerAndVictim(out var attacker, out var victim);
            victim.AddUpgrade(new UpgradeCamouflage());

            var check = AttackPossibilityChecker.Instance.CanAttack(attacker, victim);

            Assert.False(check);
        }

        [Fact]
        public void TestAttackPossibilityCamouflage2()
        {
            CreateAttackerAndVictim(out var attacker, out var victim);
            victim.AddUpgrade(new UpgradeCamouflage());
            attacker.AddUpgrade(new UpgradeSharpVision());

            var check = AttackPossibilityChecker.Instance.CanAttack(attacker, victim);

            Assert.True(check);
        }
        
        [Fact]
        public void TestAttackPossibilityBurrowing1()
        {
            CreateAttackerAndVictim(out var attacker, out var victim);
            victim.AddUpgrade(new UpgradeBurrowing());

            var check = AttackPossibilityChecker.Instance.CanAttack(attacker, victim);

            Assert.True(check);
        }
        
        [Fact]
        public void TestAttackPossibilityBurrowing2()
        {
            CreateAttackerAndVictim(out var attacker, out var victim);
            victim.AddUpgrade(new UpgradeBurrowing());
            victim.AddFood(1);

            var check = AttackPossibilityChecker.Instance.CanAttack(attacker, victim);

            Assert.False(check);
        }
    }
}