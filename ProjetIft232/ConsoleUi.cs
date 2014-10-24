using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ProjetIft232
{
    class ConsoleUi
    {
        public void Interact()
        {
            //System.Reflection.Assembly.GetCallingAssembly().GetType(
            //Game.GetType().attr
            bool Quit = false;
            while (!Quit)
            {
                string input = Console.ReadLine();
                Object current = Game.CurrentGame.CurrentPlayer;
                foreach (string word in input.Split(' '))
                {
                    MethodInfo CallableMethod = current.GetType().GetMethods().FirstOrDefault(
                                                    method=>method.GetCustomAttributes(true).Any(
                                                            attribute=>attribute.GetType()==typeof(UserCallable) && ((UserCallable)attribute).UserCommand==word)
                                                );
                    if (CallableMethod == null)
                    {
                        Console.WriteLine("La commande {0} m'existe pas", word);
                        return;
                    }
                    else
                    {
                        object[] args = {}; // TODO: Faire en sorte que si il y a un argument, le demander en popup.
                        Object returned = CallableMethod.Invoke(current,args);
                        if (returned.GetType() == typeof(CommandResult))
                        {
                            Console.WriteLine("{0}: {1}",word,((CommandResult)returned).TextResult);
                        }
                        else
                        {
                            Debug.WriteLine("Going into object {0}", returned.GetType());
                            current = returned;
                        }
                    }
                }
            }

        }
    }
}
