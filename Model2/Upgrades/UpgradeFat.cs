namespace Model.Upgrades
{
    public class UpgradeFat : UpgradeSingle
    {
        public UpgradeFat()
        {
            UpgradeType = UpgradeType.Fat;
            AppliedInSingleCopy = false;
        }

        public override int AdditionalFoodNeeded => 0;

        public bool Full { get; set; } = false;
    }
}