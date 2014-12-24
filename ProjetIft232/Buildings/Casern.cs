using System.Runtime.Serialization;

namespace Core.Buildings
{
    [DataContract]
    public class Casern : Building
    {
        public Casern(Building building)
            : base(building)
        {
        }
    }
}