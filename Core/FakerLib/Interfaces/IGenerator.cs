using System;

namespace Core.FakerLib.Interfaces
{
    public interface IGenerator
    {
        bool CanGenerate(Type t);

        object Generate(Type t);
    }
}