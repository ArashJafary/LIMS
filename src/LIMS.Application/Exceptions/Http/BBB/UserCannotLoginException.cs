using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Application.Exceptions.Http.BBB
{
    public class UserCannotLoginException : Exception
    {
        public UserCannotLoginException()
        {
            
        }
        public UserCannotLoginException(string message) : base(message) 
        {
            
        }
    }
}
