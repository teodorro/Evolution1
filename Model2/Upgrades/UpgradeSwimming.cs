namespace Model.Upgrades
{
    public class UpgradeSwimming : UpgradeSingle
    {
        public UpgradeSwimming()
        {
            UpgradeType = UpgradeType.Swimming;
        }

        public override int AdditionalFoodNeeded => 0;
    }
}