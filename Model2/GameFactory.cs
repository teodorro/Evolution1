using System;
using System.Collections.Generic;
using System.Linq;
using Model.Upgrades;

namespace Model
{
    public class GameFactory
    {
        private static readonly Lazy<GameFactory> _instance = new Lazy<GameFactory>(() => new GameFactory());
        public static GameFactory Instance => _instance.Value;


        public Game GetGame(List<IPlayer> players)
        {
            var cards = MixCards(GetCards());

            foreach (var player in players)
                player.Reset();

            SetDefaultCardsDistribution(cards, players);

            var game = new Game(cards, players.ToList());

            return game;
        }

        public Game GetTestGame(List<IPlayer> players, List<Card> cards)
        {
            foreach (var player in players)
                player.Reset();
            
            var game = new Game(cards, players.ToList());

            return game;
        }

        private List<Card> MixCards(List<Card> cards)
        {
            var mixed = new List<Card>();
            var rand = new Random(DateTime.Now.Millisecond);

            while (cards.Count != 0)
            {
                var ind = rand.Next(cards.Count - 1);
                mixed.Add(cards[ind]);
                cards.RemoveAt(ind);
            }

            return mixed;
        }

        private void SetDefaultCardsDistribution(List<Card> cards, IEnumerable<IPlayer> players)
        {
            for (int i = 0; i < Game.DefaultPlayerCardsAmount; i++)
                foreach (var player in players)
                    if (cards.Count > 0)
                    {
                        var card = cards.Last();
                        player.AddCard(card);
                        cards.Remove(card);
                    }
                    else
                        break;
        }

        private List<Card> GetCards()
        {
            var cards = new List<Card>();

            cards.Add(new Card(new UpgradePoisonous(), new UpgradeCarnivorous()));
            cards.Add(new Card(new UpgradePoisonous(), new UpgradeCarnivorous()));
            cards.Add(new Card(new UpgradePoisonous(), new UpgradeCarnivorous()));
            cards.Add(new Card(new UpgradePoisonous(), new UpgradeCarnivorous()));
            cards.Add(new Card(new UpgradeRunning(), null));
            cards.Add(new Card(new UpgradeRunning(), null));
            cards.Add(new Card(new UpgradeRunning(), null));
            cards.Add(new Card(new UpgradeRunning(), null));
            cards.Add(new Card(new UpgradeHighBodyWeight(), new UpgradeCarnivorous()));
            cards.Add(new Card(new UpgradeHighBodyWeight(), new UpgradeCarnivorous()));
            cards.Add(new Card(new UpgradeHighBodyWeight(), new UpgradeCarnivorous()));
            cards.Add(new Card(new UpgradeHighBodyWeight(), new UpgradeCarnivorous()));
            cards.Add(new Card(new UpgradeHighBodyWeight(), new UpgradeFat()));
            cards.Add(new Card(new UpgradeHighBodyWeight(), new UpgradeFat()));
            cards.Add(new Card(new UpgradeHighBodyWeight(), new UpgradeFat()));
            cards.Add(new Card(new UpgradeHighBodyWeight(), new UpgradeFat()));
            cards.Add(new Card(new UpgradeSwimming(), null));
            cards.Add(new Card(new UpgradeSwimming(), null));
            cards.Add(new Card(new UpgradeSwimming(), null));
            cards.Add(new Card(new UpgradeSwimming(), null));
            cards.Add(new Card(new UpgradeSwimming(), null));
            cards.Add(new Card(new UpgradeSwimming(), null));
            cards.Add(new Card(new UpgradeSwimming(), null));
            cards.Add(new Card(new UpgradeSwimming(), null));
            cards.Add(new Card(new UpgradeBurrowing(), new UpgradeFat()));
            cards.Add(new Card(new UpgradeBurrowing(), new UpgradeFat()));
            cards.Add(new Card(new UpgradeBurrowing(), new UpgradeFat()));
            cards.Add(new Card(new UpgradeBurrowing(), new UpgradeFat()));

            return cards;
        }



    }
}