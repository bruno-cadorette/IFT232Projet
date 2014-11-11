using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetIft232
{
    public class CommandResult
    {
        public string TextResult { get; set; }
        public CommandResult()
        {

        }

        public CommandResult(string result)
        {
            this.TextResult = result;
        }
    }
}
