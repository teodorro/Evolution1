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


//    public delegate void DevelopingStoppedEventHandler(object sender, DevelopingStoppedEventArgs e);
//
//    public class DevelopingStoppedEventArgs : EventArgs
//    {
//        public IPlayer Player { get; }
//        public DevelopingStoppedEventArgs(IPlayer player) => Player = player;
//    }



    public interface IPlayer
    {
        ReadOnlyCollection<Card> Cards { get; }
        AnimalCollection Animals { get; }

        void Reset();

        //-- development phase

        void AddCard(Card card);
        void AddAnimal(Card card);
        void AddUpgrade(Animal animal, Card card, UpgradeSingle upgrade);
        void AddUpgrade(Animal animalLeft, Card card, UpgradePair upgrade);

        //-- eating phase

        void Feed(Animal animal);
//        void AttackAnimal(Animal carnivore, Animal victim);
        void UseUpgrade(Animal animal, UpgradeSingle upgrade);


        void RemoveAnimal(Animal animal);
    }



    public class Player : IPlayer
    {
        private readonly List<Card> _cards = new List<Card>();

        public ReadOnlyCollection<Card> Cards => _cards.AsReadOnly();
        public AnimalCollection Animals { get; private set; } = new AnimalCollection();
        
        public event EventHandler<EventArgs> DevelopingStopped;


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

        public void RemoveAnimal(Animal animal)
        {
            Animals.RemoveAnimal(animal);
        }

        public void ChangeOrder(Animal animal, int newPosition)
        {
            Animals.SetNewPosition(animal, newPosition);
        }

        public void AddUpgrade(Animal animal, Card card, UpgradeSingle upgrade)
        {
            if (card.Upgrade1 != upgrade && card.Upgrade2 != upgrade)
                throw new CardUpgradeIncostintenceException();
            Animals.AddUpgrade(animal, upgrade);
            _cards.Remove(card);
        }

        public void AddUpgrade(Animal animalLeft, Card card, UpgradePair upgrade)
        {
            if (card.Upgrade1 != upgrade && card.Upgrade2 != upgrade)
                throw new CardUpgradeIncostintenceException();
            Animals.AddUpgrade(animalLeft, upgrade);
            _cards.Remove(card);
        }

        public void AddParasite(IPlayer player, Animal animal, Card card)
        {
            if (player == null || animal == null || card == null)
                throw new ArgumentNullException();
            if (card.Upgrade1.UpgradeType != UpgradeType.Parasite && card.Upgrade2.UpgradeType != UpgradeType.Parasite)
                throw new CardUpgradeIncostintenceException();
            if (!player.Animals.Contains(animal))
                throw new PlayerAnimalIncostintenceException();
            if (Animals.Contains(animal))
                throw new NoParasiteToYourAnimalsException();

            player.AddUpgrade(animal, card, new UpgradeParasite());
            _cards.Remove(card);
        }

        public void Reset()
        {
            _cards.Clear();
            Animals.Clear();
        }




        public void Feed(Animal animal)
        {
            throw new NotImplementedException();
        }

//        public void AttackAnimal(Animal carnivore, Animal victim)
//        {
//            throw new NotImplementedException();
//        }

        public void UseUpgrade(Animal animal, UpgradeSingle upgrade)
        {
            throw new NotImplementedException();
        }

    }
}