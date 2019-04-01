namespace Model.Upgrades
{
    public class UpgradeFat : UpgradeSingle
    {
        public UpgradeFat()
        {
            UpgradeType = UpgradeType.Fat;
            AppliedInSingleCopy = false;
        }
    }
}