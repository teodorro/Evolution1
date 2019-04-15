namespace Model.Upgrades
{
    public class UpgradeRunning : UpgradeSingle
    {
        public UpgradeRunning()
        {
            UpgradeType = UpgradeType.Running;
        }

        public override int AdditionalFoodNeeded => 0;
    }
}