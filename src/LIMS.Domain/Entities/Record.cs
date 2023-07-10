using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LIMS.Domain.Entity;
using LIMS.Domain.Enumerables;
using Microsoft.VisualBasic;

namespace LIMS.Domain.Entities
{
    public sealed class Record : BaseEntity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        [RegularExpression("/((([A-Za-z]{3,9}:(?:\\/\\/)?)(?:[-;:&=\\+\\$,\\w]+@)?[A-Za-z0-9.-]+|(?:www.|[-;:&=\\+\\$,\\w]+@)[A-Za-z0-9.-]+)((?:\\/[\\+~%\\/.\\w-_]*)?\\??(?:[-\\+=&;%@.\\w_]*)#?(?:[\\w]*))?)/\r\n")]
        public DateTime StartDataTime { get; private set; }
        public DateTime EndDateTime { get; private set; }
        public bool Published { get; private set; }
        public RecordStateTypes State { get; private set; }

        public Meeting Meeting { get; private set; }
        public IReadOnlyList<Playback> Playbacks { get; }

        public async Task IsValid(string name, string description, Meeting meeting)
        {
            await Task.Run(() =>
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentNullException($"{nameof(name)} Cannot Be Null Or Empty.");
                if (string.IsNullOrWhiteSpace(description))
                    throw new ArgumentNullException($"{description} Cannot Be Null Or Empty");
                if (meeting is null)
                    throw new ArgumentNullException($"{nameof(meeting)} is Not Nullable.");
            });
        }
        public async Task CreateRecord(string name, string description, Meeting meeting)
        {
            await IsValid(name, description, meeting);
            await Task.Run(() =>
            {
                Name = name;
                Description = description;
                State = RecordStateTypes.Capture;
                Meeting = meeting;
            });
        }

        public async Task StopRecord(DateTime now)
            => await Task.Run(() =>
            {
                State = RecordStateTypes.Process;
                EndDateTime = now;
            });

        public async Task PublishRecord()
            => await Task.Run(() =>
            {
                State = RecordStateTypes.Publish;
            });
    }
}
