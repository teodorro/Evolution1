using System;
using System.Collections.Generic;
using System.Linq;
using Model.Upgrades;

namespace Model
{
    public class Fight
    {
        private Animal _attacker;
        private Animal _victim;
        private Dictionary<Upgrade, bool> DefenceCardsUsed = new Dictionary<Upgrade, bool>();
        private List<IPlayer> _players;

        public Animal Victim => _victim;
        public Animal Attacker => _attacker;
        public event DefenceChooseHandler DefenceChoose;
        public event FightOverHandler FightOver;


        public Fight(Animal attacker, List<IPlayer> players)
        {
            _attacker = attacker;
            _players = players;
        }


        public void Start(Animal victim)
        {
            _victim = victim;
            var defence = _victim.Upgrades.Where(x =>
                x.UpgradeType == UpgradeType.Mimicry || x.UpgradeType == UpgradeType.Running ||
                x.UpgradeType == UpgradeType.TailLoss).ToList();

            if (defence.Count > 0)
            {
                DefenceCardsUsed = defence.ToDictionary(item => item, item => false);
                DefenceChoose?.Invoke(this, new DefenceChooseEventArgs(this));
            }
            else
            {
                FightOver?.Invoke(this, new FightOverEventArgs(this, true));
            }
        }
        



        private void TestOnAttackChoose(object sender, DefenceChooseEventArgs args)
        {
            var canUse = args.Fight.DefenceCardsUsed.Where(x => !x.Value);
            var toUse = canUse.First().Key;

            switch (toUse.UpgradeType)
            {
                case UpgradeType.TailLoss:
                    (toUse as UpgradeTailLoss).Use();
                    break;
            }
        }

    }


    public delegate void VictimChooseEventHandler(object sender, VictimChooseEventArgs e);

    public class VictimChooseEventArgs : EventArgs
    {
        public Fight Fight { get; set; }

        public VictimChooseEventArgs(Fight fight)
        {
            Fight = fight;
        }
    }


    public delegate void DefenceChooseHandler(object sender, DefenceChooseEventArgs e);

    public class DefenceChooseEventArgs : EventArgs
    {
        public Fight Fight { get; private set; }

        public DefenceChooseEventArgs(Fight fight)
        {
            Fight = fight;
        }
    }


    public delegate void FightOverHandler(object sender, FightOverEventArgs e);

    public class FightOverEventArgs : EventArgs
    {
        public Fight Fight { get; private set; }
        public bool Killed { get; private set; }

        public FightOverEventArgs(Fight fight, bool killed)
        {
            Fight = fight;
            Killed = killed;
        }
        
    }
}