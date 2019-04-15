namespace Model.Upgrades
{
    public class UpgradeCarnivorous : UpgradeSingle
    {
        public UpgradeCarnivorous()
        {
            UpgradeType = UpgradeType.Carnivorous;
        }

        public override int AdditionalFoodNeeded => 1;
    }
}