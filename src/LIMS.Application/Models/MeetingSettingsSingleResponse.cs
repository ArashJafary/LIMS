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
        private MeetingSettingsSingleResponse()
        {
            
        }

        public static MeetingSettingsSingleResponse OnOk() => new MeetingSettingsSingleResponse();
        public static MeetingSettingsSingleResponse OnFail(string platform) => new MeetingSettingsSingleResponse(platform);
    }
}
