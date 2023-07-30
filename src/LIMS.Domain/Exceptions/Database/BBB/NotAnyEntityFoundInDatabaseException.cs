using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Domain.Exceptions.Database.BBB
{
    public class NotAnyEntityFoundInDatabaseException : Exception
    {
        public NotAnyEntityFoundInDatabaseException()
        {

        }
        public NotAnyEntityFoundInDatabaseException(string message) : base(message)
        {

        }
        public NotAnyEntityFoundInDatabaseException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
