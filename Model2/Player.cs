using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Model.Upgrades;

namespace Model
{
    public interface IPlayer
    {
        ReadOnlyCollection<Card> Cards { get; }
        AnimalCollection Animals { get; }
        ReadOnlyCollection<IPlayer> OtherPlayers { get; }

        void AddCard(Card card);
        Animal AddAnimal(Card card);
        void RemoveAnimal(Animal animal);
        void ChangeOrder(ObservableCollection<Animal> animals);
        void AddUpgrade(Animal animal, Card card, UpgradeSingle upgrade);
        void AddUpgrade(Animal animalFirst, Animal animalSecond, Card card, UpgradePair upgrade);
        bool CanBeUpgraded(Animal animal, UpgradeSingle upgrade);
        bool CanBeUpgraded(Animal animalFirst, Animal animalSecond, UpgradePair upgrade);
        void AddParasite(IPlayer player, Card card);
        void Reset();
    }



    public class Player : IPlayer
    {
        private readonly List<Card> _cards = new List<Card>();
        private readonly List<IPlayer> _players = new List<IPlayer>();

        public ReadOnlyCollection<Card> Cards => _cards.AsReadOnly();
        public AnimalCollection Animals { get; private set; }
        public ReadOnlyCollection<IPlayer> OtherPlayers { get; }


        public void AddCard(Card card)
        {
            if (card == null)
                throw new ArgumentNullException("null card");
            _cards.Add(card);
        }

        public Animal AddAnimal(Card card)
        {
            if (!_cards.Contains(card))
                throw new NoCardsException();
            _cards.Remove(card);
            var animal = new Animal();
            //_animals.Add(animal);
            return animal;
        }

        public void RemoveAnimal(Animal animal)
        {
//            if (_animals.Contains(animal))
//                _animals.Remove(animal);
//            else
//                throw new PlayerAnimalIncostintenceException();
        }

        public void ChangeOrder(ObservableCollection<Animal> animals)
        {
            
        }

        public void AddUpgrade(Animal animal, Card card, UpgradeSingle upgrade)
        {
            throw new NotImplementedException();
        }

        public void AddUpgrade(Animal animalFirst, Animal animalSecond, Card card, UpgradePair upgrade)
        {
            throw new NotImplementedException();
        }

        public void AddUpgrade(Animal animal, Card card, Upgrade upgrade)
        {

        }

        public bool CanBeUpgraded(Animal animal, UpgradeSingle upgrade)
        {
            // if it's parasite - false
            // if it's fat - true
            // if it's not pair, check it's not already exist 
            // if it's scavenger, check it's not carnivorous
            // if it's carnivorous, check it's not scavenger
            // if it's pair, check there's no same card with this animal on the same position
            
            if (upgrade is UpgradeFat)
                return true;
            if (upgrade is UpgradeParasite)
                return false;

            if (upgrade is UpgradeCarnivorous)
            {
            }

            if (upgrade is UpgradeScavanger)
            {
            }

            return true;
        }

        public bool CanBeUpgraded(Animal animalFirst, Animal animalSecond, UpgradePair upgrade)
        {
            throw new NotImplementedException();
        }

        public void AddParasite(IPlayer player, Card card)
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            _cards.Clear();
//            _animals.Clear();
        }

    }
}