namespace Model.Upgrades
{
    public class UpgradeParasite : UpgradeSingle
    {
        public UpgradeParasite()
        {
            UpgradeType = UpgradeType.Parasite;
            CanBeAppliedOtherPlayers = true;
            CanBeAppliedThePlayer = false;
        }
    }
}