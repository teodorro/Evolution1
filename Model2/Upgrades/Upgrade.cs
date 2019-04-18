using System;

namespace Model.Upgrades
{

    public abstract class Upgrade : ObjectWithId
    {
        public string Name { get; protected set; }
        public bool AppliedInSingleCopy { get; protected set; } = true;
//        public bool AppliedToCouple { get; protected set; } = false;
        public bool CanBeAppliedOtherPlayers { get; protected set; } = false;
        public bool CanBeAppliedThePlayer { get; protected set; } = true;
        public UpgradeType UpgradeType { get; protected set; }

        public abstract int AdditionalFoodNeeded { get; }

        
    }


    public abstract class UpgradeSingle : Upgrade
    {
        public Animal Animal { get; set; }
    }

    public interface IUsable
    {
        void Use();
    }


    public abstract class UpgradePair : Upgrade
    {
        public Animal LeftAnimal { get; set; }
        public Animal RightAnimal { get; set; }

    }




    public enum UpgradeType
    {
        Poisonous,
        Carnivorous,
        Running,
        HighBodyWeight,
        Swimming,
        Burrowing,
        Fat,
        TailLoss,
        Piracy,
        Hibernation,
        SharpVision,
        Camouflage,
        Mimicry,
        Scavanger,
        Grazing,
        Symbiosys,
        Parasite,
        Cooperation,
        Communication
    }
}