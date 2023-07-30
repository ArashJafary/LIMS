using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Domain.Exceptions.Database.BBB
{
    public class EntityConnotAddInDatabaseException : Exception
    {
        public EntityConnotAddInDatabaseException(string message) : base(message) { }
        public EntityConnotAddInDatabaseException(string message, Exception innerException) : base(message, innerException) { }
        public EntityConnotAddInDatabaseException() : base() { }
    }
}
