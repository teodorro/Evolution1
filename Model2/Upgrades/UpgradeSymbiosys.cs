namespace Model.Upgrades
{
    public class UpgradeSymbiosys : UpgradePair
    {
        public UpgradeSymbiosys()
        {
            UpgradeType = UpgradeType.Symbiosys;
        }

        public override int AdditionalFoodNeeded => 0;
    }
}