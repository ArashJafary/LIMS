using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Application.Models
{
    public class ResultSingleResponse<T>
    {
        public IEnumerable<string> Errors { get; private set; }
        public string Error { get; private set; }
        public T? Data { get; private set; }

        public ResultSingleResponse(string error)
        {
            Error = error;
        }

        public ResultSingleResponse(IEnumerable<string> errors)
        {
            Errors = errors;
        }

        public ResultSingleResponse(T data)
        {
            Data  = data;
        }
        public static ResultSingleResponse<T> OnFailed(string error)
            => new(error) { Data = default, Error = error };

        public static ResultSingleResponse<T> OnFailed(List<string> errors) =>
            new(errors) { Data = default, Errors = errors };

        public static ResultSingleResponse<T> OnSuccess(T data)
            => new(data) { Data = data, Errors = { } };
    }
}
