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
        private static AnimalCollection GetThreeAnimals(out Animal a1, out Animal a2, out Animal a3)
        {
            var animals = new AnimalCollection();
            a1 = animals.AddAnimal();
            a2 = animals.AddAnimal();
            a3 = animals.AddAnimal();
            return animals;
        }



        [Fact]
        public void TestAnimalsList()
        {
            var animals = new AnimalCollection();

            animals.AddAnimal();
            Assert.Single(animals.Animals);

            animals.RemoveAnimal(animals.Animals.First());
            Assert.Empty(animals.Animals);
            animals.RemoveAnimal(new Animal());
            Assert.Empty(animals.Animals);

            animals.AddAnimal();
            animals.AddAnimal();
            animals.Reset();
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

            Assert.Throws<AnimalNotFoundException>(() => animals.CanBeUpgraded(new Animal(), new UpgradeCommunication()));
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
            Assert.Throws<AnimalNotFoundException>(() => animals.GetPosition(new Animal()));
        }
        

        [Fact]
        public void TestCanBeMovedToPositionPlainErrors()
        {
            var animals = GetThreeAnimals(out var a1, out var a2, out var a3);

            Assert.Throws<ArgumentNullException>(() => animals.CanBeMovedToPosition(null, 1));
            Assert.Throws<AnimalNotFoundException>(() => animals.CanBeMovedToPosition(new Animal(), 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => animals.CanBeMovedToPosition(a2, 3));
        }

        [Fact]
        public void TestCanBeMovedToPositionNoPairCards()
        {
            var animals = GetThreeAnimals(out var a1, out var a2, out var a3);

            Assert.True(animals.CanBeMovedToPosition(a2, 0));
        }

        [Fact]
        public void TestCanBeMovedToPositionBetweenCards()
        {
            var animals = GetThreeAnimals(out var a1, out var a2, out var a3);
            animals.AddUpgrade(a1, new UpgradeCommunication());

            Assert.False(animals.CanBeMovedToPosition(a3, 1));
        }

        [Fact]
        public void TestCanBeMovedToPositionOverLeft()
        {
            var animals = GetThreeAnimals(out var a1, out var a2, out var a3);
            animals.AddUpgrade(a1, new UpgradeCommunication());

            Assert.False(animals.CanBeMovedToPosition(a2, 0));
        }

        [Fact]
        public void TestCanBeMovedToPositionOverEnd()
        {
            var animals = GetThreeAnimals(out var a1, out var a2, out var a3);
            animals.AddUpgrade(a2, new UpgradeCommunication());

            Assert.False(animals.CanBeMovedToPosition(a2, 2));
        }

        [Fact]
        public void TestCanBeMovedToPositionOneMoveMany()
        {
            var animals = GetThreeAnimals(out var a1, out var a2, out var a3);
            animals.AddUpgrade(a2, new UpgradeCommunication());

            Assert.True(animals.CanBeMovedToPosition(a1, 2));
        }

        [Fact]
        public void TestCanBeMovedToPositionManyMoveMany()
        {
            var animals = GetThreeAnimals(out var a1, out var a2, out var a3);
            var a4 = animals.AddAnimal();
            var a5 = animals.AddAnimal();
            animals.AddUpgrade(a1, new UpgradeCommunication());
            animals.AddUpgrade(a3, new UpgradeCommunication());

            Assert.True(animals.CanBeMovedToPosition(a1, 2));
        }

        #endregion //Position

    }
}