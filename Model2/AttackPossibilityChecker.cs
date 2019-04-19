using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Model.Upgrades;

namespace Model
{

    public interface IAttackPossibilityChecker
    {
        bool CanAttack(Animal attacker, Animal victim);
    }



    public class AttackPossibilityChecker : IAttackPossibilityChecker
    {
        private static readonly Lazy<AttackPossibilityChecker> _instance = new Lazy<AttackPossibilityChecker>(() => new AttackPossibilityChecker());
        public static AttackPossibilityChecker Instance => _instance.Value;


        protected AttackPossibilityChecker()
        {
            
        }

        public bool CanAttack(Animal attacker, Animal victim)
        {
            var swimming = CheckSwimming(attacker, victim);
            var burrowing = CheckBorrowing(attacker, victim);
            var camouflage = CheckCamouflage(attacker, victim);
            var heavyweight = CheckHeavyweight(attacker, victim);
            var symbiosys = CheckSymbiosys(attacker, victim);

            return swimming
                && burrowing
                && camouflage
                && heavyweight
                && symbiosys;
        }

        private bool CheckSymbiosys(Animal attacker, Animal victim)
        {
            var symboExists = victim.Upgrades.Any(x => x.UpgradeType == UpgradeType.Symbiosys);
            if (symboExists)
                return (victim.Upgrades.First(x => x.UpgradeType == UpgradeType.Symbiosys) as UpgradeSymbiosys)
                       .LeftAnimal == victim;
            return true;
        }

        private bool CheckHeavyweight(Animal attacker, Animal victim)
        {
            var aHeavy = attacker.Upgrades.Any(x => x.UpgradeType == UpgradeType.HighBodyWeight);
            var vNotHeavy = victim.Upgrades.All(x => x.UpgradeType != UpgradeType.HighBodyWeight);

            return aHeavy || vNotHeavy;
        }


        private bool CheckCamouflage(Animal attacker, Animal victim)
        {
            var aSharp = attacker.Upgrades.Any(x => x.UpgradeType == UpgradeType.SharpVision);
            var vNotCamouflage = victim.Upgrades.All(x => x.UpgradeType != UpgradeType.Camouflage);

            return aSharp || vNotCamouflage;
        }

        private bool CheckBorrowing(Animal attacker, Animal victim)
        {
            var vBorrowing = victim.Upgrades.Any(x => x.UpgradeType == UpgradeType.Burrowing);
            var fed = vBorrowing && victim.FoodGot == victim.FoodNeeded;

            return !fed;
        }

        private bool CheckSwimming(Animal attacker, Animal victim)
        {
            var bothSwim = attacker.Upgrades.Any(x => x.UpgradeType == UpgradeType.Swimming) &
                           victim.Upgrades.Any(x => x.UpgradeType == UpgradeType.Swimming);
            var bothNotSwim = attacker.Upgrades.All(x => x.UpgradeType != UpgradeType.Swimming) &
                              victim.Upgrades.All(x => x.UpgradeType != UpgradeType.Swimming);

            return bothSwim || bothNotSwim;
        }
    }
}