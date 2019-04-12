using System;
using Model.Upgrades;

namespace Model
{
    public class Card : ObjectWithId
    {
        public Upgrade Upgrade1 { get; }
        public Upgrade Upgrade2 { get; }


        public Card(Upgrade upgrade1, Upgrade upgrade2)
        {
            Upgrade1 = upgrade1 ?? throw new ArgumentNullException("upgrade1 == null");
            Upgrade2 = upgrade2;
        }
    }
}