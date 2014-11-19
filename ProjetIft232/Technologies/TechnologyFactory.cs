using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetIft232.Technologies
{
    public class TechnologyFactory
    {
        public static Technology CreateTechnology()
        {
            return GetTechnology(0);
        }

        private static Technology GetTechnology(int type)
        {
            return new Technology();

        }
    }
}
