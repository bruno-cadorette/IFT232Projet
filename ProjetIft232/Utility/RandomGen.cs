using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ProjetIft232.Utility
{
    public class RandomGen
    {
        private static RandomGen instance = null;
        private bool random;
        private Random randomGen;
        private static object SyncRoot= new object();
        private RandomGen()
        {
            random = false;
            randomGen = new Random();
        }

        public void SetRandom(bool truc)
        {
            random = truc;
        }

        public int Next(int min, int max)
        {
            if (random)
            {
                return randomGen.Next(min, max);
            }
            else
            {
                return (min + max)/2;
            }
        }

        public static RandomGen GetInstance()
        {
            if (instance == null)
            {
                lock (SyncRoot)
                {
                    if (instance == null)
                    {
                        instance = new RandomGen();
                    }
                }
            }
            return instance;
        }
    }
}
