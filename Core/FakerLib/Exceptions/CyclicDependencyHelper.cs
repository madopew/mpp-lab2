using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Core.FakerLib.Exceptions
{
    internal static class CyclicDependencyHelper
    {
        public static bool IsCyclic(Type t)
        {
            var cons = t.GetConstructors();
            return cons.Any() && t.GetConstructors()
                .All(IsCyclic);
        }

        public static void ThrowException(Type t) =>
            throw new CyclicDependencyException($"{t} contains cyclical dependency");

        public static void Assert(Type t)
        {
            if (IsCyclic(t))
            {
                ThrowException(t);
            }
        }

        private static bool IsCyclic(ConstructorInfo constructor)
        {
            var types = constructor.GetParameters()
                .Select(p => p.ParameterType)
                .ToList();

            int i = 0;
            while (i < types.Count)
            {
                var deps = GetDirectUniqueDependencies(types[i]);
                if (types.Intersect(deps).Any())
                {
                    return true;
                }
                
                types.AddRange(deps);
                i++;
            }

            return false;
        }

        private static List<Type> GetDirectUniqueDependencies(Type t)
        {
            return t.GetConstructors()
                .SelectMany(c => c.GetParameters())
                .Select(p => p.ParameterType)
                .Distinct()
                .ToList();
        }
    }
}