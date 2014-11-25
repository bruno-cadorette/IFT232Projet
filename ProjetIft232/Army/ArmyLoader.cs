using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ProjetIft232.Army
{
    public class ArmyLoader : Loader
    {
        private static ArmyLoader instance;

        private static object SyncRoot = new object();


        protected override IEnumerable<XElement> GetChilds(XElement root)
        {
            return root.Element(XName.Get("Soldiers")).Elements(XName.Get("Soldier"));
        }

        public ArmyUnit GetSoldier(int type)
        {
            return new ArmyUnit((ArmyUnit)_entities[type]);
        }

        public IEnumerable<ArmyUnit> Soldiers()
        {
            return _entities.Select(x => (ArmyUnit)x.Value);
        }

        public IEnumerable<ArmyUnit> ArmyUnit()
        {
            return _entities.Select(x => (ArmyUnit)x.Value);
        }

        protected override BuildableEntity CreateEntity(XElement element, int id, string name, string description, int turns, Requirement requirement)
        {
            Resources resources = GetResources(element);
            string specialType = Special(element);
            int Attack = int.Parse(GetAttribute(element, "Attack"));
            int Defense = int.Parse(GetAttribute(element, "Defense"));
            int Size = int.Parse(GetAttribute(element, "size"));
            ArmyUnit army = new ArmyUnit(id, name, description,Attack,Defense,Size, turns, resources, requirement);

            switch (specialType)
            {
                case "General":
                default:
                    return army;
            }

        }

        public static ArmyLoader GetInstance()
        {
            if (instance == null)
            {
                lock (SyncRoot)
                {
                    if (instance == null)
                    {
                        instance = new ArmyLoader();
                    }
                }
            }
            return instance;
        }

    }
}
