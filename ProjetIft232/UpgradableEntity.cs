using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ProjetIft232.Army;
using ProjetIft232.Buildings;
using ProjetIft232.Technologies;

namespace ProjetIft232
{
    [DataContract]
    [KnownType(typeof (Building))]
    [KnownType(typeof (ArmyUnit))]
    public abstract class UpgradableEntity : BuildableEntity
    {
        [DataMember] private List<int> _CurrentTechnologies;

        public UpgradableEntity()
        {
        }

        public UpgradableEntity(UpgradableEntity entity)
            : this(entity.ID, entity.Name, entity.Description, entity.TurnsLeft, entity.Requirement)
        {
        }

        public UpgradableEntity(int id, string name, string description, int turnsLeft, Requirement requirement)
            : base(id, name, description, turnsLeft, requirement)
        {
            _CurrentTechnologies = new List<int>();
        }

        public void Upgrade(Technology technology)
        {
            _CurrentTechnologies.Add(technology.ID);
            ReduceTurnLeft(technology.Enhancements.ConstructionTime);
            UpgradeEntity(technology);
        }

        private void ReduceTurnLeft(int minus)
        {
            TurnsLeft = ((TurnsLeft - minus) < 0) ? 0 : TurnsLeft - minus;
        }

        protected abstract void UpgradeEntity(Technology technology);

        public bool CanBeAffected(Technology technology)
        {
            return technology.CanAffect(this) && _CurrentTechnologies.All(x => x != technology.ID);
        }

        public bool CanBeUpgraded(Resources actualResource, Technology technology)
        {
            return !InConstruction && technology.ApplicationCost <= actualResource && CanBeAffected(technology);
        }
    }
}