using Core.Army;
using Core.Buildings;
using Core.Technologies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Core
{
    [DataContract]
    [KnownType(typeof (Building))]
    [KnownType(typeof (Soldier))]
    public abstract class UpgradableEntity : BuildableEntity
    {
        [DataMember] 
        private List<int> _CurrentTechnologies;

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

        public void Upgrade(IEnumerable<Technology> technologies)
        {
            foreach (var technology in technologies.Where(x => x.CanAffect(this)))
            {
                Upgrade(technology);
            }
        }

        private void ReduceTurnLeft(int minus)
        {
            TurnsLeft = Math.Max(TurnsLeft - minus, 0);
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
        public override bool Equals(object obj)
        {
            return base.Equals(obj) && 
                Enumerable.SequenceEqual(_CurrentTechnologies.OrderBy(x=>x), (obj as UpgradableEntity)._CurrentTechnologies.OrderBy(x=>x));
        }
        public static bool operator ==(UpgradableEntity a, UpgradableEntity b)
        {
            if ((object)a == null)
            {
                return (object)b == null;
            }

            return a.Equals(b);
        }
        public static bool operator !=(UpgradableEntity a, UpgradableEntity b)
        {
            return !(a == b);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}