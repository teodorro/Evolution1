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

        void Reset();

        //-- development phase

        void AddCard(Card card);
        void AddAnimal(Card card);
        void ChangeOrder(ObservableCollection<Animal> animals);
        void AddUpgrade(Animal animal, Card card, UpgradeSingle upgrade);
        void AddUpgrade(Animal animalFirst, Animal animalSecond, Card card, UpgradePair upgrade);
        void AddParasite(IPlayer player, Card card);

        //-- eating phase

        void FeedWithPlantFood(Animal animal);
        void AttackAnimal(Animal carnivore, Animal victim);


        void RemoveAnimal(Animal animal);
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

        public void AddAnimal(Card card)
        {
            if (!_cards.Contains(card))
                throw new NoCardsException();
            _cards.Remove(card);
            Animals.AddAnimal();
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

        public void AddParasite(IPlayer player, Card card)
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            _cards.Clear();
            Animals.Clear();
        }

    }
}