using System;
using Core.FakerLib.Interfaces;

namespace StringGenerator
{
    public class StringGenerator : IGenerator
    {
        public bool CanGenerate(Type t)
        {
            return t == typeof(string);
        }

        public object Generate(Type t)
        {
            return Guid.NewGuid().ToString();
        }
    }
}