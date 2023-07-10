using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Application.Models
{
    public class SingleResponse<T>
    {
        public IEnumerable<string> Errors { get; private set; }
        public string Error { get; private set; }
        public T? Data { get; private set; }

        public SingleResponse(string error)
        {
            Error = error;
        }

        public SingleResponse(IEnumerable<string> errors)
        {
            Errors = errors;
        }

        public SingleResponse(T data)
        {
            Data  = data;
        }
        public static SingleResponse<T> OnFailed(string error)
            => new(error) { Data = default, Error = error };

        public static SingleResponse<T> OnFailed(List<string> errors) =>
            new(errors) { Data = default, Errors = errors };

        public static SingleResponse<T> OnSuccess(T data)
            => new(data) { Data = data, Errors = { } };
    }
}
