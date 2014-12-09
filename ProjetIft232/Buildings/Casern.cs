using System.Runtime.Serialization;

namespace ProjetIft232.Buildings
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