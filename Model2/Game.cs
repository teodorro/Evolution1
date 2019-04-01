using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Model
{
    public class Game
    {
        private readonly List<Card> _cardsRemain;
        private List<Card> _cardsDump = new List<Card>();
        private List<IPlayer> _players;
        private int _currentMove;
        public const int DefaultPlayerCardsAmount = 6;

        public ReadOnlyCollection<Card> CardsRemain =>_cardsRemain.AsReadOnly();
        public ReadOnlyCollection<IPlayer> Players => _players.AsReadOnly();

        public Turn CurrentTurn { get; private set; }


        internal Game(List<Card> cards, List<IPlayer> players)
        {
            _cardsRemain = cards;
            _players = players;

            
        }


        public void NextTurn()
        {

        }

    }

}
