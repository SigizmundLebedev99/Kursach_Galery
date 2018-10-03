using System;
using System.Collections.Generic;
using System.Text;

namespace Galery.Server.Service.Infrostructure
{
    public class OperationResult<T>
    {
        public List<T> Results { get; set; } = new List<T>();

        public bool Sucseeded { get; set; }

        public List<ErrorMessage> ErrorMessages { get; set; } = new List<ErrorMessage>();

        public OperationResult(bool sucseeded)
        {
            Sucseeded = sucseeded;
        }

        public void AddErrorMessage(string property, string message)
        {
            ErrorMessages.Add(new ErrorMessage { Property = property, Message = message });
            Sucseeded = false;
        }
    }
}
