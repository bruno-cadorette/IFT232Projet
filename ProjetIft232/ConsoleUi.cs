using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ProjetIft232
{
    class ConsoleUi
    {
        public void Interact(Game g)
        {
            // Game initial setup
            Console.WriteLine("Bonjour, quel est le nom de votre ville?");
            string cityName = Console.ReadLine();
            var player = new Player();
            player.Cities.Add(new City(cityName));
            g.Players.Add(player);


            Stack<Object> navigation = new Stack<object>(new Object[] {g.CurrentPlayer} );
            bool Quit = false;
            while (!Quit)
            {

                Console.WriteLine(navigation.Peek().ToString());

                if(navigation.Count>1)
                    Console.WriteLine("0. Back");

                Console.WriteLine(String.Join(Environment.NewLine,
                    GetCallableMethods(navigation.Peek().GetType()).Concat(GetCallableMethods(typeof(Game)))
                    .Select((methodinfo, i) => String.Format("{0}. {1}", i+1, ((UserCallable)methodinfo.GetCustomAttributes(typeof(UserCallable), true).First()).UserCommand))));

                string input = Console.ReadLine();
                foreach (string word in input.Split(' '))
                {
                    switch (word)
                    {
                        case "back":
                        case "0":
                            if(navigation.Count>1)
                                navigation.Pop();
                            else
                                Console.WriteLine("Je ne peux pas venir plus en arriere...");
                            break;
                        default:
                        {
                            int methodNumber = int.Parse(word)-1;
                            IEnumerable<MethodInfo> topNavMethods = GetCallableMethods(navigation.Peek().GetType());
                            IEnumerable<MethodInfo> baseMethods = GetCallableMethods(typeof (Game));
                            MethodInfo callableMethod = topNavMethods.Concat(baseMethods).ElementAtOrDefault(methodNumber);
                            if (callableMethod == null)
                            {
                                Console.WriteLine("La commande {0} m'existe pas", word);
                                return;
                            }
                            else
                            {
                                object[] args = {};
                                Object returned;
                                object concernedObject = methodNumber < topNavMethods.Count() ? navigation.Peek() : g;
                                    // TODO: Faire en sorte que si il y a un argument, le demander en popup.
                                try
                                {
                                    returned = callableMethod.Invoke(concernedObject, args);
                                }
                                catch (TargetInvocationException e)
                                {
                                    throw e.InnerException;
                                }

                                if (returned.GetType() == typeof (CommandResult))
                                {
                                    Console.WriteLine("{0}: {1}", ((UserCallable)callableMethod.GetCustomAttribute(typeof(UserCallable))).UserCommand, ((CommandResult)returned).TextResult);
                                }
                                else
                                {
                                    Debug.WriteLine("Going into object {0}", returned.GetType());
                                    navigation.Push(returned);
                                }
                            }
                        }
                            break;

                    }
                }

               


            }

        }

        private IEnumerable<MethodInfo> GetCallableMethods(Type type)
        {
            return type.GetMethods().Where(
                method => method.GetCustomAttributes(true).Any(
                    attribute =>
                        attribute.GetType() == typeof (UserCallable))
                );
        }
    }
}
