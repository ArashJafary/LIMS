using LIMS.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Application.Strategy
{
    public class PlatformHandleResolverService
    {
        private readonly List<IHandleMeetingService> _handleMeetingServices;

        public PlatformHandleResolverService(List<IHandleMeetingService> handleMeetingServices)
            => _handleMeetingServices = handleMeetingServices;

        public IHandleMeetingService ResolveMeetingHandler(string meetingHandlerName)
        {
            _ = meetingHandlerName ?? throw new ArgumentNullException("Cannot Get Null Handler Name Argument.");

            foreach (IHandleMeetingService service in _handleMeetingServices)
                if (meetingHandlerName == service.MeetingHandlerName)
                    return service;

            return null!;
        }
    }
}
