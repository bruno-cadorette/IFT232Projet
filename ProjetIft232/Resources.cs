using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.AccessControl;
using System.Text;

namespace ProjetIft232
{
    //TODO: Changer pour une classe
    public enum ResourcesType
    {
        Wood,
        Gold,
        Meat,
        Rock,
        Population,
        End
    }

    public class Resource
    {
        public static Dictionary<ResourcesType, string> Name = new Dictionary<ResourcesType, string>()
        {
            {ResourcesType.Wood, "Wood"},
            {ResourcesType.Gold, "Gold"},
            {ResourcesType.Meat, "Meat"},
            {ResourcesType.Rock, "Rock"},
            {ResourcesType.Population, "Population"},
        };

        // Plus la valeur est basse et plus la ressource est precieuse
        public static Dictionary<ResourcesType, int> Rarity = new Dictionary<ResourcesType, int>()
        {
            {ResourcesType.Wood, 12},
            {ResourcesType.Gold, 1},
            {ResourcesType.Meat, 7},
            {ResourcesType.Rock, 10},
            {ResourcesType.Population, 0},
        };
    }

    [DataContract]
    public class Resources
    {
        [DataMember]
        private int[] resources;

        public Resources ()
        {
            resources = new int[(int)ResourcesType.End];
        }

        public Resources(ResourcesType type, int qty)
            : this()
        {
            resources = new int[(int)ResourcesType.End];
            resources[(int) type] = qty;
        }

        public Resources(int wood, int gold, int meat, int rock, int population) : this()
        {
            resources[(int)ResourcesType.Wood] =  wood;
            resources[(int)ResourcesType.Gold]=  gold;
            resources[(int)ResourcesType.Meat] = meat;
            resources[(int)ResourcesType.Rock] = rock;
            resources[(int)ResourcesType.Population] = population;
           
        }


        public Resources(Resources a) : this()
        {
            for (int i = 0; i < (int)ResourcesType.End; i++)
            {
                resources[i] = a.resources[i];
            }
        }

        public Resources(Dictionary<ResourcesType, int> resources)
            :this()
        {
            foreach (var resource in resources)
            {
                this.resources[(int)(resource.Key)] = resource.Value;
            }
        }

        public bool isEmpty()
        {
            return(resources.Count() == 0);
        }

        public int get(string resource) {
            ResourcesType r = Resource.Name.FirstOrDefault(x => x.Value == resource).Key;
            return resources[(int) r];
        }

        
        public int this[ResourcesType i]
{
    get { return resources[(int)i]; }
    set { resources[(int)i] = (int)value; }
}

        public static Resources Zero()
        {
            return new Resources();
        }

        public static Resources operator+(Resources debut, Resources b)
        {
            Resources ress = Zero();
            for (int i = 0; i < (int)ResourcesType.End; i++)
            {
                ress.resources[i] = debut.resources[i] + b.resources[i];
            }
            return ress;
        }



        public static Resources operator -(Resources debut, Resources b)
        {
            Resources ress = Zero();
            for (int i = 0; i < (int)ResourcesType.End; i++)
            {
                ress.resources[i] = debut.resources[i] - b.resources[i];
            }
            return ress;
        }

        public static bool operator <(Resources a, Resources b)
        {
            for (int i = 0; i < (int)ResourcesType.End; i++)
            {
                if (a.resources[i] >= b.resources[i])
                    return false;

            }
            return true;
        }

        public void Abs()
        {
            for (int i = 0; i < (int)ResourcesType.End; i++)
            {
                if (resources[i] < 0)
                    resources[i] = 0;

            }
        }

        public static bool operator <=(Resources a, Resources b)
        {
            for (int i = 0; i < (int)ResourcesType.End; i++)
            {
                if (a.resources[i] > b.resources[i])
                    return false;

            }
            return true;
        }

        public static bool operator >(Resources a, Resources b)
        {
            bool greaterKnow = false;
            for (int i = 0; i < (int)ResourcesType.End; i++)
            {
                if (a.resources[i] < b.resources[i])
                    return false;
                if (a.resources[i] > b.resources[i])
                    greaterKnow = true;
            }
            return greaterKnow;
        }

        public static bool operator >=(Resources a, Resources b)
        {
            for (int i = 0; i < (int)ResourcesType.End; i++)
            {
                if (a.resources[i] < b.resources[i])
                    return false;

            }
            return true;
        }

        public static bool operator ==(Resources a, Resources b)
        {
            for (int i = 0; i < (int)ResourcesType.End; i++)
            {
                if (a.resources[i] != b.resources[i])
                    return false;
            }
            return true;
        }

        public static bool operator !=(Resources a, Resources b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < (int)ResourcesType.End; i++)
            {
                sb.Append(string.Format(" {0} : {1} ", (ResourcesType) i, resources[i]));
            }
            return sb.ToString();
        }

        public void Update(Resources a)
        {
            for (int i = 0; i < (int)ResourcesType.End; i++)
            {
                resources[i] += a.resources[i];
            }
        }


        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is Resources))
                return false;
            Resources temp = obj as Resources;
            return this == temp;
        }
    }
}
