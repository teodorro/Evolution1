namespace Model.Upgrades
{
    public class UpgradeCamouflage : UpgradeSingle
    {
        public UpgradeCamouflage()
        {
            UpgradeType = UpgradeType.Camouflage;
        }

        public override int AdditionalFoodNeeded => 0;
    }
}