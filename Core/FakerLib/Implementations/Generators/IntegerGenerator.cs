using System;
using Core.FakerLib.Interfaces;

namespace Core.FakerLib.Implementations.Generators
{
    internal class IntegerGenerator : IGenerator
    {
        private readonly Random rnd = new Random();
        
        public bool CanGenerate(Type t)
        {
            return t == typeof(int);
        }

        public object Generate(Type t)
        {
            return rnd.Next();
        }
    }
}