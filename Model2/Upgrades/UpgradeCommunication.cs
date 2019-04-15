namespace Model.Upgrades
{
    public class UpgradeCommunication : UpgradePair
    {
        public UpgradeCommunication()
        {
            UpgradeType = UpgradeType.Communication;
        }

        public override int AdditionalFoodNeeded => 0;
    }
}