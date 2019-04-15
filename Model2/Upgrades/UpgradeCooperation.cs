namespace Model.Upgrades
{
    public class UpgradeCooperation : UpgradePair
    {
        public UpgradeCooperation()
        {
            UpgradeType = UpgradeType.Cooperation;
        }

        public override int AdditionalFoodNeeded => 0;
    }
}