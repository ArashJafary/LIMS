using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Domain.Entities
{
    public sealed class Playback : BaseEntity
    {
        public string Url { get; private set; }
        public long Length { get; private set; }
        public long Size { get; private set; }
        public Record Record { get; private set; }

        public Playback(string url, long length,long size) 
            => (Url,Length,Size) = (url,Length,Size);
    }
}
