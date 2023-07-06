using LIMS.Application.Services.Database.BBB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using BigBlueApi.Application.DTOs;
using LIMS.Application.Models;
using LIMS.Domain.Entity;

namespace LIMS.Application.Services.Meeting.BBB
{
    public class BBBHandleMeetingService
    {
        private readonly BBBMeetingServiceImpl _meetingService;
        private readonly BBBServerServiceImpl _serverService;
        private readonly BBBUserServiceImpl _userService;
        private readonly BBBMemberShipServiceImpl _memberShipService;

        public BBBHandleMeetingService(
            BBBMeetingServiceImpl sessionService,
            BBBServerServiceImpl serverService,
            BBBUserServiceImpl userService,
            BBBMemberShipServiceImpl memberShipService
        ) =>
            (_meetingService, _serverService, _userService, _memberShipService) = (
                sessionService,
                serverService,
                userService,
                memberShipService
            );

        public async Task<SingleResponse<ServerAddEditDto>> UseCapableServerCreateMeeting()
        {
            var server = await _serverService.MostCapableServer();
            if (!server.Success)
                if (server.Exception is not null)
                    return SingleResponse<ServerAddEditDto>.OnFailed(server.Exception.Data.ToString());
                else
                    return SingleResponse<ServerAddEditDto>.OnFailed(server.OnFailedMessage);
            return SingleResponse<ServerAddEditDto>.OnSuccess(server.Result);
        }
    }

   
}
