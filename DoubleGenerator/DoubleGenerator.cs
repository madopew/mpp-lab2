using System;
using Core.FakerLib.Interfaces;

namespace DoubleGenerator
{
    public class DoubleGenerator : IGenerator
    {
        private readonly Random rnd = new Random();
        
        public bool CanGenerate(Type t)
        {
            return t == typeof(double);
        }

        public object Generate(Type t)
        {
            return rnd.NextDouble();
        }
    }
}