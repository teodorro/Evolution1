using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Model.Upgrades;

namespace Model
{
    public enum FoodType
    {
        FoodSupply,
        Additional
    }


    public delegate void TryFeedAnimalEventHandler(object sender, TryFeedAnimalEventArgs e);

    public class TryFeedAnimalEventArgs : EventArgs
    {
        public IPlayer Player { get; }
        public IAnimal Animal { get; }
        public TryFeedAnimalEventArgs(IPlayer player, IAnimal animal)
        {
            Player = player;
            Animal = animal;
        }
    }



    public interface IPlayer
    {
        ReadOnlyCollection<Card> Cards { get; }
        AnimalCollection Animals { get; }
        string Name { get; }

        event TryFeedAnimalEventHandler TryFeedAnimalEvent;

        void Reset();

        //-- development phase

        void AddCard(Card card);
        void AddAnimal(Card card);
        void AddUpgrade(IAnimal animal, Card card, UpgradeSingle upgrade);
        void AddUpgrade(IAnimal animalLeft, Card card, UpgradePair upgrade);

        //-- eating phase

        void TryFeed(IAnimal animal);
        void Feed(IAnimal animal);
        //        void AttackAnimal(Animal carnivore, Animal victim);
        void UseUpgrade(IAnimal animal, UpgradeSingle upgrade);
        void AddParasite(IPlayer player, IAnimal animal, Card card);

        void RemoveAnimal(IAnimal animal);
    }



    public class Player : IPlayer
    {
        private readonly List<Card> _cards = new List<Card>();

        public ReadOnlyCollection<Card> Cards => _cards.AsReadOnly();
        public AnimalCollection Animals { get; }
        public string Name { get; }
        public event TryFeedAnimalEventHandler TryFeedAnimalEvent;

        public event EventHandler<EventArgs> DevelopingStopped;


        public Player(string name)
        {
            Animals = new AnimalCollection(this);
            Name = name;
        }


        public void AddCard(Card card)
        {
            if (card == null)
                throw new ArgumentNullException("null card");
            _cards.Add(card);
        }

        public void AddAnimal(Card card)
        {
            if (!_cards.Contains(card))
                throw new NoCardsException();
            _cards.Remove(card);
            Animals.AddAnimal();
        }

        public void RemoveAnimal(IAnimal animal)
        {
            Animals.RemoveAnimal(animal);
        }

        public void ChangeOrder(IAnimal animal, int newPosition)
        {
            Animals.SetNewPosition(animal, newPosition);
        }

        public void AddUpgrade(IAnimal animal, Card card, UpgradeSingle upgrade)
        {
            if (card.Upgrade1 != upgrade && card.Upgrade2 != upgrade)
                throw new CardUpgradeIncostintenceException();
            Animals.AddUpgrade(animal, upgrade);
            _cards.Remove(card);
        }

        public void AddUpgrade(IAnimal animalLeft, Card card, UpgradePair upgrade)
        {
            if (card.Upgrade1 != upgrade && card.Upgrade2 != upgrade)
                throw new CardUpgradeIncostintenceException();
            Animals.AddUpgrade(animalLeft, upgrade);
            _cards.Remove(card);
        }

        public void AddParasite(IPlayer player, IAnimal animal, Card card)
        {
            if (player == null || animal == null || card == null)
                throw new ArgumentNullException();
            if (card.Upgrade1.UpgradeType != UpgradeType.Parasite && card.Upgrade2.UpgradeType != UpgradeType.Parasite)
                throw new CardUpgradeIncostintenceException();
            if (!player.Animals.Contains(animal))
                throw new PlayerAnimalIncostintenceException();
            if (Animals.Contains(animal))
                throw new NoParasiteToYourAnimalsException();

            player.AddUpgrade(animal, card, (UpgradeSingle)(card.Upgrade1.UpgradeType == UpgradeType.Parasite ? card.Upgrade1 : card.Upgrade2));
            _cards.Remove(card);
        }

        public void Reset()
        {
            _cards.Clear();
            Animals.Clear();
        }

        public void TryFeed(IAnimal animal)
        {
            if (animal == null)
                throw new ArgumentNullException();
            if (!Animals.Contains(animal))
                throw new AnimalNotFoundException();
            TryFeedAnimalEvent?.Invoke(this, new TryFeedAnimalEventArgs(this, animal));
        }

        public void Feed(IAnimal animal)
        {
            if (animal == null)
                throw new ArgumentNullException();
            if (!Animals.Contains(animal))
                throw new AnimalNotFoundException();
            animal.AddFood(new FoodToken(true), null);
        }


//        public void AttackAnimal(Animal carnivore, Animal victim)
//        {
//            throw new NotImplementedException();
//        }

        public void UseUpgrade(IAnimal animal, UpgradeSingle upgrade)
        {
            throw new NotImplementedException();
        }



        
    }



}