namespace Model.Upgrades
{
    public class UpgradeBurrowing : UpgradeSingle
    {
        public UpgradeBurrowing()
        {
            UpgradeType = UpgradeType.Burrowing;
        }

        public override int AdditionalFoodNeeded => 0;
    }
}