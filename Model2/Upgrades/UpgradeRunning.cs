using System;

namespace Model.Upgrades
{
    public class UpgradeRunning : UpgradeSingle
    {
        public UpgradeRunning()
        {
            UpgradeType = UpgradeType.Running;
        }

        public override int AdditionalFoodNeeded => 0;

        public int Use()
        {
            return new Random(DateTime.Now.Millisecond).Next(1, 6);
        }
    }
}