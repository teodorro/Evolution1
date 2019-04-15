namespace Model.Upgrades
{
    public class UpgradePoisonous : UpgradeSingle
    {
        public UpgradePoisonous()
        {
            UpgradeType = UpgradeType.Parasite;
        }

        public override int AdditionalFoodNeeded => 0;
    }
}