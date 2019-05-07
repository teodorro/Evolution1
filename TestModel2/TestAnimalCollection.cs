using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Upgrades;
using Xunit;

namespace TestModel
{
    public class TestAnimalCollection
    {
        private static AnimalCollection GetAnimals() => new AnimalCollection(new Player("noname"));
        private static Animal GetAnimal() => new Animal(new Player("noname"));

        private static AnimalCollection GetThreeAnimals(out IAnimal a1, out IAnimal a2, out IAnimal a3)
        {
            var animals = GetAnimals();
            a1 = animals.AddAnimal();
            a2 = animals.AddAnimal();
            a3 = animals.AddAnimal();
            return animals;
        }

        private static AnimalCollection GetEightAnimals(out IAnimal a1, out IAnimal a2, out IAnimal a3, out IAnimal a4, 
            out IAnimal a5, out IAnimal a6, out IAnimal a7, out IAnimal a8)
        {
            var animals = GetAnimals();
            a1 = animals.AddAnimal();
            a2 = animals.AddAnimal();
            a3 = animals.AddAnimal();
            a4 = animals.AddAnimal();
            a5 = animals.AddAnimal();
            a6 = animals.AddAnimal();
            a7 = animals.AddAnimal();
            a8 = animals.AddAnimal();
            return animals;
        }



        [Fact]
        public void TestAnimalsList()
        {
            var animals = GetAnimals();

            animals.AddAnimal();
            Assert.Single(animals.Animals);

            animals.RemoveAnimal(animals.Animals.First());
            Assert.Empty(animals.Animals);
            animals.RemoveAnimal(GetAnimal());
            Assert.Empty(animals.Animals);

            animals.AddAnimal();
            animals.AddAnimal();
            animals.Clear();
            Assert.Empty(animals.Animals);
        }



        #region CanBeUpgradePair

        [Fact]
        public void TestCanBeUpgradedPairNull()
        {
            var animals = GetThreeAnimals(out var a1, out var a2, out var a3);

            UpgradeCommunication u = null;
            Assert.Throws<ArgumentNullException>(() => animals.CanBeUpgraded(a2, u));
            Assert.Throws<ArgumentNullException>(() => animals.CanBeUpgraded(null, new UpgradeCommunication()));
        }

        [Fact]
        public void TestCanBeUpgradedPairSimple()
        {
            var animals = GetThreeAnimals(out var a1, out var a2, out var a3);

            Assert.True(animals.CanBeUpgraded(a2, new UpgradeCommunication()));
        }
        
        [Fact]
        public void TestCanBeUpgradedPairOutsider()
        {
            var animals = GetThreeAnimals(out var a1, out var a2, out var a3);

            Assert.Throws<AnimalNotFoundException>(() => animals.CanBeUpgraded(GetAnimal(), new UpgradeCommunication()));
        }

        [Fact]
        public void TestCanBeUpgradedPairEnd()
        {
            var animals = GetThreeAnimals(out var a1, out var a2, out var a3);

            Assert.False(animals.CanBeUpgraded(a3, new UpgradeCommunication()));
        }

        [Fact]
        public void TestCanBeUpgradedPairDupSamePosition()
        {
            var animals = GetThreeAnimals(out var a1, out var a2, out var a3);

            animals.AddUpgrade(a2, new UpgradeCommunication());
            Assert.False(animals.CanBeUpgraded(a2, new UpgradeCommunication()));
        }

        [Fact]
        public void TestCanBeUpgradedPairDupAnotherPosition()
        {
            var animals = GetThreeAnimals(out var a1, out var a2, out var a3);

            animals.AddUpgrade(a1, new UpgradeCommunication());
            Assert.True(animals.CanBeUpgraded(a2, new UpgradeCommunication()));
        }

        #endregion //CanBeUpgradePair


        #region AddUpgradePair

        [Fact]
        public void TestAddUpgradePairSimple()
        {
            var animals = GetThreeAnimals(out var a1, out var a2, out var a3);

            animals.AddUpgrade(a1, new UpgradeCommunication());

            Assert.Contains(a1.Upgrades, x => x.UpgradeType == UpgradeType.Communication);
            Assert.Contains(a2.Upgrades, x => x.UpgradeType == UpgradeType.Communication);
            Assert.Equal(a1.Upgrades[0], a2.Upgrades[0]);
        }

