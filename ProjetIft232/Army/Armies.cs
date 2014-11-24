using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjetIft232.Army
{
    public class Armies
    {

        private int moral;

        private List<ArmyUnit> units;

        public Armies()
        {
            moral = 100;
            units = new List<ArmyUnit>();
        }

        public void addUnit(ArmyUnit unit)
        {
            units.Add(unit);
        }

        public List<ArmyUnit> getUnits()
        {
            return units;
        }

        public int getMoral()
        {
            return moral;
        }

        public void setMoral(int moral)
        {
            this.moral = moral;
        }

        public int getDefense()
        {
            return units.Sum(n => n.Defense);
        }

        public int getAttack()
        {
            return units.Sum(n => n.Attack);
        }

        public int size()
        {
            return units.Count;
        }

        public void loose(int n)
        {
            int lost = n > size() ? size() : n;
            units.RemoveRange(0, lost);
        }

        public bool Fight(Armies opponent)
        {
            Random rand = new Random();

            int ourDefense;
            int ourAttack;

            int theirDefense;
            int theirAttack;

            int ourDamage;
            int theirDamage;

            int ourlooses;
            int theirlooses;


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

                ourDamage = Math.Max(size() * (1 + ourAttack / theirDefense) / 20, 1);
                theirDamage = Math.Max(opponent.size() * (1 + theirAttack / ourDefense) / 20, 1);

                ourlooses = theirDamage * 100 / size();
                theirlooses = ourDamage * 100 / opponent.size();

                loose(theirDamage);

                opponent.loose(ourDamage);

                moral = moral - ourlooses * 100 / moral;
                opponent.setMoral(opponent.getMoral() - theirlooses * 100 / opponent.getMoral());

                if (size() == 0)
                    return false;
                if (opponent.size() == 0)
                    return true;

                if (ourlooses > 25)
                {
                    if (getMoral() < rand.Next(100))
                        return false;
                }
                if (theirlooses > 25)
                {
                    if (opponent.getMoral() < rand.Next(100))
                        return true;
                }
            }
        }
    }
}
