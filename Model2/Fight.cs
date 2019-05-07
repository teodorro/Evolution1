using System;
using System.Collections.Generic;
using System.Linq;
using Model.Upgrades;

namespace Model
{
    public interface IFight
    {
        IAnimal Attacker { get; }
        void Start(IAnimal victim);
        void Cancel();
        event DefenceChooseHandler DefenceChoose;
        event FightOverHandler FightOver;
    }


    public class Fight : IFight
    {
        private IAnimal _attacker;
        private IAnimal _victim;
        private Dictionary<Upgrade, bool> DefenceCardsNotUsed = new Dictionary<Upgrade, bool>();
        private List<IPlayer> _players;

        public IAnimal Victim => _victim;
        public IAnimal Attacker => _attacker;
        public event DefenceChooseHandler DefenceChoose;
        public event FightOverHandler FightOver;


        public Fight(IAnimal attacker, List<IPlayer> players)
        {
            _attacker = attacker;
            _players = players;
        }


        public void Start(IAnimal victim)
        {
            _victim = victim;
            var defence = _victim.Upgrades.Where(x =>
                x.UpgradeType == UpgradeType.Mimicry || x.UpgradeType == UpgradeType.Running ||
                x.UpgradeType == UpgradeType.TailLoss).ToList();

            if (defence.Count > 0)
            {
                DefenceCardsNotUsed = defence.ToDictionary(item => item, item => true);
                DefenceChoose?.Invoke(this, new DefenceChooseEventArgs(this, DefenceCardsNotUsed));
            }
            else
            {
                if (victim.Upgrades.Any(x => x.UpgradeType == UpgradeType.Poisonous))
                    Attacker.Poisoned = true;
                FightOver?.Invoke(this, new FightOverEventArgs(this, true));
            }
        }

        public void Cancel()
        {
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
        public Fight Fight { get; }
        public Dictionary<Upgrade, bool> DefenceCardsNotUsed { get; }

        public DefenceChooseEventArgs(Fight fight, Dictionary<Upgrade, bool> defenceCardsNotUsed)
        {
            Fight = fight;
            DefenceCardsNotUsed = defenceCardsNotUsed;
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