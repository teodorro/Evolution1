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
        void Clear();

        bool CanBeUpgraded(Animal animal, UpgradeSingle upgrade);
        void AddUpgrade(Animal animal, UpgradeSingle upgrade);

        bool CanBeUpgraded(Animal animalLeft, UpgradePair upgrade);
        void AddUpgrade(Animal animalLeft, UpgradePair upgrade);

        void RemoveUpgrade(Animal animalLeft, UpgradePair upgrade);

//        bool CanBeMovedToPosition(Animal animal, int position);
        void SetNewPosition(Animal animal, int newPosition);
        int GetPosition(Animal animal);
    }



    public class AnimalCollection : IAnimalCollection, IReadOnlyCollection<Animal>
    {
        private readonly List<Animal> _animals = new List<Animal>();
        public ReadOnlyCollection<Animal> Animals => _animals.AsReadOnly();
        private IPlayer _player;
        private Animal NextAnimal(Animal animal) => _animals[_animals.IndexOf(animal) + 1];
        private Animal PreviousAnimal(Animal animal) => _animals[_animals.IndexOf(animal) - 1];


        public AnimalCollection(IPlayer player)
        {
            _player = player ?? throw new ArgumentNullException();
        }


        public Animal AddAnimal()
        {
            var animal = new Animal(_player);
            _animals.Add(animal);
            return animal;
        }

        public void RemoveAnimal(Animal animal)
        {
            if (animal.Upgrades.Any(x => x.GetType().IsSubclassOf(typeof(UpgradePair))))
            {
                var pairUpgrades = animal.Upgrades.Where(x => x.GetType().IsSubclassOf(typeof(UpgradePair))).ToList();
                foreach (var upgr in pairUpgrades)
                {
                    var u = upgr as UpgradePair;
                    if (u.LeftAnimal == animal)
                        NextAnimal(animal).RemoveUpgrade(u);
                    else if (u.RightAnimal == animal)
                        PreviousAnimal(animal).RemoveUpgrade(u);
                    animal.RemoveUpgrade(u);
                }
            }
            if (_animals.Contains(animal))
                _animals.Remove(animal);
        }

        public void Clear() => _animals.Clear();


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
                NextAnimal(animalLeft).AddUpgrade(upgrade, false);
            }
            else
                throw new UpgradesIncompatibleException();
        }

        public void RemoveUpgrade(Animal animalLeft, UpgradePair upgrade)
        {
            if (animalLeft == null || upgrade == null)
                throw new ArgumentNullException();

            animalLeft.RemoveUpgrade(upgrade);
            NextAnimal(animalLeft).RemoveUpgrade(upgrade);
        }

        public void SetNewPosition(Animal animal, int newPosition)
        {
            if (animal == null)
                throw new ArgumentNullException();
            if (!_animals.Contains(animal))
                throw new AnimalNotFoundException();
            if (newPosition < 0 || newPosition >= Count)
                throw new ArgumentOutOfRangeException();

            var animal2 = _animals[newPosition];
            if (animal == animal2)
                return;

            var chains = GetChains();
            var curChain = chains.First(x => x.Animals.Contains(animal));
            var curPosition = curChain.Animals.IndexOf(animal) + curChain.Start;
            var indCurChain = chains.IndexOf(curChain);

            if (newPosition < curChain.Start && newPosition >= 0)
            {
                MoveChain(newPosition, chains, indCurChain);
            }
            else if (newPosition >= curChain.End && newPosition <= chains.Last().End)
            {
                MoveChain(newPosition, chains, indCurChain);
            }
            else if (newPosition >= curChain.Start && newPosition <= curChain.End
                                                   && (curPosition >= curChain.Middle && newPosition < curChain.Middle
                                                       || curPosition < curChain.Middle &&
                                                       newPosition >= curChain.Middle))
            {
                InvertChain(curChain, chains);
            }


        }

        private void MoveChain(int newPosition, ObservableCollection<ChainOfAnimals> chains, int indCurChain)
        {
            var animalAtNewPos = _animals[newPosition];
            var indChainNewPos = chains.IndexOf(chains.First(x => x.Animals.Contains(animalAtNewPos)));
            chains.Move(indCurChain, indChainNewPos);
            ConvertChainsToAnimals(chains);
        }
        
        private void InvertChain(ChainOfAnimals curChain, ObservableCollection<ChainOfAnimals> chains)
        {
            var animals = InvertAnimalsPositions(curChain);
            InvertUpgradesLeftRight(animals);

            curChain.Animals = animals;
            ConvertChainsToAnimals(chains);
        }

        private void InvertUpgradesLeftRight(List<Animal> animals)
        {
            var pairUpgradesWithDups = animals.SelectMany(a => a.Upgrades)
                .Where(u => u.GetType().IsSubclassOf(typeof(UpgradePair)));
            var pairUpgrades = new List<Upgrade>();
            foreach (var u in pairUpgradesWithDups)
                if (pairUpgrades.All(x => x.Id != u.Id))
                    pairUpgrades.Add(u);
            foreach (var u in pairUpgrades)
            {
                var up = u as UpgradePair;
                var r = up.RightAnimal;
                up.RightAnimal = up.LeftAnimal;
                up.LeftAnimal = r;
            }
        }

        private List<Animal> InvertAnimalsPositions(ChainOfAnimals curChain)
        {
            var animals = new List<Animal>();
            for (int i = curChain.Animals.Count - 1; i >= 0; i--)
                animals.Add(curChain.Animals[i]);
            return animals;
        }


        private ObservableCollection<ChainOfAnimals> GetChains()
        {
            var chains = new ObservableCollection<ChainOfAnimals>();

            var j = 0;
            for (int i = 0; i < Count; i++)
            {
                if (i < j)
                    continue;
                for (;;)
                {
                    var next = false;
                    var a = _animals[j];
                    if (a.Upgrades.Any(x => x.GetType().IsSubclassOf(typeof(UpgradePair))))
                        foreach (var u in a.Upgrades.Where(x => x.GetType().IsSubclassOf(typeof(UpgradePair))))
                            if ((u as UpgradePair).LeftAnimal == a)
                            {
                                next = true;
                                break;
                            }
                    j++;
                    if (!next)
                        break;
                }
                chains.Add(new ChainOfAnimals(i, new List<Animal>()));
                for (int k = i; k < j; k++)
                    chains.Last().Animals.Add(_animals[k]);
            }

            return chains;
        }

        private void ConvertChainsToAnimals(ObservableCollection<ChainOfAnimals> chains)
        {
            _animals.Clear();
            foreach (var chain in chains)
                foreach (var animal in chain.Animals)
                    _animals.Add(animal);
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



    internal class ChainOfAnimals
    {
        public List<Animal> Animals { get; set; }

        public double Middle => Start + ((double)Animals.Count) / 2;
        public int Start { get; }
        public int Length => Animals.Count;
        public int End => Start + Animals.Count;

        public ChainOfAnimals(int start, IEnumerable<Animal> animals)
        {
            Animals = animals.ToList();
            Start = start;
        }
    }
}