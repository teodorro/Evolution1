using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Upgrades;
using Xunit;

namespace TestModel
{
    public class TestAnimal
    {
        private static Animal GetAnimal() => new Animal(new Player("noname"));
        public static Animal UpgradeAnimal(Animal animal, IEnumerable<UpgradeType> upgrades)
        {
            foreach (var u in upgrades)
                switch (u)
                {
                    case UpgradeType.Burrowing:
                        animal.AddUpgrade(new UpgradeBurrowing());
                        break;
                    case UpgradeType.Camouflage:
                        animal.AddUpgrade(new UpgradeCamouflage());
                        break;
                    case UpgradeType.Carnivorous:
                        animal.AddUpgrade(new UpgradeCarnivorous());
                        break;
                    case UpgradeType.Fat:
                        animal.AddUpgrade(new UpgradeFat());
                        break;
                    case UpgradeType.Grazing:
                        animal.AddUpgrade(new UpgradeGrazing());
                        break;
                    case UpgradeType.Hibernation:
                        animal.AddUpgrade(new UpgradeHibernation());
                        break;
                    case UpgradeType.HighBodyWeight:
                        animal.AddUpgrade(new UpgradeHighBodyWeight());
                        break;
                    case UpgradeType.Mimicry:
                        animal.AddUpgrade(new UpgradeMimicry());
                        break;
                    case UpgradeType.Parasite:
                        animal.AddUpgrade(new UpgradeParasite());
                        break;
                    case UpgradeType.Piracy:
                        animal.AddUpgrade(new UpgradePiracy());
                        break;
                    case UpgradeType.Poisonous:
                        animal.AddUpgrade(new UpgradePoisonous());
                        break;
                    case UpgradeType.Running:
                        animal.AddUpgrade(new UpgradeRunning());
                        break;
                    case UpgradeType.Scavanger:
                        animal.AddUpgrade(new UpgradeScavenger());
                        break;
                    case UpgradeType.SharpVision:
                        animal.AddUpgrade(new UpgradeSharpVision());
                        break;
                    case UpgradeType.Swimming:
                        animal.AddUpgrade(new UpgradeSwimming());
                        break;
                    case UpgradeType.TailLoss:
                        animal.AddUpgrade(new UpgradeTailLoss());
                        break;
                }
            return animal;
        }

        [Fact]
        public void TestCanBeUpgradedNull()
        {
            var animal = UpgradeAnimal(GetAnimal(), new List<UpgradeType>() { UpgradeType.Burrowing, UpgradeType.Camouflage });

            Assert.Throws<ArgumentNullException>(() => animal.CanBeUpgraded(null));
        }

        [Fact]
        public void TestCanBeUpgradedSimple()
        {
            var animal = UpgradeAnimal(GetAnimal(), new List<UpgradeType>() { UpgradeType.Burrowing, UpgradeType.Camouflage });

            Assert.True(animal.CanBeUpgraded(new UpgradeFat()));
        }

        [Fact]
        public void TestCanBeUpgradedDup()
        {
            var animal = UpgradeAnimal(GetAnimal(), new List<UpgradeType>() { UpgradeType.Burrowing, UpgradeType.Camouflage });

            Assert.False(animal.CanBeUpgraded(new UpgradeBurrowing()));
        }

        [Fact]
        public void TestCanBeUpgradedFatTwice()
        {
            var animal = UpgradeAnimal(GetAnimal(), new List<UpgradeType>() { UpgradeType.Burrowing, UpgradeType.Fat });

            Assert.True(animal.CanBeUpgraded(new UpgradeFat()));
        }

        [Fact]
        public void TestCanBeUpgradedCarnivorousScavenger()
        {
            var animal = UpgradeAnimal(GetAnimal(), new List<UpgradeType>() { UpgradeType.Burrowing, UpgradeType.Carnivorous });

            Assert.False(animal.CanBeUpgraded(new UpgradeScavenger()));
        }

        [Fact]
        public void TestCanBeUpgradedScavengerCarnivorous()
        {
            var animal = UpgradeAnimal(GetAnimal(), new List<UpgradeType>() { UpgradeType.Burrowing, UpgradeType.Scavanger });

            Assert.False(animal.CanBeUpgraded(new UpgradeCarnivorous()));
        }



        [Fact]
        public void TestAddUpgradeNull()
        {
            var animal = UpgradeAnimal(GetAnimal(), new List<UpgradeType>() { UpgradeType.Burrowing, UpgradeType.Camouflage });

            Assert.Throws<ArgumentNullException>(() => animal.AddUpgrade(null));
        }

        [Fact]
        public void TestAddUpgradeSimple()
        {
            var animal = UpgradeAnimal(GetAnimal(), new List<UpgradeType>() { UpgradeType.Burrowing, UpgradeType.Camouflage });

            animal.AddUpgrade(new UpgradeFat());

            Assert.Contains(animal.Upgrades, x => x.UpgradeType == UpgradeType.Fat);
        }

        [Fact]
        public void TestAddUpgradeDup()
        {
            var animal = UpgradeAnimal(GetAnimal(), new List<UpgradeType>() { UpgradeType.Burrowing, UpgradeType.Camouflage });

            Assert.Throws<UpgradesIncompatibleException>(() => animal.AddUpgrade(new UpgradeBurrowing()));
        }

        [Fact]
        public void TestFoodNeeded()
        {
            var animal = UpgradeAnimal(GetAnimal(), new List<UpgradeType>() { UpgradeType.Carnivorous, UpgradeType.Camouflage, UpgradeType.Parasite });

            Assert.Equal(4, animal.FoodNeeded);
        }

        [Fact]
        public void TestAddFood()
        {
            var animal = UpgradeAnimal(GetAnimal(), new List<UpgradeType>() { UpgradeType.Carnivorous, UpgradeType.Camouflage, UpgradeType.Parasite });

            Assert.Throws<ArgumentException>(() => animal.AddFood(3));
            animal.AddFood(1);
            animal.AddFood(2);
            Assert.Equal(3, animal.FoodGot);
            animal.AddFood(2);
            Assert.Equal(4, animal.FoodGot);
            Assert.Throws<AnimalAlreadyFedException>(() => animal.AddFood(1));
        }

        [Fact]
        public void RemoveUpgradeNull()
        {
            var animal = UpgradeAnimal(GetAnimal(), new List<UpgradeType>() { UpgradeType.Carnivorous, UpgradeType.Camouflage, UpgradeType.Parasite });

            Assert.Throws<ArgumentNullException>(() => animal.RemoveUpgrade(null));
        }

        [Fact]
        public void RemoveUpgradeSimple()
        {
            var animal = UpgradeAnimal(GetAnimal(), new List<UpgradeType>() { UpgradeType.Carnivorous, UpgradeType.Camouflage, UpgradeType.Parasite });
            var upgrade = animal.Upgrades.First(x => x.UpgradeType == UpgradeType.Camouflage) as UpgradeSingle;

            animal.RemoveUpgrade(upgrade);

            Assert.DoesNotContain(upgrade, animal.Upgrades);
        }

        [Fact]
        public void RemoveUpgradeSimple2()
        {
            var animal = UpgradeAnimal(GetAnimal(), new List<UpgradeType>() { UpgradeType.Carnivorous, UpgradeType.Camouflage, UpgradeType.Parasite });
            var upgrade = new UpgradeFat();

            animal.RemoveUpgrade(upgrade);

            Assert.DoesNotContain(upgrade, animal.Upgrades);
        }


    }
}