using LIMS.Application.Models;
using LIMS.Application.Services.Database;
using LIMS.Application.Strategy;
using LIMS.Domain.Enumerables;
using LIMS.Domain.Services;

namespace LIMS.Application.Services.Http
{
    public class MeetingSettingsService
    {
        private readonly MeetingServiceImpl _meetingService;
        private readonly PlatformHandleResolverService _platformHandleResolverService;

        public MeetingSettingsService(MeetingServiceImpl meetingService, PlatformHandleResolverService platformHandleResolver)
            => (_platformHandleResolverService, _meetingService) = (platformHandleResolver, meetingService);

        public async ValueTask<MeetingSettingsSingleResponse> IsSettingsOkAsync(string meetingId)
        {
            var meeting = await _meetingService.FindOneMeetingWithMeetingId(meetingId);

            string platformName;

            IHandleMeetingService platform = null!;

            switch (meeting.Result.Platform)
            {
                case PlatformTypes.BigBlueButton:
                    _platformHandleResolverService.ResolveMeetingHandler("bbb");
                    platformName = "BigBlueButton";
                    break;
                case PlatformTypes.AdobeConnect:
                    _platformHandleResolverService.ResolveMeetingHandler("connect");
                    platformName = "Adobe Connect";
                    break;
                default:
                    throw new NotImplementedException("Undefined Platform.");
            }

            var settingsOk = await platform.IsOkSettings(meetingId);

            if (!settingsOk.Data)
                return MeetingSettingsSingleResponse.OnFail(platformName);

            return MeetingSettingsSingleResponse.OnOk();
        }
    }
}
