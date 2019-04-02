using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Model.Upgrades;

namespace Model
{
    public interface IAnimalCollection
    {
        ReadOnlyCollection<Animal> Animals { get; }

        Animal AddAnimal();
        void RemoveAnimal(Animal animal);
        void Reset();

        bool CanBeUpgraded(Animal animal, UpgradeSingle upgrade);
        void AddUpgrade(Animal animal, UpgradeSingle upgrade);

        bool CanBeUpgraded(Animal animalLeft, UpgradePair upgrade);
        void AddUpgrade(Animal animalLeft, UpgradePair upgrade);

        bool CanBeMovedToPosition(Animal animal, int position);
        void SetNewPosition(Animal animal, int position);
        int GetPosition(Animal animal);
    }



    public class AnimalCollection : IAnimalCollection, IReadOnlyCollection<Animal>
    {
        private readonly List<Animal> _animals = new List<Animal>();
        public ReadOnlyCollection<Animal> Animals => _animals.AsReadOnly();

        
        public Animal AddAnimal()
        {
            var animal = new Animal();
            _animals.Add(animal);
            return animal;
        }

        public void RemoveAnimal(Animal animal)
        {
            if (_animals.Contains(animal))
                _animals.Remove(animal);
        }

        public void Reset() => _animals.Clear();


        public bool CanBeUpgraded(Animal animal, UpgradeSingle upgrade)
        {
            if (_animals.Contains(animal))
                return animal.CanBeUpgraded(upgrade);
            else
                throw new AnimalNotFoundException();
        }

        public bool CanBeUpgraded(Animal animalLeft, UpgradePair upgrade)
        {
            var i1 = GetPosition(animalLeft);
            if (i1 == Count - 1)
                return false;
            var animalRight = _animals[i1 + 1];
            return animalLeft.CanBeUpgraded(upgrade, true) && animalRight.CanBeUpgraded(upgrade, false);
        }

        public void AddUpgrade(Animal animal, UpgradeSingle upgrade)
        {
            if (_animals.Contains(animal))
                animal.AddUpgrade(upgrade);
            else
                throw new AnimalNotFoundException();
        }

        public void AddUpgrade(Animal animalLeft, UpgradePair upgrade)
        {
            if (CanBeUpgraded(animalLeft, upgrade))
            {
                animalLeft.AddUpgrade(upgrade, true);
                _animals[GetPosition(animalLeft) + 1].AddUpgrade(upgrade, false);
            }
            else
                throw new UpgradesIncompatibleException();
        }


        public bool CanBeMovedToPosition(Animal animal, int position)
        {
            if (animal == null)
                throw new ArgumentNullException();
            if (!_animals.Contains(animal))
                throw new AnimalNotFoundException();
            if (position < 0 || position >= Count)
                throw new ArgumentOutOfRangeException();

            var animal2 = _animals[position];
            if (animal == animal2)
                return true;

            throw new NotImplementedException();

        }

        public void SetNewPosition(Animal animal, int position)
        {
            if (CanBeMovedToPosition(animal, position))
                SetNewPosition(animal, position);
            else
                throw new ArgumentException("Unacceptable position");
        }

        public int GetPosition(Animal animal)
        {
            if (animal == null)
                throw new ArgumentNullException();
            if (!_animals.Contains(animal))
                throw new AnimalNotFoundException();
            return _animals.IndexOf(animal);
        }


        public IEnumerator<Animal> GetEnumerator()
        {
            return _animals.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => _animals.Count;
    }
}