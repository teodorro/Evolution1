﻿namespace Model.Upgrades
{
    public class UpgradePiracy : UpgradeSingle
    {
        public UpgradePiracy()
        {
            UpgradeType = UpgradeType.Piracy;
        }

        public override int AdditionalFoodNeeded => 0;

        public bool UsedThisTurn { get; set; }
    }
}