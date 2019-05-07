using System;
using System.Collections.Generic;
using System.Linq;
using Model.Upgrades;

namespace Model
{
    public class FoodToken : ObjectWithId
    {
        public FoodToken(bool fromBase) => FromBase = fromBase;

        public bool FromBase { get; }
    }



    public class PhaseConsumption : IPhase
    {
        public List<FoodToken> Food { get; } = new List<FoodToken>();


        public PhaseConsumption(int playersAmount)
        {
            var foodAmount = GenerateFood(playersAmount);
            for (int i = 0; i < foodAmount; i++)
                Food.Add(new FoodToken(true));
        }


        private int GenerateFood(int playersAmount)
        {
            var rand = new Random(DateTime.Now.Millisecond);
            switch (playersAmount)
            {
                case 2:
                    return rand.Next(1, 6) + 2;
                case 3:
                    return rand.Next(1, 6) + rand.Next(1, 6);
                case 4:
                    return rand.Next(1, 6) + rand.Next(1, 6) + 2;
                case 5:
                    return rand.Next(1, 6) + rand.Next(1, 6) + rand.Next(1, 6) + 2;
                case 6:
                    return rand.Next(1, 6) + rand.Next(1, 6) + rand.Next(1, 6) + 4;
                case 7:
                    return rand.Next(1, 6) + rand.Next(1, 6) + rand.Next(1, 6) + rand.Next(1, 6) + 2;
                case 8:
                    return rand.Next(1, 6) + rand.Next(1, 6) + rand.Next(1, 6) + rand.Next(1, 6) + 4;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


    }
}