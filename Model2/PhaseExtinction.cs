using System;
using System.Collections.Generic;
using System.Linq;
using Model.Upgrades;

namespace Model
{
    public class PhaseExtinction : IPhase
    {
        private IEnumerable<IPlayer> players;

        public PhaseExtinction(IEnumerable<IPlayer> players)
        {
            this.players = players ?? throw new ArgumentNullException();
        }


        public void StartExtinct()
        {
            foreach (var player in players)
                foreach (var animal in player.Animals.ToList())
                    if (animal.Poisoned || Starve(animal))
                        player.Animals.RemoveAnimal(animal);
        }

        private bool Starve(IAnimal animal)
        {
            if (animal.FoodNeeded - animal.FoodGot == 0)
                return false;
            if (animal.NoFreeFat())
                return true;
            var fatCards = animal.Upgrades.Where(x => x.UpgradeType == UpgradeType.Fat);
            var fullFatCards = fatCards.Where(x => (x as UpgradeFat).Full);
            if (!fullFatCards.Any())
                return true;
            if (fullFatCards.Count() < animal.FoodNeeded - animal.FoodGot)
                return true;
            for (int i = 0; i < animal.FoodNeeded - animal.FoodGot; i++)
            {
                (fullFatCards.First(x => (x as UpgradeFat).Full) as UpgradeFat).Full = false;
                animal.AddFood(new FoodToken(false), null);
                if (animal.FoodNeeded - animal.FoodGot == 0)
                    return false;
            }

            return true;
        }
    }
}