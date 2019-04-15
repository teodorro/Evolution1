namespace Model.Upgrades
{
    public class UpgradeMimicry : UpgradeSingle
    {
        public UpgradeMimicry()
        {
            UpgradeType = UpgradeType.Mimicry;
        }

        public override int AdditionalFoodNeeded => 0;
    }
}