using System;
using System.Collections.Generic;
using System.Linq;
using Core.Utility;

namespace Core.Army
{
    public class Armies : ICollection<ArmyUnit>
    {
        private readonly List<ArmyUnit> flees;
        private List<ArmyUnit> units;

        public Armies()
        {
            units = new List<ArmyUnit>();
            flees = new List<ArmyUnit>();
        }

        public void Rally()
        {
            units.AddRange(flees);
            flees.Clear();
        }

        public void Add(ArmyUnit unit)
        {
            units.Add(unit);
        }

        public int Defense()
        {
            return units.Sum(n => n.Attributes.Defence * n.Size);
        }

        public int Attack()
        {
            return units.Sum(n => n.Attributes.Attack * n.Size);
        }


        public void LoseUnit(int n)
        {
            int unitId;
            int number;
            int moral;
            ArmyUnit tmp;
            int lost = n;
            if (lost > Count)
                units.Clear();
            while (lost > 0 && units.Any())
            {
                unitId = RandomGen.GetInstance().Next(0, units.Count);
                number = RandomGen.GetInstance().Next(1, Math.Min(units[unitId].Size, lost));
                tmp = units[unitId];
                tmp.moral -= (number * 100 / tmp.Size) * 100 / tmp.moral;
                moral = (number * 100 / Count) * 100;

                foreach (var unit in units)
                {
                    if (unit.moral > 0)
                    {
                        unit.moral -= moral / unit.moral;
                    }
                    if (unit.moral < 30)
                    {
                        if (unit.moral < RandomGen.GetInstance().Next(0, 100))
                        {
                            flees.Add(unit);
                            unit.moral = 0;
                        }
                    }
                }


                tmp.Size -= number;
                if (tmp.Size == 0)
                {
                    units.Remove(tmp);
                }
                units = units.Where(w => w.moral > 0).ToList();
                lost -= number;
            }
        }

        public bool Fight(Armies opponent)
        {
            int ourDefense;
            int ourAttack;

            int theirDefense;
            int theirAttack;

            int ourDamage;
            int theirDamage;


            if (Count == 0)
                return false;
            if (opponent.Count() == 0)
                return true;

            while (true)
            {
                ourDefense = Defense();
                ourAttack = Attack();

                theirDefense = opponent.Defense();
                theirAttack = opponent.Attack();

                ourDamage = Math.Max(Count * (1 + ourAttack / theirDefense) / 20, 1);
                theirDamage = Math.Max(opponent.Count() * (1 + theirAttack / ourDefense) / 20, 1);

                LoseUnit(theirDamage);

                opponent.LoseUnit(ourDamage);


                if (Count == 0)
                {
                    opponent.Rally();
                    return false;
                }
                if (opponent.Count == 0)
                {
                    Rally();
                    return true;
                }
            }
        }

        public void Clear()
        {
            units.Clear();
        }

        public bool Contains(ArmyUnit unit)
        {
            return units.Contains(unit);
        }

        public void CopyTo(ArmyUnit[] array, int arrayIndex)
        {
            units.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return units.Sum(n => n.Size); }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(ArmyUnit unit)
        {
            return units.Remove(unit);
        }

        public IEnumerator<ArmyUnit> GetEnumerator()
        {
            return units.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}