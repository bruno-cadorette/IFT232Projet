using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetIft232
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class UserCallable : Attribute
    {
        public String UserCommand { get; set; }
        public UserCallable(String UserCommand)
        {
            this.UserCommand = UserCommand;
        }
    }
}
