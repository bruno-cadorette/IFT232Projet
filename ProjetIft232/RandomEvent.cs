using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetIft232.Buildings;

namespace ProjetIft232
{
    public class Effect
    {
        public Resources resource { get; private set; }
        public int nbBuildDestroyed { get; private set; }
        public string description { get; private set; }

        public Effect(Resources rsc, int nb, string desc)
        {
            resource = rsc;
            nbBuildDestroyed = nb;
            description = desc;

        }
        public Effect(int nb, string desc)
        {
            resource = new Resources(0, 0, 0, 0, 0);
            nbBuildDestroyed = nb;
            description = desc;

        }

        public Effect(Resources rsc, string desc)
        {
            resource = rsc;
            nbBuildDestroyed = 0;
            description = desc;

        }

        public string Apply(City city)
        {
            Random random = new Random();
            int bati;
            string result = "- ";
            result += description;
            city.RemoveResources(resource);
            for (int i = 0; i < nbBuildDestroyed; i++)
                if (city.Buildings.Count > 0)
                {
                    bati = random.Next(0, city.Buildings.Count - 1);
                    result += "\n" + city.Buildings[bati].Name + " a été détruit.";
                    city.RemoveBuilding(bati);
                }
            return result; 

        }


    }

    public class Event
    {
        public string name {get; private set;}
        public BuildingType counter { get; private set; }
        public Effect normalEffect { get; private set; }
        public Effect counterEffect { get; private set; }

        public Event(string nm,  BuildingType ct, Effect ef1, Effect ef2)
        {
            name =nm;
            counter = ct;
            normalEffect = ef1;
            counterEffect = ef2;

        }

        public Event(string nm, Effect ef1)
        {
            name =nm;
            counter = BuildingType.Null;
            normalEffect = ef1;
            counterEffect = null;

        }



    }

    public class RandomEvent
    {
        public List<Event> events= new List<Event>();
        //Exemple de tuple : 
        // {Meteores,((Ressources, (nombre de batiments détruits, description))}
        // {Meteores,(((Ressources, (( nombre de batiments détruits,description),(batiment requis, antidescription))))}
        public static Dictionary<string, Event> cost = new Dictionary<string, Event>()
        {
   /*         {"Zombies", Tuple.Create<Resources, Tuple<int, string>>(new Resources(0,0,1000,0,200), Tuple.Create<int, string>(0,"Les zombies attaquent ! Coureeeeeeeez !"))},
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
   */
        };

        public RandomEvent()
        {
            events.Add(new Event("Météorites", BuildingType.House,new Effect(new Resources(1000, 200, 100, 0, 100),3,"C'est un oiseau ! Non ! C'est un avion ! Ah non, une meteorite..."), new Effect(1,"Waouh ! Sauvé par la maison ! :D")));
            events.Add(new Event("Zombies", BuildingType.Casern, new Effect(new Resources(0,0,1000,0,200),0, "Les zombies attaquent ! Coureeeeeeeez !"), new Effect(new Resources(0,0,200,0,10),0,"Grâce à la caserne, les villageois on pu se défendre !")));
            events.Add(new Event("Ebola", BuildingType.Hospital, new Effect(new Resources(0,0,200,0,1000),1,"Un ebola sauvage apparait dans les hautes herbes ! Gotta catch'em all ! :D"),new Effect(new Resources(0,0,0,0,15),"L'hôpital nous a tous sauvé ! Gloire à l'hôpital ! ")));
            events.Add(new Event("Extra-terrestres", new Effect(new Resources(100,0,0,200,200),1,"Une soucoupe est passee et a recupere quelques ressources...")));
            events.Add(new Event("Chevres enragees", BuildingType.Farm, new Effect(new Resources(0, 0, 0, 0, 500), 1, "On nous attaque ! Une troupe de chevres enragees met nos vies en danger !"),new Effect(new Resources(0,0,-500,0,0),"Elles ont trouvees dans la ferme la sedentarite qui leur manquait")));
            events.Add(new Event("Singes malicieux",new Effect(new Resources(0,3000,0,0,0),"Ces singes etaient mignons... Mais ils nous ont vole notre or !")));
            events.Add(new Event("Seisme",new Effect(new Resources(0,0,0,-1000,500),2,"Ca tremble ! Ca tremble ! ")));
            events.Add(new Event("Godzilla",new Effect(new Resources(500,200,500,500,200),3,"J'ai cru voir un gros lezard se balader dans la ville...")));
            events.Add(new Event("Mode des bijoux en or", BuildingType.Market, new Effect(new Resources(0, 1000, 0, 0, 0), 0, "Mon precieux..."), new Effect(new Resources(0, 0, 0, 0, 0), "Heureusement, le marché était là pour approvisionner la population avant qu'ils piochent dans les réserves...")));
            events.Add(new Event("Pluie feconde", new Effect(new Resources(0, 0, -1000, 0, 0), 0, "Une pluie a amélioré les récoltes chef !")));
            events.Add(new Event("Decouverte d'un leprechaun", new Effect(new Resources(0, -2000, 0, 0, 0), 0, "On a trouve un leprechaun ! Ce petit homme faisait une drole de tete quand on a requisitionne son or ! :D")));
            events.Add(new Event("Incendie quantique", new Effect(new Resources(-1000, 0, 0, 0, 0),1,"J'aurai jure qu'il y avait un bâtiment ici... Pas une forêt.")));
            events.Add(new Event("Baby Boom", new Effect(new Resources(0,0,1000,0,-600),0,"Tout le monde a voulu imiter des lapins, c'est bien fait pour eux.")));
        }
        public  String Next(City city)
        {
            Random random = new Random();
            

            int nombre = random.Next(0, events.Count);
            string result = "Votre ville a subi : " + events[nombre].name + "\n";
            if (!city.IsBuilt(events[nombre].counter))
            {

                return result + events[nombre].normalEffect.Apply(city);
           }
           else
           {
              return result + events[nombre].counterEffect.Apply(city);
           }
        }
        
    }
}
