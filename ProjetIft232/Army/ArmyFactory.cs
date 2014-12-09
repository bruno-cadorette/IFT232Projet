using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ProjetIft232.Army
{
    public class ArmyFactory : Factory
    {
        private static ArmyFactory instance;

        private static readonly object SyncRoot = new object();


        protected override IEnumerable<XElement> GetChilds(XElement root)
        {
            return root.Element(XName.Get("Soldiers")).Elements(XName.Get("Soldier"));
        }

        public ArmyUnit GetSoldier(int type)
        {
            return new ArmyUnit((ArmyUnit) _entities[type]);
        }

        public IEnumerable<ArmyUnit> Soldiers()
        {
            return _entities.Select(x => (ArmyUnit) x.Value);
        }


        protected override BuildableEntity CreateEntity(XElement element, int id, string name, string description,
            int turns, Requirement requirement)
        {
            Resources resources = GetResources(element);
            string specialType = Special(element);
            int Attack = int.Parse(GetAttribute(element, "Attack"));
            int Defense = int.Parse(GetAttribute(element, "Defense"));
            int Size = int.Parse(GetAttribute(element, "size"));
            ArmyUnit army = new ArmyUnit(id, name, description, Attack, Defense, Size, turns, resources, requirement);

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

        public static ArmyUnit CreateArmyUnit(int id, City city)
        {
            ArmyUnit unit = GetInstance().GetSoldier(id);
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

        public static ArmyUnit CreateBarbarian(int id)
        {
            return GetInstance().GetSoldier(id);
        }
    }
}