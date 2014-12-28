using System.Runtime.Serialization;
using Core.Military;

namespace Core.Technologies
{
    [DataContract]
    public sealed class Enhancement
    {
        public Enhancement(Resources resources, SoldierAttributes soldierAttributes, int constructionTime)
        {
            Resources = resources;
            SoldierAttributes = soldierAttributes;
            ConstructionTime = constructionTime;
        }

        [DataMember]
        public Resources Resources { get; private set; }

        [DataMember]
        public int ConstructionTime { get; private set; }

        [DataMember]
        public SoldierAttributes SoldierAttributes { get; private set; }


        public static Enhancement Zero()
        {
            return new Enhancement(Resources.Zero(), new SoldierAttributes(), 0);
        }
    }
}