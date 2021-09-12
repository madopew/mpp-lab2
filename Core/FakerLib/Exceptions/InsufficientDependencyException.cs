using System;

namespace Core.FakerLib.Exceptions
{
    public class InsufficientDependencyException : Exception
    {
        public InsufficientDependencyException(string msg)
            : base(msg)
        {
            
        }
    }
}