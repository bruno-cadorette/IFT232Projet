using ProjetIft232.Army;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ProjetIft232.Technologies
{
    public class TechnologyLoader : Loader
    {
        private static TechnologyLoader instance;

        private static object SyncRoot = new object();

        public Technology GetTechnology(int type)
        {
            return (Technology)GetEntity(type);
        }

        public IEnumerable<Technology> Technologies()
        {
            return _entities.Select(x => (Technology)x.Value);
        }

        protected override BuildableEntity CreateEntity(XElement element, int id, string name, string description, int turns, Requirement requirement)
        {
            var affectedEntities = GetAffectedEntities(element);
            return new Technology(id, name, description, requirement, turns, affectedEntities.Item1,affectedEntities.Item2, GetApplicationCost(element), GetEnhancement(element));
        }

        protected override IEnumerable<XElement> GetChilds(XElement root)
        {
            return root.Element(XName.Get("Technologies")).Elements(XName.Get("Technology"));
        }

        private Tuple<IEnumerable<int>, IEnumerable<int>> GetAffectedEntities(XElement element)
        {
            var entities = element.Element(XName.Get("AffectedEntities"));
            if (entities != null)
            {
                return Tuple.Create(
                    entities.Elements(XName.Get("AffectedBuilding")).Select(x=>int.Parse(x.Value)),
                    entities.Elements(XName.Get("AffectedSoldier")).Select(x=>int.Parse(x.Value)))
                    ;
            }
            return Tuple.Create(Enumerable.Empty<int>(), Enumerable.Empty<int>());
        }

        private Resources GetApplicationCost(XElement element)
        {
            var cost = element.Element(XName.Get("ApplicationCost"));
            if(cost!=null)
            {
                return GetResources(cost);
            }
            return Resources.Zero();
        }
        private Enhancement GetEnhancement(XElement element)
        {
            var enhancement = element.Element(XName.Get("Enhancement"));
            if (enhancement != null)
            {
                var resources = GetResources(enhancement);
                var construction = enhancement.Element(XName.Get("ConstructionTime"));
                int constructionTime = (construction != null) ? int.Parse(construction.Value) : 0;

                var soldierAttributes = enhancement.Element(XName.Get("SoldierAttributes"));
                int attack = 0;
                int defence = 0;
                if (soldierAttributes != null)
                {
                    var xElementAttack = soldierAttributes.Element(XName.Get("Attack"));
                    var xElementDefence = soldierAttributes.Element(XName.Get("Defence"));
                    attack = (xElementAttack != null) ? int.Parse(xElementAttack.Value) : 0;
                    defence = (xElementDefence != null) ? int.Parse(xElementDefence.Value) : 0;
                }
                return new Enhancement(resources, new SoldierAttributes(attack,defence), constructionTime);
            }
            return Enhancement.Zero();
        }


        public static TechnologyLoader GetInstance()
        {
            if (instance == null)
            {
                lock (SyncRoot)
                {
                    if (instance == null)
                    {
                        instance = new TechnologyLoader();
                    }
                }
            }
            return instance;
        }
    }
}
