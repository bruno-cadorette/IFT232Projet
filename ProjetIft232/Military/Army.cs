using System;
using System.Collections.Generic;
using System.Linq;
using Core.Utility;
using Core.Map;
using System.Runtime.Serialization;

namespace Core.Military
{
    [DataContract]
    public class Army : MovableItem, IEnumerable<Groupment>
    {
        [DataMember]
        private List<Groupment> units;
        public Resources Resources { get; set; }

        public Army()
        {
            units = new List<Groupment>();
            Speed = 1;
            Resources = new Resources();
        }

        public Resources AvailableTransport()
        {
            return Resources - units.Aggregate(Resources.Zero(), (acc, x) => acc + x.Transport);
        }

        public Resources GiveAllResources()
        {
            var resources = Resources;
            Resources = Resources.Zero();
            return resources;
        }
        private void Regroup()
        {
            units = units.Where(x => x.Size > 0).ToList();
        }

        public void Add(Soldier unit)
        {
            var groupment = units.FirstOrDefault(x => x.Type == unit);
            if (groupment != null)
            {
                groupment.Add(1);
            }
            else
            {
                units.Add(new Groupment(unit));
            }
        }
        private void Add(Groupment group)
        {
            units.Add(group);
        }
        public int Attack
        {
            get
            {
                return units.Sum(n => n.Attributes.Attack);
            }
        }

        public int Defense
        {
            get
            {
                return units.Sum(n => n.Attributes.Defence);
            }
        }

        public int Size
        {
            get
            {
                return units.Sum(n => n.Size);
            }
        }


        public void LoseUnit(int damage)
        {
            foreach (var group in units)
            {
                float ratio = (float)group.Attributes.Defence / (float)Defense;
                group.TakeDamage((int)(ratio * damage));
            }
            Regroup();
        }

        public bool Fight(Army opponent)
        {

            if (this.Size == 0)
                return false;
            if (opponent.Size == 0)
                return true;

            while (true)
            {


                int ourDamage = DamageCalculator(Attack, opponent.Defense);
                int theirDamage = DamageCalculator(opponent.Attack, Defense);

                LoseUnit(theirDamage);

                opponent.LoseUnit(ourDamage);


                if (Size == 0)
                {
                    return false;
                }
                if (opponent.Size == 0)
                {
                    return true;
                }
            }
        }

        private int DamageCalculator(int attack, int defense)
        {
            float ratio = Math.Min(Math.Max((float)attack / (float)defense, 0.05f), 1f);
            return (int)(attack * ratio);
        }

        public void Merge(Army army)
        {
            foreach (var group in army)
            {
                var a = this.FirstOrDefault(x => x.Type == group.Type);
                if (a != null)
                {
                    a.Add(group.Size);
                }
                else
                {
                    this.Add(group);
                }
            }
            Resources += army.GiveAllResources();
        }

        public IEnumerator<Groupment> GetEnumerator()
        {
            return units.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public override WorldMapItem InteractWith(WorldMapItem item)
        {
            var otherArmy = (item as Army);
            if (item.PlayerId == PlayerId)
            {
                Merge(otherArmy);
                return null;
            }
            else if (Fight(otherArmy))
            {
                Resources += otherArmy.GiveAllResources();
                return null;
            }
            else
            {
                otherArmy.Resources += GiveAllResources();
                return otherArmy;
            }
        }
    }
}