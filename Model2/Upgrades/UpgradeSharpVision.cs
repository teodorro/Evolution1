namespace Model.Upgrades
{
    public class UpgradeSharpVision : UpgradeSingle
    {
        public UpgradeSharpVision()
        {
            UpgradeType = UpgradeType.SharpVision;
        }

        public override int AdditionalFoodNeeded => 0;
    }
}