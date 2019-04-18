using System;
using System.Collections.Generic;
using System.Linq;
using Model.Upgrades;

namespace Model
{

    public interface IAttackPossibilityChecker
    {
        bool CheckCanAttack(Animal attacker, Animal victim);
    }



    public class AttackPossibilityChecker : IAttackPossibilityChecker
    {
        private static readonly Lazy<AttackPossibilityChecker> _instance = new Lazy<AttackPossibilityChecker>(() => new AttackPossibilityChecker());
        public static AttackPossibilityChecker Instance => _instance.Value;


        protected AttackPossibilityChecker()
        {
            
        }

        public bool CheckCanAttack(Animal attacker, Animal victim)
        {
            var swimming = CheckSwimming(attacker, victim);

            return swimming;
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