        [Fact]
        public void TestAddUpgradePairNotSimple()
        {
            var animals = GetThreeAnimals(out var a1, out var a2, out var a3);

            animals.AddUpgrade(a1, new UpgradeCommunication());
            animals.AddUpgrade(a2, new UpgradeCommunication());

            Assert.Equal(2, a2.Upgrades.Count(x => x.UpgradeType == UpgradeType.Communication));
        }

        [Fact]
        public void TestAddUpgradePairWrongPos()
        {
            var animals = GetThreeAnimals(out var a1, out var a2, out var a3);

            Assert.Throws<UpgradesIncompatibleException>(() => animals.AddUpgrade(a3, new UpgradeCommunication()));

        }

        #endregion //AddUpgradePair


        #region Position

        [Fact]
        public void TestGetPosition()
        {
            var animals = GetThreeAnimals(out var a1, out var a2, out var a3);

            Assert.Equal(1, animals.GetPosition(a2));
            Assert.Throws<ArgumentNullException>(() => animals.GetPosition(null));
            Assert.Throws<AnimalNotFoundException>(() => animals.GetPosition(GetAnimal()));
        }
        
        [Fact]
        public void TestSetPositionSimple()
        {
            var animals = GetEightAnimals(out var a1, out var a2, out var a3, out var a4, out var a5, out var a6, out var a7, out var a8);

            animals.SetNewPosition(a4, 1);
            Assert.Equal(1, animals.GetPosition(a4));
            Assert.Equal(2, animals.GetPosition(a2));
            Assert.Equal(3, animals.GetPosition(a3));

            animals.SetNewPosition(a4, 3);
            Assert.Equal(3, animals.GetPosition(a4));
            Assert.Equal(1, animals.GetPosition(a2));
            Assert.Equal(2, animals.GetPosition(a3));
        }

        [Fact]
        public void TestSetPositionChainChainLeftNeighbors()
        {
            var animals = GetEightAnimals(out var a1, out var a2, out var a3, out var a4, out var a5, out var a6, out var a7, out var a8);
            var c1 = new UpgradeCommunication();
            var c2 = new UpgradeCommunication();
            animals.AddUpgrade(a2, c1);
            animals.AddUpgrade(a3, c2);

            animals.SetNewPosition(a3, 0);

            Assert.Equal(0, animals.GetPosition(a2));
            Assert.Equal(1, animals.GetPosition(a3));
            Assert.Equal(2, animals.GetPosition(a4));
            Assert.Equal(a3, c2.LeftAnimal);
            Assert.Equal(a2, c1.LeftAnimal);
        }

        [Fact]
        public void TestSetPositionChainChainRightNeighbors()
        {
            var animals = GetEightAnimals(out var a1, out var a2, out var a3, out var a4, out var a5, out var a6, out var a7, out var a8);
            var c1 = new UpgradeCommunication();
            var c2 = new UpgradeCommunication();
            animals.AddUpgrade(a2, c1);
            animals.AddUpgrade(a3, c2);

            animals.SetNewPosition(a2, 4);

            Assert.Equal(2, animals.GetPosition(a2));
            Assert.Equal(3, animals.GetPosition(a3));
            Assert.Equal(4, animals.GetPosition(a4));
            Assert.Equal(a3, c2.LeftAnimal);
            Assert.Equal(a2, c1.LeftAnimal);
        }

        [Fact]
        public void TestSetPositionChainChainLeftGap()
        {
            var animals = GetEightAnimals(out var a1, out var a2, out var a3, out var a4, out var a5, out var a6, out var a7, out var a8);
            var c1 = new UpgradeCommunication();
            var c2 = new UpgradeCommunication();
            animals.AddUpgrade(a5, c1);
            animals.AddUpgrade(a6, c2);

            animals.SetNewPosition(a5, 1);

            Assert.Equal(1, animals.GetPosition(a5));
            Assert.Equal(2, animals.GetPosition(a6));
            Assert.Equal(3, animals.GetPosition(a7));
            Assert.Equal(a5, c1.LeftAnimal);
            Assert.Equal(a6, c2.LeftAnimal);
        }

