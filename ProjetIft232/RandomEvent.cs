using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetIft232
{
    public class RandomEvent
    {
        public List<String> events= new List<String>();

        public static Dictionary<string, Tuple<Resources, Tuple<int, string>>> cost = new Dictionary<string, Tuple<Resources, Tuple<int, string>>>()
        {
            {"Meteores", Tuple.Create<Resources, Tuple<int, string>>(new Resources(1000,200,100,0,100), Tuple.Create<int, string>(3,"C'est un oiseau ! Non ! C'est un avion ! Ah non, une meteorite..."))},
            {"Zombies", Tuple.Create<Resources, Tuple<int, string>>(new Resources(0,0,1000,0,200), Tuple.Create<int, string>(0,"Les zombies attaquent ! Coureeeeeeeez !"))},
            {"Ebola", Tuple.Create<Resources, Tuple<int, string>>(new Resources(0,0,200,0,1000), Tuple.Create<int, string>(1,"Il fallait se laver les mains..."))},
            {"Extra-terrestres", Tuple.Create<Resources, Tuple<int, string>>(new Resources(100,0,0,200,200), Tuple.Create<int, string>(1,"Une soucoupe est passee et a recupere quelques ressources..."))},
            {"Chevres enragees", Tuple.Create<Resources, Tuple<int, string>>(new Resources(0,0,0,0,500), Tuple.Create<int, string>(1,"On nous attaque ! Une troupe de chevres enragees met nos vies en danger !"))},
            {"Singes malicieux", Tuple.Create<Resources, Tuple<int, string>>(new Resources(0,3000,0,0,0), Tuple.Create<int, string>(0,"Ces singes etaient mignons... Mais ils nous ont vole notre or !"))},
            {"Seisme", Tuple.Create<Resources, Tuple<int, string>>(new Resources(0,0,0,-1000,500), Tuple.Create<int, string>(2,"Ca tremble ! Ca tremble ! "))},
            {"Godzilla", Tuple.Create<Resources, Tuple<int, string>>(new Resources(500,200,500,500,200), Tuple.Create<int, string>(3,"J'ai cru voir un gros lezard se balader dans la ville..."))},
            {"Mode des bijoux en or", Tuple.Create<Resources, Tuple<int, string>>(new Resources(0,1000,0,0,0), Tuple.Create<int, string>(3,"Mon precieux..."))},
            {"Pluie feconde", Tuple.Create<Resources, Tuple<int, string>>(new Resources(0,0,-1000,0,0), Tuple.Create<int, string>(0,"Une pluie ameliorant les recoltes."))},
            {"Decouverte d'un leprechaun", Tuple.Create<Resources, Tuple<int, string>>(new Resources(0,-2000,0,0,0), Tuple.Create<int, string>(0,"On a trouve un leprechaun ! Ce petit homme faisait une drole de tete quand on a requisitionne son or ! :D"))},
            {"Incendie quantique", Tuple.Create<Resources, Tuple<int, string>>(new Resources(-1000,0,0,0,0), Tuple.Create<int, string>(0,"J'aurai jure que tout brulait..."))},
            {"Baby Boom", Tuple.Create<Resources, Tuple<int, string>>(new Resources(0,0,1000,0,-600), Tuple.Create<int, string>(0,"Tout le monde a voulu imiter des lapins, c'est bien fait pour eux."))}

        };

        public RandomEvent()
        {
            events.Add("Meteores");
            events.Add("Zombies");
            events.Add("Ebola");
            events.Add("Extra-terrestres");
            events.Add("Chevres enragees");
            events.Add("Singes malicieux");
            events.Add("Seisme");
            events.Add("Godzilla");
            events.Add("Mode des bijoux en or");
            events.Add("Pluie feconde");
            events.Add("Decouverte d'un leprechaun");
            events.Add("Incendie quantique");
            events.Add("Baby Boom");
        }
        public  String Next(City city)
        {
            Random random = new Random();

                int nombre = random.Next(0, events.Count);
                city.RemoveResources(cost[events[nombre]].Item1);
                for (int i = 0; i < cost[events[nombre]].Item2.Item1 ; i++ )
                    if(city.Buildings.Count >0)
                        city.RemoveBuilding(random.Next(0,city.Buildings.Count-1));
                return "Votre ville a subi : " + events[nombre] + "\n " + cost[events[nombre]].Item2.Item2;
        }
        
    }
}
