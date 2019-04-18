using System;

namespace Model.Upgrades
{
    public class UpgradeCarnivorous : UpgradeSingle, IUsable
    {
        public event StartAttackEventHandler OnUse;


        public UpgradeCarnivorous()
        {
            UpgradeType = UpgradeType.Carnivorous;
        }

        public override int AdditionalFoodNeeded => 1;

        public void Use()
        {
            OnUse?.Invoke(this, new StartAttackEventArgs(Animal));
        }

    }



    public delegate void StartAttackEventHandler(object sender, StartAttackEventArgs e);

    public class StartAttackEventArgs : EventArgs
    {
        public Animal Attacker { get; private set; }

        public StartAttackEventArgs(Animal attacker)
        {
            Attacker = attacker;
        }
    }



}