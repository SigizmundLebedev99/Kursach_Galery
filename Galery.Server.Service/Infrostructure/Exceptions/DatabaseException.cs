using System;
using System.Collections.Generic;
using System.Text;

namespace Galery.Server.Service.Exceptions
{
    public class DatabaseException : Exception
    {
        public string InnerMessage { get; }

        public DatabaseException(string message, string innerMes) : base(message)
        {
            InnerMessage = innerMes;
        }
    }
}
