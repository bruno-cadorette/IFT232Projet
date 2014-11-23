using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetIft232.Buildings;

namespace ProjetIft232.Army
{
    public class Warrior : ArmyUnit
    {
        public Warrior()
            :base(0,"Guerrier","Un Guerrier méchant",1,1,3,Resources.Zero(),Requirement.Zero())
        {
           
            //Requirement = Requirement.Zero();//new Requirement(req,new Resources(0,0,2,0,1));
            Transport = new Resources(1,1,1,1,1);
        }
    }
}
