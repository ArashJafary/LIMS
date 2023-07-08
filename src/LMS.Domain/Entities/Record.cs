using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LIMS.Domain.Entity;
using Microsoft.VisualBasic;

namespace LIMS.Domain.Entities
{
    public sealed class Record : BaseEntity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        [RegularExpression("/((([A-Za-z]{3,9}:(?:\\/\\/)?)(?:[-;:&=\\+\\$,\\w]+@)?[A-Za-z0-9.-]+|(?:www.|[-;:&=\\+\\$,\\w]+@)[A-Za-z0-9.-]+)((?:\\/[\\+~%\\/.\\w-_]*)?\\??(?:[-\\+=&;%@.\\w_]*)#?(?:[\\w]*))?)/\r\n")]
        public string Url { get; private set; }
        public DateTime StartDataTime { get; private set; }
        public DateTime EndDateTime { get; private set; }
        public bool Published { get; private set; }
        public string State { get; private set; }

        public Meeting Meeting { get; }
        public IReadOnlyList<Playback> Playbacks { get; }

        public Record(Meeting meeting, string url)
        {
            if (meeting is null)
                throw new ArgumentNullException($"{nameof(meeting)} is Not Nullable.");
            Meeting = meeting;
            Url = url;
        }
    }
}
