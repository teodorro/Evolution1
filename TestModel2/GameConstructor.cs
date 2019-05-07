using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Upgrades;

namespace TestModel
{
    public class GameConstructor
    {
        public static GameConstructor Instance => _instance.Value;
        private static readonly Lazy<GameConstructor> _instance = new Lazy<GameConstructor>(() => new GameConstructor());


        public void ArrangeTwoAnimals(out Game game, out Player player1, List<UpgradeSingle> upgrades1, out Player player2, List<UpgradeSingle> upgrades2)
        {
            player1 = new Player("player1");
            player2 = new Player("player2");
            var players = new List<IPlayer>() { player1, player2 };
            var cards = new List<Card>()
            {
                new Card(new UpgradeBurrowing(), new UpgradeBurrowing()),
                new Card(new UpgradeBurrowing(), new UpgradeBurrowing())
            };
            foreach (var upgrade in upgrades1)
                cards.Add(new Card(upgrade, new UpgradeBurrowing()));
            foreach (var upgrade in upgrades2)
                cards.Add(new Card(upgrade, new UpgradeBurrowing()));
            game = GameFactory.Instance.GetTestGame(players, cards);

            var i = 0;
            player1.AddCard(cards[i++]);
            player1.AddAnimal(player1.Cards.First());
            player2.AddCard(cards[i++]);
            player2.AddAnimal(player2.Cards.First());

            for (var j = 0; j < upgrades1.Count; j++)
            {
                player1.AddCard(cards[i + j]);
                player1.AddUpgrade(player1.Animals.First(), player1.Cards.First(), player1.Cards.First().Upgrade1 as UpgradeSingle);
            }

            i += upgrades1.Count;
            for (var j = 0; j < upgrades2.Count; j++)
            {
                player2.AddCard(cards[i + j]);
                player2.AddUpgrade(player2.Animals.First(), player2.Cards.First(), player2.Cards.First().Upgrade1 as UpgradeSingle);
            }
            i += upgrades2.Count;
        }

        public void ArrangeThreeAnimals(out Game game, out Player player1, List<UpgradeSingle> upgrades1, out Player player2, List<UpgradeSingle> upgrades2, List<UpgradeSingle> upgrades3, List<UpgradePair> upgrades23)
        {
            player1 = new Player("player1");
            player2 = new Player("player2");
            var players = new List<IPlayer>() { player1, player2 };
            var cards = new List<Card>()
            {
                new Card(new UpgradeBurrowing(), new UpgradeBurrowing()),
                new Card(new UpgradeBurrowing(), new UpgradeBurrowing()),
                new Card(new UpgradeBurrowing(), new UpgradeBurrowing())
            };
            foreach (var upgrade in upgrades1)
                cards.Add(new Card(upgrade, new UpgradeBurrowing()));
            foreach (var upgrade in upgrades2)
                cards.Add(new Card(upgrade, new UpgradeBurrowing()));
            foreach (var upgrade in upgrades3)
                cards.Add(new Card(upgrade, new UpgradeBurrowing()));
            foreach (var upgrade in upgrades23)
                cards.Add(new Card(upgrade, new UpgradeBurrowing()));
            game = GameFactory.Instance.GetTestGame(players, cards);

            var i = 0;
            player1.AddCard(cards[i++]);
            player1.AddAnimal(player1.Cards.First());
            player2.AddCard(cards[i++]);
            player2.AddAnimal(player2.Cards.First());
            player2.AddCard(cards[i++]);
            player2.AddAnimal(player2.Cards.First());

            for (var j = 0; j < upgrades1.Count; j++)
            {
                player1.AddCard(cards[i + j]);
                player1.AddUpgrade(player1.Animals.First(), player1.Cards.First(), player1.Cards.First().Upgrade1 as UpgradeSingle);
            }

            i += upgrades1.Count;
            for (var j = 0; j < upgrades2.Count; j++)
            {
                player2.AddCard(cards[i + j]);
                player2.AddUpgrade(player2.Animals.First(), player2.Cards.First(), player2.Cards.First().Upgrade1 as UpgradeSingle);
            }
            i += upgrades2.Count;
            for (var j = 0; j < upgrades3.Count; j++)
            {
                player2.AddCard(cards[i + j]);
                player2.AddUpgrade(player2.Animals.Last(), player2.Cards.First(), player2.Cards.First().Upgrade1 as UpgradeSingle);
            }
            i += upgrades3.Count;
            for (var j = 0; j < upgrades23.Count; j++)
            {
                player2.AddCard(cards[i + j]);
                player2.AddUpgrade(player2.Animals.First(), player2.Cards.First(), player2.Cards.First().Upgrade1 as UpgradePair);
            }
            i += upgrades23.Count;
        }
    }
}