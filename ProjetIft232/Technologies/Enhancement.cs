using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProjetIft232.Technologies
{
    [DataContract]
    public sealed class Enhancement
    {
        [DataMember]
        public Resources Resources { get; private set; }
        [DataMember]
        public int ConstructionTime { get; private set; }


        public Enhancement(Resources resources, int constructionTime)
        {
            Resources = resources;
            ConstructionTime = constructionTime;
        }

        public static Enhancement Zero()
        {
            return new Enhancement(Resources.Zero(), 0);
        }
    }
}
