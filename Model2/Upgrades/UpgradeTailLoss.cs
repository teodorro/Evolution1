using System;

namespace Model.Upgrades
{
    public class UpgradeTailLoss : UpgradeSingle
    {
        public event TailLossEventHandler OnUse;


        public UpgradeTailLoss()
        {
            UpgradeType = UpgradeType.TailLoss;
        }

        public override int AdditionalFoodNeeded => 0;


        public void Use()
        {
            OnUse?.Invoke(this, new TailLossEventArgs(Animal));
        }
    }



    public delegate void TailLossEventHandler(object sender, TailLossEventArgs e);

    public class TailLossEventArgs : EventArgs
    {
        public Animal Victim { get; private set; }

        public TailLossEventArgs(Animal victim)
        {
            Victim = victim;
        }
    }
}