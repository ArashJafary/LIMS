using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LIMS.Domain.Entities;
using LIMS.Domain.Enumerables;
using Microsoft.VisualBasic;

namespace LIMS.Domain.Entities
{
    public sealed class Record : BaseEntity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        public DateTime StartDataTime { get; private set; }
        public DateTime EndDateTime { get; private set; }
        public bool Published { get; private set; }
        public RecordStateTypes State { get; private set; }

        public Meeting Meeting { get; private set; }
        public IReadOnlyList<Playback> Playbacks { get; }

        public void IsValid(string name, string description, Meeting meeting)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException($"{nameof(name)} Cannot Be Null Or Empty.");
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentNullException($"{description} Cannot Be Null Or Empty");
            if (meeting is null)
                throw new ArgumentNullException($"{nameof(meeting)} is Not Nullable.");
        }

        public void CreateRecord(string name, string description, Meeting meeting)
        {
            IsValid(name, description, meeting);
            Name = name;
            Description = description;
            State = RecordStateTypes.Capture;
            Meeting = meeting;
        }

        public void StopRecord(DateTime? stopedDateTime = null)
            => (State, EndDateTime) = (RecordStateTypes.Process, stopedDateTime ?? DateTime.Now);

        public void PublishRecord()
                => State = RecordStateTypes.Publish;
    }
}
