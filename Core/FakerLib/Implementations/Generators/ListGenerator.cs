using System;
using System.Collections;
using System.Collections.Generic;
using Core.FakerLib.Interfaces;

namespace Core.FakerLib.Implementations.Generators
{
    internal class ListGenerator : IGenerator
    {
        private readonly Random rnd = new Random();
        
        public bool CanGenerate(Type t)
        {
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(List<>);
        }

        public object Generate(Type t)
        {
            var list = (IList) Activator.CreateInstance(t);
            var amount = rnd.Next(0, 100);
            
            for (int i = 0; i < amount; i++)
            {
                list.Add(Faker.Create(t.GetGenericArguments()[0]));
            }

            return list;
        }
    }
}