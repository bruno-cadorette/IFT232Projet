using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetIft232
{
    class UnicornFactory : Building
    {
        private int[] AddedResources = new int[(int)ResourceType.End];

        public UnicornFactory()
        {
            for (int i = 0; i < (int)ResourceType.End;i++)
                AddedResources[i] = 1;
        }

        private void AddResources()
        {
            
        }
    }
}
