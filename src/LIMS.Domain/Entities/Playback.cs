using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Domain.Entities
{
    public sealed class Playback : BaseEntity
    {
        [RegularExpression("/((([A-Za-z]{3,9}:(?:\\/\\/)?)(?:[-;:&=\\+\\$,\\w]+@)?[A-Za-z0-9.-]+|(?:www.|[-;:&=\\+\\$,\\w]+@)[A-Za-z0-9.-]+)((?:\\/[\\+~%\\/.\\w-_]*)?\\??(?:[-\\+=&;%@.\\w_]*)#?(?:[\\w]*))?)/\r\n")]
        public string Url { get; private set; }
        public long Length { get; private set; }
        public long Size { get; private set; }

        public Record Record { get; private set; }

        private Playback() { }

        public Playback(string url, long length,long size) 
            => (Url,Length,Size) = (url,length,size);
    }
}
