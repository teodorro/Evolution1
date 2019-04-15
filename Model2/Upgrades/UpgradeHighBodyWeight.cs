namespace Model.Upgrades
{
    public class UpgradeHighBodyWeight : UpgradeSingle
    {
        public UpgradeHighBodyWeight()
        {
            UpgradeType = UpgradeType.HighBodyWeight;
        }

        public override int AdditionalFoodNeeded => 1;
    }
}