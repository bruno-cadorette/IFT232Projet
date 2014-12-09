using System;
using System.Collections.Generic;
using System.Linq;
using ProjetIft232.Utility;

namespace ProjetIft232.Army
{
    public class Armies
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
            foreach (ArmyUnit unit in flees)
            {
                units.Add(unit);
            }
            flees.Clear();
        }

        public void addUnit(ArmyUnit unit)
        {
            units.Add(unit);
        }

        public List<ArmyUnit> getUnits()
        {
            return units;
        }

        public int getDefense()
        {
            return units.Sum(n => n.Attributes.Defence*n.Size);
        }

        public int getAttack()
        {
            return units.Sum(n => n.Attributes.Attack*n.Size);
        }

        public int size()
        {
            return units.Sum(n => n.Size);
        }


        public void LoseUnit(int n)
        {
            int unitId;
            int number;
            int moral;
            int rand;
            ArmyUnit tmp;
            int lost = n;
            if (lost > size())
                units.Clear();
            while (lost > 0 && units.Any())
            {
                unitId = RandomGen.GetInstance().Next(0, units.Count);
                number = RandomGen.GetInstance().Next(1, Math.Min(units[unitId].Size, lost));
                tmp = units[unitId];
                tmp.moral -= (number*100/tmp.Size)*100/tmp.moral;
                moral = (number*100/size())*100;

                foreach (var unit in units)
                {
                    if (unit.moral > 0)
                        unit.moral -= moral/unit.moral;
                    if (unit.moral < 30)
                    {
                        rand = RandomGen.GetInstance().Next(0, 100);
                        if (unit.moral < rand)
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


            if (size() == 0)
                return false;
            if (opponent.size() == 0)
                return true;

            while (true)
            {
                ourDefense = getDefense();
                ourAttack = getAttack();

                theirDefense = opponent.getDefense();
                theirAttack = opponent.getAttack();

                ourDamage = Math.Max(size()*(1 + ourAttack/theirDefense)/20, 1);
                theirDamage = Math.Max(opponent.size()*(1 + theirAttack/ourDefense)/20, 1);

                LoseUnit(theirDamage);

                opponent.LoseUnit(ourDamage);


                if (size() == 0)
                {
                    opponent.Rally();
                    return false;
                }
                if (opponent.size() == 0)
                {
                    Rally();
                    return true;
                }
            }
        }
    }
}