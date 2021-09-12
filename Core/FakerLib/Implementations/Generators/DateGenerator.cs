using System;
using Core.FakerLib.Interfaces;

namespace Core.FakerLib.Implementations.Generators
{
    internal class DateGenerator : IGenerator
    {
        private readonly Random rnd = new Random();
        public bool CanGenerate(Type t)
        {
            return t == typeof(DateTime);
        }

        public object Generate(Type t)
        {
            return new DateTime(rnd.Next(1, 3000), rnd.Next(1, 13), rnd.Next(1, 29));
        }
    }
}