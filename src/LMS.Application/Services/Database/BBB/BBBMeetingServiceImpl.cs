using BigBlueApi.Application.DTOs;
using BigBlueApi.Application.Mappers;
using BigBlueApi.Domain;
using BigBlueApi.Domain.IRepository;
using LIMS.Domain.IRepositories;
using LIMS.Domain.Entities;
using LIMS.Domain.Models;
using LIMS.Domain.Entity;
using LIMS.Domain;

namespace LIMS.Application.Services.Database.BBB
{
    public class BBBMeetingServiceImpl
    {
        private readonly IMeetingRepository _meetings;
        private readonly IUnitOfWork _uow;

        public BBBMeetingServiceImpl(IMeetingRepository repository, IUnitOfWork uow) =>
            (_meetings,_uow) = (repository, uow);

        public async ValueTask<OperationResult<string>> CreateNewMeetingAsync(MeetingAddDto meeting)
        {
            try
            {
                var result = await _meetings.CreateMeetingAsync(MeetingDtoMapper.Map(meeting));
                await _uow.SaveChangesAsync();

                return OperationResult<string>.OnSuccess(result);
            }
            catch (Exception exception)
            {
                return OperationResult<string>.OnException(exception);
            }
        }

        public async ValueTask<OperationResult<bool>> CanLoginOnExistMeeting(string meetingId, UserRoleTypes role, string password)
        {
            try
            {
                var meeting = await _meetings.FindByMeetingIdAsync(meetingId);

                if (role == UserRoleTypes.Attendee)
                {
                        if (meeting.AttendeePassword == password)
                                 return OperationResult<bool>.OnSuccess(true);
                }
                else if (role == UserRoleTypes.Moderator)
                {
                        if (meeting.ModeratorPassword == password)
                                 return OperationResult<bool>.OnSuccess(true);
                }
                else if(role == UserRoleTypes.Guest)
                        return OperationResult<bool>.OnSuccess(true);

                return OperationResult<bool>.OnFailed(
                    "Your Moderator User or Password Intended Not Exists in my Records.");
            }
            catch (Exception exception)
            {
                return OperationResult<bool>.OnException(exception);
            }
        }

        public async ValueTask<OperationResult<Domain.Entities.Meeting>> FindMeeting(long id)
        {
            try
            {
                var meeting  = await _meetings.FindAsync(id);
                if (meeting is null)
                    return OperationResult<Domain.Entities.Meeting>.OnFailed("Meeting Information is Null");

                return OperationResult<Domain.Entities.Meeting>.OnSuccess(meeting);
            }
            catch (Exception exception)
            {
                return OperationResult<Domain.Entities.Meeting>.OnException(exception);
            }
        }

        public async ValueTask<OperationResult<Domain.Entities.Meeting>> FindMeetingWithMeetingId(string meetingId)
        {
            try
            {
                var meeting = await _meetings.FindByMeetingIdAsync(meetingId);
                if (meeting is null)
                    return OperationResult<Domain.Entities.Meeting>.OnFailed("Meeting Information is Null");
                return OperationResult<Domain.Entities.Meeting>.OnSuccess(meeting);
            }
            catch (Exception exception)
            {
                return OperationResult<Domain.Entities.Meeting>.OnException(exception);
            }
        }

        public async ValueTask<OperationResult> EditSession(long id, MeetingEditDto session)
        {
            try
            {
                var meeting =await _meetings.FindAsync(id);
                meeting.Update(session.Name,
                    session.ModeratorPassword,
                    session.AttendeePassword,
                    session.EndDateTime,
                    session.limitCapacity);
                await _uow.SaveChangesAsync();
                return new OperationResult();
            }
            catch (Exception exception)
            {
                return OperationResult.OnException(exception);
            }
        }

        public async ValueTask<OperationResult> StopRunning(string meetingId)
        {
            try
            {
                var meeting = await _meetings.FindByMeetingIdAsync(meetingId);
                meeting.EndSession(DateTime.Now);
                await _uow.SaveChangesAsync();
                return new OperationResult();
            }
            catch (Exception exception)
            {
                return OperationResult.OnException(exception);
            }
        }
    }
}
