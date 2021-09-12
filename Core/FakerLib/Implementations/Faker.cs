using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core.FakerLib.Exceptions;
using Core.FakerLib.Interfaces;

namespace Core.FakerLib.Implementations
{
    public static class Faker
    {
        private class FakerImpl
        {
            private readonly List<IGenerator> generators;

            public FakerImpl()
            {
                var generatorType = typeof(IGenerator);
                var impls = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .Where(t => t.GetInterfaces().Contains(generatorType) && t.IsClass)
                    .Select(t => (IGenerator) Activator.CreateInstance(t));
                generators = new List<IGenerator>(impls);
            }

            public object Create(Type t)
            {
                foreach (var generator in generators)
                {
                    if (generator.CanGenerate(t)) return generator.Generate(t);
                }

                CyclicDependencyHelper.Assert(t);

                var obj = Initialize(t);
                InitializeFields(obj);
                return obj;
            }

            private object Initialize(Type t)
            {
                var constructors = t.GetConstructors().ToList();
                foreach (var constructor in constructors)
                {
                    try
                    {
                        var pars = constructor.GetParameters()
                            .Select(p => p.ParameterType)
                            .Select(Faker.Create);

                        return constructor.Invoke(pars.ToArray());
                    }
                    catch
                    {
                        // cant create - no worries try next
                    }
                }

                throw new InsufficientDependencyException($"Cannot create object of type {t}");
            }

            private void InitializeFields(object o)
            {
                var objType = o.GetType();
                objType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                    .ToList()
                    .ForEach(f =>
                        {
                            try
                            {
                                if (Equals(f.GetValue(o), GetDefaultValue(f.FieldType)))
                                {
                                    f.SetValue(o, Faker.Create(f.FieldType));
                                }
                            }
                            catch
                            {
                                // cant create - no worries try next
                            }
                        }
                    );
            }
            
            private static object GetDefaultValue(Type t)
            {
                return t.IsValueType ? Activator.CreateInstance(t) : null;
            }
        }

        private static readonly FakerImpl Impl = new();

        public static T Create<T>()
        {
            return (T) Impl.Create(typeof(T));
        }

        internal static object Create(Type t)
        {
            return Impl.Create(t);
        }
    }
}