using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Model.Upgrades;

namespace Model
{
    public class Game
    {
        private readonly List<Card> _cardsRemain;
        private List<Card> _cardsDump = new List<Card>();
        private List<IPlayer> _players;

        public const int DefaultPlayerCardsAmount = 6;

        public ReadOnlyCollection<Card> CardsRemain =>_cardsRemain.AsReadOnly();
        public ReadOnlyCollection<IPlayer> Players => _players.AsReadOnly();

        public Turn CurrentTurn { get; private set; }

        public event VictimChooseEventHandler VictimChoose;


        internal Game(List<Card> cards, List<IPlayer> players)
        {
            _cardsRemain = cards;
            _players = players;
            SubscsribeAttackEvent();
        }


        #region Fight

        private void OnAttackStart(object sender, StartAttackEventArgs args)
        {
            var fight = new Fight(args.Attacker, _players);
            fight.FightOver += OnFightOver;
            VictimChoose?.Invoke(this, new VictimChooseEventArgs(fight));
        }

        private void OnFightOver(object sender, FightOverEventArgs fightOverEventArgs)
        {
            if (fightOverEventArgs.Killed)
            {
                fightOverEventArgs.Fight.Victim.Player.RemoveAnimal(fightOverEventArgs.Fight.Victim);
                fightOverEventArgs.Fight.Attacker.AddFood(new FoodToken(false), new FoodToken(false));
            }
        }

        private void SubscsribeAttackEvent()
        {
            foreach (var card in _cardsRemain)
            {
                if (card.Upgrade1?.UpgradeType == UpgradeType.Carnivorous)
                    (card.Upgrade1 as UpgradeCarnivorous).OnUse += OnAttackStart;
                if (card.Upgrade2?.UpgradeType == UpgradeType.Carnivorous)
                    (card.Upgrade2 as UpgradeCarnivorous).OnUse += OnAttackStart;
            }
            foreach (var player in _players)
                foreach (var card in player.Cards)
                {
                    if (card.Upgrade1?.UpgradeType == UpgradeType.Carnivorous)
                        (card.Upgrade1 as UpgradeCarnivorous).OnUse += OnAttackStart;
                    if (card.Upgrade2?.UpgradeType == UpgradeType.Carnivorous)
                        (card.Upgrade2 as UpgradeCarnivorous).OnUse += OnAttackStart;
                }
        }

        #endregion // Fight

    }


    



}
