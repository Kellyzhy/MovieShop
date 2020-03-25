using System;
using System.Collections.Generic;
using System.Text;

namespace MovieShop.Core.Exceptions
{
    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message)
        {
        }
    }

public class NotFoundException : Exception
    {
        public NotFoundException(string name, object key)
            : base($"Resource \"{name}\" ({key}) was not found.")
        {
        }
        public NotFoundException(string message) : base(message)
        {
        }
    }
}
