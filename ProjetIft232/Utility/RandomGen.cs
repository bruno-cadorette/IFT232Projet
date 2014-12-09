using System;

namespace ProjetIft232.Utility
{
    public class RandomGen
    {
        private static Random _instance;
        private static readonly object SyncRoot = new object();


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
                    return false;
                }
            }
            return false;
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

        private class PredictableRandom : Random
        {
            public int randomNumber { get; set; }

            public override int Next()
            {
                return randomNumber;
            }

            public override int Next(int maxValue)
            {
                return Math.Min(maxValue - 1, randomNumber);
            }

            public override int Next(int minValue, int maxValue)
            {
                return Math.Max(minValue, Math.Min(maxValue - 1, randomNumber));
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
    }
}