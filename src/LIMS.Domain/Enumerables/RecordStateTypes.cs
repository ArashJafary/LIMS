using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Domain.Enumerables;
    public enum RecordStateTypes
    {
        Capture = 1,
        Archive = 2,
        Sanity = 3,
        Process = 4,
        Publish = 5,
        Playback = 6
    }
