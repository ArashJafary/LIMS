using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LIMS.Domain.Entity;

namespace LIMS.Domain.Entities
{
    public sealed class Record : BaseEntity
    {
        [RegularExpression("/((([A-Za-z]{3,9}:(?:\\/\\/)?)(?:[-;:&=\\+\\$,\\w]+@)?[A-Za-z0-9.-]+|(?:www.|[-;:&=\\+\\$,\\w]+@)[A-Za-z0-9.-]+)((?:\\/[\\+~%\\/.\\w-_]*)?\\??(?:[-\\+=&;%@.\\w_]*)#?(?:[\\w]*))?)/\r\n")]
        public string Url { get; set; }
        public Meeting Meeting { get; set; }
        public Record(Meeting meeting, string url)
        {
            if (meeting is null)
                throw new ArgumentNullException($"{nameof(meeting)} is Not Nullable.");
            Meeting = meeting;
            Url = url;
        }
    }
}
