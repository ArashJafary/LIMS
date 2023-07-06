using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LIMS.Domain.Entities;
using LIMS.Domain.Enumerables;

namespace LIMS.Domain.Entities
{
    public sealed class Platform : BaseEntity
    {
        public string PlatformType { get; private set; }
        public Platform(PlatformTypes platformType)
            => PlatformType = platformType.ToString();
        public IReadOnlyList<Meeting> Meetings { get; }
    }
}
