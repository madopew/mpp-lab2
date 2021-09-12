using System;

namespace Core.FakerLib.Exceptions
{
    public class CyclicDependencyException : Exception
    {
        public CyclicDependencyException(string message)
            : base(message)
        {
        }
    }
}