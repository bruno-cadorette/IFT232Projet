using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Core.Military
{
    public class ArmyFactory : BuildableEntityFactory
    {
        private static ArmyFactory instance;

        private static readonly object SyncRoot = new object();


        protected override IEnumerable<XElement> GetChilds(XElement root)
        {
            return root.Element(XName.Get("Soldiers")).Elements(XName.Get("Soldier"));
        }

        public Soldier GetSoldier(int type)
        {
            return new Soldier((Soldier) _entities[type]);
        }

        public IEnumerable<Soldier> Soldiers()
        {
            return _entities.Select(x => (Soldier) x.Value);
        }


        protected override BuildableEntity CreateEntity(XElement element, int id, string name, string description,
            int turns, Requirement requirement)
        {
            Resources resources = GetResources(element);
            string specialType = Special(element);
            int attack = int.Parse(GetAttribute(element, "Attack"));
            int defense = int.Parse(GetAttribute(element, "Defense"));
            int health = int.Parse(GetAttribute(element, "Health")); 
            Soldier army = new Soldier(id, name, description, attack, defense, health, turns, resources, requirement);

            switch (specialType)
            {
                case "General":
                default:
                    return army;
            }
        }

        public static ArmyFactory GetInstance()
        {
            if (instance == null)
            {
                lock (SyncRoot)
                {
                    if (instance == null)
                    {
                        instance = new ArmyFactory();
                    }
                }
            }
            return instance;
        }

        public static Soldier CreateArmyUnit(int id, City city)
        {
            Soldier unit = GetInstance().GetSoldier(id);
            if (unit != null)
            {
                foreach (
                    var tech in
                        city.ResearchedTechnologies.Where(
                            x => !x.InConstruction && x.AffectedSoldiers.Any(soldier => soldier == id)))
                {
                    unit.Upgrade(tech);
                }
                if (unit.CanBeBuild(city.Ressources, city.Buildings, city.ResearchedTechnologies))
                {
                    return unit;
                }
            }
            return null;
        }

        public static Soldier CreateBarbarian()
        {
            return GetInstance().Soldiers().FirstOrDefault();
        }
    }
}