namespace Model.Upgrades
{
    public class UpgradeGrazing : UpgradeSingle
    {
        public UpgradeGrazing()
        {
            UpgradeType = UpgradeType.Grazing;
        }

        public override int AdditionalFoodNeeded => 0;
    }
}