using Core.Buildings;
using Core.Technologies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Configuration
{
    public class BuildableEntityFactory<T> where T : BuildableEntity
    {
        private Dictionary<int, T> entities;
        public BuildableEntityFactory(IEnumerable<T> data)
        {
            entities = data.ToDictionary(x => x.ID);
        }
        public T this[int index]
        {
            get
            {
                return entities[index];
            }
        }
        public T CreateEntity(int type, Resources resources, IEnumerable<Building> buildings, IEnumerable<Technology> technologies, int number = 1)
        {
            var entity = this[type];
            if (entity is UpgradableEntity)
            {
                (entity as UpgradableEntity).Upgrade(technologies);
            }
            return (entity.CanBeBuild(resources * number, buildings, technologies)) ? entity : null;
        }
        public IEnumerable<T> Entities()
        {
            return entities.Values;
        }
    }
}
