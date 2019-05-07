namespace Model.Upgrades
{
    public class UpgradePoisonous : UpgradeSingle
    {
        public UpgradePoisonous()
        {
            UpgradeType = UpgradeType.Poisonous;
        }

        public override int AdditionalFoodNeeded => 0;


    }
}