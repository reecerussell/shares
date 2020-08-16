using System;

namespace Shares.Core.Password
{
    public class InvalidIterationCountException : Exception
    {
        public InvalidIterationCountException() 
            : base("Iteration count cannot be less than 1.")
        {
        }
    }

    public class InvalidKeySizeException : Exception
    {
        public InvalidKeySizeException()
            : base("Key size must be divisible by, and greater than 8.")
        {

        }
    }

    public class InvalidSaltSizeException : Exception
    {
        public InvalidSaltSizeException()
            : base("Salt size must be divisible by, and greater than 8.")
        {
        }
    }
}
