namespace Model.Upgrades
{
    public class UpgradeTailLoss : UpgradeSingle
    {
        public UpgradeTailLoss()
        {
            UpgradeType = UpgradeType.TailLoss;
        }

        public override int AdditionalFoodNeeded => 0;
    }
}