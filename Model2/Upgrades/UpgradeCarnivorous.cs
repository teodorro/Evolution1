using System;

namespace Model.Upgrades
{
    public class UpgradeCarnivorous : UpgradeSingle, IUsable
    {
        public event StartAttackEventHandler OnUse;

        public bool WasUsedThisTurn { get; set; }


        public UpgradeCarnivorous()
        {
            UpgradeType = UpgradeType.Carnivorous;
        }

        public override int AdditionalFoodNeeded => 1;

        public void Use()
        {
            if (WasUsedThisTurn)
                throw new UpgradeWasAlreadyUsedThisTurnException();
            OnUse?.Invoke(this, new StartAttackEventArgs(Animal));
            WasUsedThisTurn = true;
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