namespace Model.Upgrades
{
    public class UpgradeScavenger : UpgradeSingle
    {
        public UpgradeScavenger()
        {
            UpgradeType = UpgradeType.Scavanger;
        }

        public override int AdditionalFoodNeeded => 0;
    }
}