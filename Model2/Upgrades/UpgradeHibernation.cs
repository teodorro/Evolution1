namespace Model.Upgrades
{
    public class UpgradeHibernation : UpgradeSingle
    {
        public UpgradeHibernation()
        {
            UpgradeType = UpgradeType.Hibernation;
        }

        public override int AdditionalFoodNeeded => 0;

        public int LastTurnUsed { get; set; } = -1;
    }
}