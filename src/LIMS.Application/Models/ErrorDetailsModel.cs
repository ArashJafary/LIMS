using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LIMS.Application.Models
{
    public class ErrorDetailsModel
    {
        public string StackTrace { get; set; }
        public string Message { get; set; }

        public ErrorDetailsModel(string stackTrace, string message)
            => (StackTrace, Message) = (stackTrace, message);

        public ErrorDetailsModel(string message) => Message = message;

        public override string ToString()
            => JsonSerializer.Serialize(this);
    }
}
