using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Application.Models
{
    public class MeetingSettingsSingleResponse
    {
        public bool Success { get; private set; } = true;
        public string Platform { get; private set; }

        private MeetingSettingsSingleResponse(string platform) => Platform = platform;
        public MeetingSettingsSingleResponse() => Success = false;

        public static MeetingSettingsSingleResponse OnOk(string platform) => new MeetingSettingsSingleResponse(platform);
        public static MeetingSettingsSingleResponse OnFail() => new MeetingSettingsSingleResponse();
    }
}
