using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ProjetIft232
{
    //TODO: Changer pour une classe
    public enum Resources
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
        private const int MAX_POP = 500000;
        private const int RESOURCE_PER_TURN = 5;
        public int Wood { get; private set; }

        public int Gold { get; private set; }

        public int Meat { get; private set; }

        public int Rock { get; private set; }

        public int Population { get; private set; }
        public Resource ()
        {
            Wood = 0;
            Gold = 0;
            Meat = 0;
            Rock = 0;
            Population = 0;
        }

        public Resource(Resource a)
        {
            Wood = a.Wood;
            Gold = a.Gold;
            Meat = a.Meat;
            Rock = a.Rock;
            Population = a.Population;
        }

        public Resource(Dictionary<Resources, int> resources)
            :this()
        {
            foreach (var resource in resources)
            {
                if (resource.Value < 0)
                    break;

                switch (resource.Key)
                {
                    case Resources.Wood:
                        Wood = resource.Value;
                        break;
                    case Resources.Rock:
                        Rock = resource.Value;
                        break;
                    case Resources.Population:
                        Population = resource.Value;
                        break;
                    case Resources.Meat:
                        Meat = resource.Value;
                        break;
                    case Resources.Gold:
                        Gold = resource.Value;
                        break;
                }
            }
        }

        public static Resource operator+(Resource debut, Resource b)
        {
            Resource a = new Resource(debut);
            a.Wood += b.Wood;
            a.Rock += b.Rock;
            a.Population += b.Population;
            a.Meat += b.Meat;
            a.Gold += b.Gold;

            return a;
        }



        public static Resource operator -(Resource debut, Resource b)
        {
            Resource a = new Resource(debut);
            a.Wood -= b.Wood;
            a.Rock -= b.Rock;
            a.Population -= b.Population;
            a.Meat -= b.Meat;
            a.Gold -= b.Gold;

            return a;
        }

        public static bool operator <(Resource a, Resource b)
        {
            return a.Gold < b.Gold && a.Meat < b.Meat && a.Population < b.Population && a.Rock < b.Rock &&
                   a.Wood < b.Wood;
        }

        public static bool operator <=(Resource a, Resource b)
        {
            return a.Gold <= b.Gold && a.Meat <= b.Meat && a.Population <= b.Population && a.Rock <= b.Rock &&
                   a.Wood <= b.Wood;
        }

        public static bool operator >(Resource a, Resource b)
        {
            return a.Gold > b.Gold && a.Meat > b.Meat && a.Population > b.Population && a.Rock > b.Rock &&
                   a.Wood > b.Wood;
        }

        public static bool operator >=(Resource a, Resource b)
        {
            return a.Gold >= b.Gold && a.Meat >= b.Meat && a.Population >= b.Population && a.Rock >= b.Rock &&
                   a.Wood >= b.Wood;
        }

        public static bool operator ==(Resource a, Resource b)
        {
            return a.Gold == b.Gold && a.Meat == b.Meat && a.Population == b.Population && a.Rock == b.Rock &&
                   a.Wood == b.Wood;
        }

        public static bool operator !=(Resource a, Resource b)
        {
            return a.Gold != b.Gold || a.Meat != b.Meat || a.Population != b.Population || a.Rock != b.Rock ||
                   a.Wood != b.Wood;
        }

        public override string ToString()
        {
            return string.Format("Bois : {0} \t Or : {1} \t Viande : {2} \t Pierre : {3} \t Population : {4} \n", Wood, Gold,
                Meat, Rock, Population);
        }

        public void Update(Resource a)
        {
            this.Meat += (a.Meat+1)*RESOURCE_PER_TURN;
            this.Gold += (a.Gold + 1) * RESOURCE_PER_TURN;
            this.Wood += (a.Wood + 1) * RESOURCE_PER_TURN;
            this.Rock += (a.Rock + 1) * RESOURCE_PER_TURN;
            this.Population += Convert.ToInt32(a.Population+1+Game.TourIndex*0.1);
        }


        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is Resource))
                return false;
            Resource temp = obj as Resource;
            return this == temp;
        }
    }
}
