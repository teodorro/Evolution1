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
        IPlayer Player { get; }
        
        bool CanBeUpgraded(UpgradeSingle upgrade);
        void AddUpgrade(UpgradeSingle upgrade);
        void RemoveUpgrade(UpgradeSingle upgrade);
        void AddFood(int foodPoints);
    }



    public class Animal : ObjectWithId, IAnimal
    {
        private readonly List<Upgrade> _upgrades = new List<Upgrade>();
        public ReadOnlyCollection<Upgrade> Upgrades => _upgrades.AsReadOnly();
        public int FoodNeeded => 1 + Upgrades.Select(x => x.AdditionalFoodNeeded).Sum();
        public int FoodGot { get; private set; } = 0;
        public IPlayer Player { get; }


        public Animal(IPlayer player)
        {
            Player = player ?? throw new ArgumentNullException();
        }


        public void RemoveUpgrade(UpgradeSingle upgrade)
        {
            throw new NotImplementedException();
        }

        public void AddFood(int foodPoints)
        {
            if (foodPoints > 2 || foodPoints < 0)
                throw new ArgumentException("impossible food points");
            if (FoodGot == FoodNeeded)
                throw new AnimalAlreadyFedException();
            FoodGot += foodPoints;
            if (FoodGot > FoodNeeded)
                FoodGot = FoodNeeded;
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