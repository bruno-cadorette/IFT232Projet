using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

    public sealed class Resource
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

    public class Resources
    {
        private const int MAX_POP = 500000;

        private Dictionary<ResourcesType, int> resources;

        public Resources ()
        {
            resources = new Dictionary<ResourcesType,int>();
        }
        public Resources(int wood, int gold, int meat, int rock, int population) : this()
        {
            resources.Add(ResourcesType.Wood, wood);
            resources.Add(ResourcesType.Gold, gold);
            resources.Add(ResourcesType.Meat, meat);
            resources.Add(ResourcesType.Rock, rock);
            resources.Add(ResourcesType.Population, population);
           
        }


        public Resources(Resources a) : this()
        {
            foreach (KeyValuePair<ResourcesType, int> kvp in a.resources)
            {
                this.resources.Add(kvp.Key, kvp.Value);
            }
        }

        public Resources(Dictionary<ResourcesType, int> resources)
            :this()
        {
            this.resources = resources;
        }

        public bool isEmpty()
        {
            return(resources.Count() == 0);
        }

        public int get(string resource) {
            ResourcesType r = Resource.Name.FirstOrDefault(x => x.Value == resource).Key;
            int value;
            resources.TryGetValue(r, out value);
            return value;
        }

        
        public int this[ResourcesType i]
{
    get { return resources[i]; }
    set { resources[i] = (int)value; }
}

        public static Resources Zero()
        {
            return new Resources();
        }

        public static Resources operator+(Resources debut, Resources b)
        {
            int old;
            Resources a = new Resources(debut);
            foreach (KeyValuePair<ResourcesType, int> kvp in b.resources)
            {
                if (a.resources.TryGetValue(kvp.Key, out old))
                {
                    a.resources[kvp.Key] = old + kvp.Value;
                }
                else a.resources.Add(kvp.Key, kvp.Value);
            }
            return a;
        }



        public static Resources operator -(Resources debut, Resources b)
        {
            int old;
            Resources a = new Resources(debut);
            foreach (KeyValuePair<ResourcesType, int> kvp in b.resources)
            {
                if (a.resources.TryGetValue(kvp.Key, out old))
                {
                    a.resources[kvp.Key] = old - kvp.Value;
                }
                else a.resources.Add(kvp.Key, -kvp.Value);
                if (a.resources[kvp.Key] < 0)
                    a.resources[kvp.Key] = 0;
            }
            return a;
        }

        public static bool operator <(Resources a, Resources b)
        {
            int old;
            if (b.isEmpty())
                return false;
            foreach (KeyValuePair<ResourcesType, int> kvp in b.resources)
            {
                if (a.resources.TryGetValue(kvp.Key, out old))
                {
                    if (old >= kvp.Value)
                        return false;
                }
                else return true;
            }
            return true;
        }

        public static bool operator <=(Resources a, Resources b)
        {
            int old;
            if (b.isEmpty() && !a.isEmpty())
                return false;
            foreach (KeyValuePair<ResourcesType, int> kvp in b.resources)
            {
                if (a.resources.TryGetValue(kvp.Key, out old))
                {
                    if (old > kvp.Value)
                        return false;
                }
                else return true;
            }
            return true;
        }

        public static bool operator >(Resources a, Resources b)
        {
            int old;
            if (a.isEmpty())
                return false;
            foreach (KeyValuePair<ResourcesType, int> kvp in b.resources)
            {
                if (a.resources.TryGetValue(kvp.Key, out old))
                {
                    if (old <= kvp.Value)
                        return false;
                }
                else
                {
                    if (kvp.Value != 0) 
                        return false;
                }
            }
            return true;
        }

        public static bool operator >=(Resources a, Resources b)
        {
            int old;
            if (a.isEmpty() && !b.isEmpty())
                return false;
            foreach (KeyValuePair<ResourcesType, int> kvp in b.resources)
            {
                if (a.resources.TryGetValue(kvp.Key, out old))
                {
                    if (old < kvp.Value)
                        return false;
                }
                else
                {
                    if (kvp.Value != 0)
                        return false;
                }
            }
            return true;
        }

        public static bool operator ==(Resources a, Resources b)
        {
            int old;
            foreach (KeyValuePair<ResourcesType, int> kvp in b.resources)
            {
                if (a.resources.TryGetValue(kvp.Key, out old))
                {
                    if (old != kvp.Value)
                        return false;
                }
                else
                {
                    if (kvp.Value != 0)
                        return false;
                }
            }
            foreach (KeyValuePair<ResourcesType, int> kvp in a.resources)
            {
                if (b.resources.TryGetValue(kvp.Key, out old))
                {
                    if (old != kvp.Value)
                        return false;
                }
                else
                {
                    if (kvp.Value != 0)
                        return false;
                }
            }
            return true;
        }

        public static bool operator !=(Resources a, Resources b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            string format = "";
            foreach (KeyValuePair<ResourcesType, int> kvp in this.resources)
            {
                format = format + Resource.Name[kvp.Key] + " : " + kvp.Value.ToString() + "\n";
            }
            return format;
        }

        public void Update(Resources a)
        {
            int old;
            foreach (KeyValuePair<ResourcesType, int> kvp in a.resources)
            {
                if (this.resources.TryGetValue(kvp.Key, out old))
                {
                    this.resources[kvp.Key] = old + kvp.Value;
                }
                else this.resources.Add(kvp.Key, kvp.Value);
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
