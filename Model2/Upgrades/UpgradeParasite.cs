﻿namespace Model.Upgrades
{
    public class UpgradeParasite : UpgradeSingle
    {
        public UpgradeParasite()
        {
            UpgradeType = UpgradeType.Parasite;
            CanBeAppliedOtherPlayers = true;
            CanBeAppliedThePlayer = false;
        }

        public override int AdditionalFoodNeeded => 2;
    }
}