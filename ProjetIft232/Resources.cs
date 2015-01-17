using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Core
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

    //A enlever parce que c'est fucking laid
    public class Resource
    {
        public static Dictionary<ResourcesType, string> Name = new Dictionary<ResourcesType, string>
        {
            {ResourcesType.Wood, "Wood"},
            {ResourcesType.Gold, "Gold"},
            {ResourcesType.Meat, "Meat"},
            {ResourcesType.Rock, "Rock"},
            {ResourcesType.Population, "Population"},
        };

        // Plus la valeur est basse et plus la ressource est precieuse
        public static Dictionary<ResourcesType, int> Rarity = new Dictionary<ResourcesType, int>
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
        private readonly int[] _resources;

        public Resources()
        {
            _resources = new int[(int)ResourcesType.End];
        }

        public Resources(ResourcesType type, int qty)
            : this()
        {
            _resources = new int[(int)ResourcesType.End];
            _resources[(int)type] = qty;
        }

        public Resources(Resources a)
            : this()
        {
            for (int i = 0; i < (int)ResourcesType.End; i++)
            {
                _resources[i] = a._resources[i];
            }
        }

        public Resources(Dictionary<ResourcesType, int> resources)
            : this()
        {
            foreach (var resource in resources)
            {
                _resources[(int)(resource.Key)] = resource.Value;
            }
        }

        public int Gold
        {
            get { return _resources[(int)ResourcesType.Gold]; }
            set { _resources[(int)ResourcesType.Gold] = value; }
        }

        public int Meat
        {
            get { return _resources[(int)ResourcesType.Meat]; }
            set { _resources[(int)ResourcesType.Meat] = value; }
        }

        public int Rock
        {
            get { return _resources[(int)ResourcesType.Rock]; }
            set { _resources[(int)ResourcesType.Rock] = value; }
        }

        public int Population
        {
            get { return _resources[(int)ResourcesType.Population]; }
            set { _resources[(int)ResourcesType.Population] = value; }
        }

        public int Wood
        {
            get { return _resources[(int)ResourcesType.Wood]; }
            set { _resources[(int)ResourcesType.Wood] = value; }
        }


        public int this[ResourcesType i]
        {
            get { return _resources[(int)i]; }
            set { _resources[(int)i] = value; }
        }

        protected bool Equals(Resources other)
        {
            return _resources.SequenceEqual(other._resources);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Resources)obj);
        }

        public override int GetHashCode()
        {
            return (_resources != null ? _resources.GetHashCode() : 0);
        }

        public bool isEmpty()
        {
            return (_resources.Count() == 0);
        }

        public static Resources Zero()
        {
            return new Resources();
        }

        public ResourcesType GetLowest()
        {
            int index = 0;
            int min = int.MaxValue;
            for (int i = 0; i < (int)ResourcesType.End; i++)
            {
                if (_resources[i] < min)
                {
                    index = i;
                    min = _resources[i];
                }
            }
            return (ResourcesType)index;

        }

        public static Resources operator +(Resources debut, Resources b)
        {
            Resources ress = Zero();
            for (int i = 0; i < (int)ResourcesType.End; i++)
            {
                ress._resources[i] = debut._resources[i] + b._resources[i];
            }
            return ress;
        }


        public static Resources operator -(Resources debut, Resources b)
        {
            Resources ress = Zero();
            for (int i = 0; i < (int)ResourcesType.End; i++)
            {
                ress._resources[i] = debut._resources[i] - b._resources[i];
            }
            return ress;
        }

        public static bool operator <(Resources a, Resources b)
        {
            for (int i = 0; i < (int)ResourcesType.End; i++)
            {
                if (a._resources[i] >= b._resources[i])
                    return false;
            }
            return true;
        }

        public void Abs()
        {
            for (int i = 0; i < (int)ResourcesType.End; i++)
            {
                if (_resources[i] < 0)
                    _resources[i] = 0;
            }
        }

        public static bool operator <=(Resources a, Resources b)
        {
            for (int i = 0; i < (int)ResourcesType.End; i++)
            {
                if (a._resources[i] > b._resources[i])
                    return false;
            }
            return true;
        }

        public static bool operator >(Resources a, Resources b)
        {
            for (int i = 0; i < (int)ResourcesType.End; i++)
            {
                if (a._resources[i] <= b._resources[i])
                    return false;
            }
            return true;
        }

        public static bool operator >=(Resources a, Resources b)
        {
            for (int i = 0; i < (int)ResourcesType.End; i++)
            {
                if (a._resources[i] < b._resources[i])
                    return false;
            }
            return true;
        }

        public static bool operator ==(Resources a, Resources b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Resources a, Resources b)
        {
            return !a.Equals(b);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < (int)ResourcesType.End; i++)
            {
                sb.Append(string.Format(" {0} : {1} ", (ResourcesType)i, _resources[i]));
            }
            return sb.ToString();
        }
        public static Resources operator*(Resources a, int n)
        {
            return new Resources()
            {
                Gold = a.Gold * n,
                Meat = a.Meat * n,
                Wood = a.Wood * n,
                Rock = a.Rock * n,
                Population = a.Population * n
            };
        }

        public void Update(Resources a)
        {
            for (int i = 0; i < (int)ResourcesType.End; i++)
            {
                _resources[i] += a._resources[i];
            }
        }
    }
}