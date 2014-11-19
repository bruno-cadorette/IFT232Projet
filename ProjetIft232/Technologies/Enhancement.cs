using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetIft232.Technologies
{
    public sealed class Enhancement
    {
        public Resources Resources { get; private set; }
        public int ConstructionTime { get; private set; }


        public Enhancement(Resources resources, int constructionTime)
        {
            Resources = resources;
            ConstructionTime = constructionTime;
        }
    }
}
