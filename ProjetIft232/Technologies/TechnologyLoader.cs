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
            return (Technology)DeepCopy(_entities[type]);
        }

        public IEnumerable<Technology> Technologies()
        {
            return _entities.Select(x => (Technology)x.Value);
        }

        protected override BuildableEntity CreateEntity(XElement element, int id, string name, string description, int turns, Requirement requirement)
        {
            return new Technology(id, name, description, requirement, turns, GetAffectedEntities(element), GetApplicationCost(element), GetEnhancement(element));
        }

        protected override IEnumerable<XElement> GetChilds(XElement root)
        {
            return root.Element(XName.Get("Technologies")).Elements(XName.Get("Technology"));
        }

        private IEnumerable<int> GetAffectedEntities(XElement element)
        {
            var entities = element.Element(XName.Get("AffectedEntities"));
            if (entities != null)
            {
                return entities.Elements(XName.Get("AffectedEntity")).Select(x=>int.Parse(x.Value));
            }
            return Enumerable.Empty<int>();
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
                var x = enhancement.Element(XName.Get("ConstructionTime"));
                int constructionTime = (x!=null)?int.Parse(x.Value):0;
                return new Enhancement(resources, constructionTime);
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
