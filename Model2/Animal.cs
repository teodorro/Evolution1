using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Model.Upgrades;

namespace Model
{
    public interface IAnimal
    {
        ReadOnlyCollection<Upgrade> Upgrades { get; }
        int FoodNeeded { get; }
        int FoodGot { get; }
        bool Hungry { get; }
        bool NoFreeFat { get; }
        bool CanEat { get; }

        IPlayer Player { get; }
        bool Poisoned { get; set; }
        bool CanAttack { get; }

        bool CanBeUpgraded(UpgradeSingle upgrade);
        void AddUpgrade(UpgradeSingle upgrade);
        void RemoveUpgrade(UpgradeSingle upgrade);
        void AddFood(FoodToken foodToken, FoodToken foodToken2);
    }



    public class Animal : ObjectWithId, IAnimal
    {
        private readonly List<Upgrade> _upgrades = new List<Upgrade>();
        public ReadOnlyCollection<Upgrade> Upgrades => _upgrades.AsReadOnly();
        public int FoodNeeded => 1 + Upgrades.Select(x => x.AdditionalFoodNeeded).Sum();
        public int FoodGot { get; private set; } = 0;
        public bool Hungry => FoodNeeded - FoodGot > 0;
        public IPlayer Player { get; }
        public bool Poisoned { get; set; } = false;

        public bool CanAttack => _upgrades.Any(x => x.UpgradeType == UpgradeType.Carnivorous);

        public bool NoFreeFat => _upgrades.All(x => x.UpgradeType != UpgradeType.Fat) || _upgrades.Where(x => x.UpgradeType == UpgradeType.Fat).All(y => ((UpgradeFat)y).Full);
        public bool CanEat => Hungry || _upgrades.Any(x => x.UpgradeType == UpgradeType.Fat && !(x as UpgradeFat).Full);

        public Animal(IPlayer player)
        {
            Player = player ?? throw new ArgumentNullException();
        }


        public void RemoveUpgrade(UpgradeSingle upgrade)
        {
            if (upgrade == null)
                throw new ArgumentNullException();
            if (_upgrades.Contains(upgrade))
                _upgrades.Remove(upgrade);
        }

        public void AddFood(FoodToken foodToken, FoodToken foodToken2 = null)
        {
            if (foodToken == null)
                throw new ArgumentNullException();
            if (!Hungry)
                throw new AnimalAlreadyFedException();
            FoodGot += 1;
            if (foodToken2 != null)
            {
                if (FoodGot < FoodNeeded)
                    FoodGot += 1;
                else if (CanEat)
                    (_upgrades.First(x => x.UpgradeType == UpgradeType.Fat && !(x as UpgradeFat).Full) as UpgradeFat)
                        .Full = true;
            }
        }

        public bool CanBeUpgraded(UpgradeSingle upgrade)
        {
            if (upgrade == null)
                throw new ArgumentNullException();

            switch (upgrade)
            {
                case UpgradeFat f:
                    return true;
                case UpgradeParasite p:
                    return true;
                case UpgradeCarnivorous c:
                    return _upgrades.All(x => x.GetType() != typeof(UpgradeCarnivorous) && x.GetType() != typeof(UpgradeScavenger));
                case UpgradeScavenger s:
                    return _upgrades.All(x => x.GetType() != typeof(UpgradeCarnivorous) && x.GetType() != typeof(UpgradeScavenger));
                default:
                    return _upgrades.All(x => x.GetType() != upgrade.GetType());
            }
        }

        public void Attack()
        {
            (_upgrades.First(x => x.UpgradeType == UpgradeType.Carnivorous) as UpgradeCarnivorous)?.Use();
        }

        public void AddUpgrade(UpgradeSingle upgrade)
        {
            if (CanBeUpgraded(upgrade))
            {
                _upgrades.Add(upgrade);
                upgrade.Animal = this;
            }
            else
                throw new UpgradesIncompatibleException();
        }

        internal bool CanBeUpgraded(UpgradePair upgrade, bool left)
        {
            if (upgrade == null)
                throw new ArgumentNullException();
            if (_upgrades.All(x => x.UpgradeType != upgrade.UpgradeType))
                return true;
            if (_upgrades.Count(x => x.UpgradeType != upgrade.UpgradeType) >= 2)
                return false;

            var u = Upgrades.First(x => x.UpgradeType == upgrade.UpgradeType) as UpgradePair;
            if (left)
                return u.LeftAnimal != this;
            else
                return u.RightAnimal != this;
        }
        
        internal void AddUpgrade(UpgradePair upgrade, bool left)
        {
            if (CanBeUpgraded(upgrade, left))
            {
                if (left)
                    upgrade.LeftAnimal = this;
                else
                    upgrade.RightAnimal = this;
                _upgrades.Add(upgrade);
            }
            else
                throw new UpgradesIncompatibleException();
        }

        internal void RemoveUpgrade(UpgradePair upgrade)
        {
            if (_upgrades.Contains(upgrade))
                _upgrades.Remove(upgrade);
        }

    }
    
}