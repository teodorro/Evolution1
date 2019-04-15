using System;

namespace Model.Upgrades
{

    public abstract class Upgrade : ObjectWithId, IEquatable<Upgrade>
    {
        public string Name { get; protected set; }
        public bool AppliedInSingleCopy { get; protected set; } = true;
//        public bool AppliedToCouple { get; protected set; } = false;
        public bool CanBeAppliedOtherPlayers { get; protected set; } = false;
        public bool CanBeAppliedThePlayer { get; protected set; } = true;
        public UpgradeType UpgradeType { get; protected set; }

        public abstract int AdditionalFoodNeeded { get; }



        public bool Equals(Upgrade other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return UpgradeType == other.UpgradeType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Upgrade) obj);
        }

        public override int GetHashCode()
        {
            return (int) UpgradeType;
        }
    }


    public abstract class UpgradeSingle : Upgrade
    {
    }


    public abstract class UpgradePair : Upgrade
    {
        public Animal LeftAnimal { get; set; }
        public Animal RightAnimal { get; set; }

        public bool Equals(UpgradePair other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return UpgradeType == other.UpgradeType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((UpgradePair)obj);
        }

        public override int GetHashCode()
        {
            return (int)UpgradeType;
        }
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