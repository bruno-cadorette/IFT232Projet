using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ProjetIft232.Buildings;

namespace ProjetIft232
{

    public abstract class Effect
    {
        public Effect(string desc)
        {
            description = desc;
        }
        public string description { get; private set; }

        public abstract String Apply(City city);
    }

    public class SimpleEffect : Effect
    {
        public Resources resource { get; private set; }

        public SimpleEffect(Resources rsc, string desc) : base (desc)
        {
            resource = rsc;

        }

        public override String Apply(City city)
        {
            string result = "- ";
            result += description;
            city.RemoveResources(resource);
            return result; 

        }


    }

    public class ProportionalEffect : Effect
    {
        public Resources resource { get; private set; }

        public ProportionalEffect(Resources rsc, string desc)
            : base(desc)
        {
            resource = rsc;

        }

        public override String Apply(City city)
        {
            string result = "- ";
            result += description;
            for (int i = 0; i < (int)ResourcesType.End; i++)
            {
                resource[(ResourcesType)i] = resource[(ResourcesType)i] * city.Ressources[(ResourcesType)i] / 100;
            }
            city.RemoveResources(resource);
            return result;

        }
    }

    public class BuildEffect : Effect
    {
        public int nbBuildDestroyed { get; private set; }

        public BuildEffect(int nb, string desc) 
            : base (desc)
        {
            nbBuildDestroyed = nb;

        }

        public override String Apply(City city)
        {
            Random random = new Random();
            int bati;
            string result = "- ";
            result += description;
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

    public class CombinatedEffect : Effect
    {
        private SimpleEffect se;
        private ProportionalEffect pe;
        private BuildEffect be;

        public CombinatedEffect(Resources fixe, Resources prop, int nb, string desc)
            : base (desc)
        {
            se = new SimpleEffect(fixe, "");
            pe = new ProportionalEffect(prop, "");
            be = new BuildEffect(nb,"");

        }

        public override String Apply(City city)
        {
            se.Apply(city);
            pe.Apply(city);
            String res = "- " + description + " ";
            res += be.Apply(city).Substring(1);
            return res;
        }
    }


    public class Event
    {
        public string name {get; private set;}
        public int counter { get; private set; }
        public Effect normalEffect { get; private set; }
        public Effect counterEffect { get; private set; }

        public Event(string nm,  int ct, Effect ef1, Effect ef2)
        {
            name =nm;
            counter = ct;
            normalEffect = ef1;
            counterEffect = ef2;

        }

        public Event(string nm, Effect ef1)
        {
            name =nm;
            counter = -1;
            normalEffect = ef1;
            counterEffect = null;

        }



    }

    public class RandomEvent
    {
        public Dictionary<Event, int> events = new Dictionary<Event, int>();
        //Exemple de tuple : 
        // {Meteores,((Ressources, (nombre de batiments détruits, description))}
        // {Meteores,(((Ressources, (( nombre de batiments détruits,description),(batiment requis, antidescription))))}

                                                                    
        public RandomEvent()
        {
            events.Add(new Event("Météorites", 0, new CombinatedEffect(new Resources { Wood = 400, Gold = 600, Meat = 500 }, new Resources { Population = 22 }, 3, "C'est un oiseau ! Non ! C'est un avion ! Ah non, une meteorite..."), new BuildEffect(1, "Waouh ! Sauvé par la maison ! :D")), 2);
            events.Add(new Event("Zombies", 5, new ProportionalEffect(new Resources{ Population = 35 },"Les zombies attaquent ! Coureeeeeeeez !"), new ProportionalEffect(new Resources{Population = 10},"Grâce à la caserne, les villageois on pu se défendre !")),5);
            events.Add(new Event("Ebola", 6, new ProportionalEffect(new Resources { Meat = 20, Population = 70 }, "Un ebola sauvage apparait dans les hautes herbes ! Gotta catch'em all ! :D"), new ProportionalEffect(new Resources{Population = 8}, "L'hôpital nous a tous sauvé ! Gloire à l'hôpital ! ")),1);
            events.Add(new Event("Extra-terrestres", new CombinatedEffect(Resources.Zero(), new Resources{Wood = 10, Rock = 10, Population = 2},1,"Une soucoupe est passee et a recupere quelques ressources...")),14);
            events.Add(new Event("Chevres enragees", 1, new CombinatedEffect(Resources.Zero(), new Resources { Population = 90 }, 1, "On nous attaque ! Une troupe de chevres enragees met nos vies en danger !"), new ProportionalEffect(new Resources { Meat = -50 }, "Elles ont trouvees dans la ferme la sedentarite qui leur manquait")), 5);
            events.Add(new Event("Singes malicieux",new SimpleEffect(new Resources{Gold = 40},"Ces singes etaient mignons... Mais ils nous ont vole notre or !")),48);
            events.Add(new Event("Seisme",new CombinatedEffect(Resources.Zero(),new Resources{Rock = -100 , Population = 26},2,"Ca tremble ! Ca tremble ! ")),8);
            events.Add(new Event("Godzilla", new CombinatedEffect(new Resources{ Wood = 1200, Gold = 5000, Meat = 1000, Rock = 1500},new Resources {Wood = 15, Gold = 5,Rock = 5,Meat = 29, Population = 58 }, 3, "J'ai cru voir un gros lezard se balader dans la ville...")),1);
            events.Add(new Event("Mode des bijoux en or", 7, new ProportionalEffect(new Resources{Gold = 35}, "Mon precieux..."), new SimpleEffect(Resources.Zero(), "Heureusement, le marché était là pour approvisionner la population avant qu'ils piochent dans les réserves...")),19);
            events.Add(new Event("Pluie feconde", new ProportionalEffect(new Resources{Meat = -50}, "Une pluie a amélioré les récoltes chef !")),20);
            events.Add(new Event("Decouverte d'un leprechaun", new SimpleEffect(new Resources{Gold = -5000}, "On a trouve un leprechaun ! Ce petit homme faisait une drole de tete quand on a requisitionne son or ! :D")),4);
            events.Add(new Event("Incendie quantique", new CombinatedEffect(new Resources{}, new Resources{Wood = -33},1,"J'aurai jure qu'il y avait un bâtiment ici... Pas une forêt.")),4);
            events.Add(new Event("Baby Boom", new ProportionalEffect(new Resources{Meat = 50,Population = -50},"Tout le monde a voulu imiter des lapins, c'est bien fait pour eux.")),10);
        }
        public  String Next(City city)
        {
            Random random = new Random();
            Event ev = null;

            foreach (Event e in events.Keys)
            {
                int nombre = random.Next(0, 99);
                if (nombre < events[e]) {
                    ev = e;
                    break;
                }
            }
            if (ev != null) { 
                string result = "Votre ville a subi : " + ev.name + "\n";
                if (!city.IsBuilt((int)ev.counter))
                {

                    return result + ev.normalEffect.Apply(city);
                 }
                 else
                {
                  return result + ev.counterEffect.Apply(city);
                }
           }
            return "Vous avez eu peur hein? Ben il ne s'est rien passe...\n"; 
        }
        
    }
}
