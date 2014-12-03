using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ProjetIft232.Utility
{
    public class RandomGen
    {
        class PredictableRandom : Random
        {
            public int randomNumber { get; set; }
            public override int Next()
            {
                return randomNumber;
            }

            public override int Next(int maxValue)
            {
                return Math.Min(maxValue-1,randomNumber);
            }

            public override int Next(int minValue, int maxValue)
            {
                return Math.Max(minValue,Math.Min(maxValue-1,randomNumber));
            }

            public override double NextDouble()
            {
                return randomNumber;
            }

            public override void NextBytes(byte[] buffer)
            {
                throw new NotImplementedException();
            }

            protected override double Sample()
            {
                return randomNumber;
            }
        }
        private static Random _instance = null;
        private static readonly object SyncRoot= new object();


        public static Boolean SetToPredictable(int nbr)
        {
            if (_instance == null)
            {
                lock (SyncRoot)
                {
                    if (_instance == null)
                    {
                        _instance = new PredictableRandom
                        {
                            randomNumber = nbr
                        };
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        public static Random GetInstance()
        {
            if (_instance == null)
            {
                lock (SyncRoot)
                {
                    if (_instance == null)
                    {
                        _instance = new Random();
                    }
                }
            }
            return _instance;
        }
    }
}