        [Fact]
        public void TestSetPositionChainChainRightGap()
        {
            var animals = GetEightAnimals(out var a1, out var a2, out var a3, out var a4, out var a5, out var a6, out var a7, out var a8);
            var c1 = new UpgradeCommunication();
            var c2 = new UpgradeCommunication();
            animals.AddUpgrade(a2, c1);
            animals.AddUpgrade(a3, c2);

            animals.SetNewPosition(a2, 6);

            Assert.Equal(4, animals.GetPosition(a2));
            Assert.Equal(5, animals.GetPosition(a3));
            Assert.Equal(6, animals.GetPosition(a4));
            Assert.Equal(a3, c2.LeftAnimal);
            Assert.Equal(a2, c1.LeftAnimal);
        }

        [Fact]
        public void TestSetPositionChainInvert2Animals()
        {
            var animals = GetEightAnimals(out var a1, out var a2, out var a3, out var a4, out var a5, out var a6, out var a7, out var a8);
            animals.AddUpgrade(a2, new UpgradeCommunication());

            animals.SetNewPosition(a2, 2);

            Assert.Equal(2, animals.GetPosition(a2));
            Assert.Equal(1, animals.GetPosition(a3));

            animals.SetNewPosition(a2, 1);

            Assert.Equal(1, animals.GetPosition(a2));
            Assert.Equal(2, animals.GetPosition(a3));
        }

        [Fact]
        public void TestSetPositionChainInvert3Animals()
        {
            var animals = GetEightAnimals(out var a1, out var a2, out var a3, out var a4, out var a5, out var a6, out var a7, out var a8);
            var c1 = new UpgradeCommunication();
            var c2 = new UpgradeCommunication();
            animals.AddUpgrade(a2, c1);
            animals.AddUpgrade(a3, c2);

            animals.SetNewPosition(a2, 3);

            Assert.Equal(3, animals.GetPosition(a2));
            Assert.Equal(2, animals.GetPosition(a3));
            Assert.Equal(1, animals.GetPosition(a4));
            Assert.Equal(a4, c2.LeftAnimal);
            Assert.Equal(a3, c1.LeftAnimal);

            animals.SetNewPosition(a2, 1);

            Assert.Equal(1, animals.GetPosition(a2));
            Assert.Equal(2, animals.GetPosition(a3));
            Assert.Equal(3, animals.GetPosition(a4));
        }

        [Fact]
        public void TestRemoveAnimalSimple()
        {
            var animals = GetThreeAnimals(out var a1, out var a2, out var a3);

            animals.RemoveAnimal(a1);

            Assert.DoesNotContain(a1, animals);
        }

        [Fact]
        public void TestRemoveAnimalPairUpgrade()
        {
            var animals = GetThreeAnimals(out var a1, out var a2, out var a3);
            animals.AddUpgrade(a1, new UpgradeCommunication());
            animals.AddUpgrade(a2, new UpgradeCommunication());
            animals.AddUpgrade(a2, new UpgradeCooperation());

            animals.RemoveAnimal(a2);

            Assert.DoesNotContain(a2, animals);
            Assert.Empty(a1.Upgrades);
            Assert.Empty(a3.Upgrades);
        }

        [Fact]
        public void TestRemovePairUpgradeNull()
        {
            var animals = GetThreeAnimals(out var a1, out var a2, out var a3);
            var u1 = new UpgradeCommunication();
            animals.AddUpgrade(a1, u1);
            animals.AddUpgrade(a2, new UpgradeCommunication());
            
            Assert.Throws<ArgumentNullException>(() => animals.RemoveUpgrade(null, u1));
            Assert.Throws<ArgumentNullException>(() => animals.RemoveUpgrade(a1, null));
        }

        [Fact]
        public void TestRemovePairUpgrade()
        {
            var animals = GetThreeAnimals(out var a1, out var a2, out var a3);
            var u1 = new UpgradeCommunication();
            animals.AddUpgrade(a1, u1);
            animals.AddUpgrade(a2, new UpgradeCommunication());

            animals.RemoveUpgrade(a1, u1);

            Assert.DoesNotContain(u1, a1.Upgrades);
            Assert.DoesNotContain(u1, a2.Upgrades);
        }


        #endregion //Position

    }
}