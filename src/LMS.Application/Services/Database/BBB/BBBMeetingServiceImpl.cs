using BigBlueApi.Application.DTOs;
using BigBlueApi.Application.Mappers;
using BigBlueApi.Domain;
using BigBlueApi.Domain.IRepository;
using BigBlueApi.Models;
using LIMS.Domain.Entity;
using LIMS.Domain.IRepositories;
using LIMS.Domain.Models;

namespace LIMS.Application.Services.Database.BBB
{
    public class BBBMeetingServiceImpl
    {
        private readonly IMeetingRepository _meetings;
        private readonly IUnitOfWork _uow;

        public BBBMeetingServiceImpl(IMeetingRepository repository, IUnitOfWork uow) =>
            (_meetings,_uow) = (repository, uow);

        public async ValueTask<OperationResult<long>> CreateMeetingAsync(SessionAddEditDto meeting)
        public async ValueTask<OperationResult<string>> CreateNewMeetingAsync(MeetingAddEditDto meeting)
        {
            await _meetings.CreateMeetingAsync(SessionMapper.Map(meeting));
            OperationResult<long>.OnSuccess(await _uow.SaveChangesAsync());
            return meeting.MeetingId;
            try
            {
                var result = await _repository.CreateMeetingAsync(MeetingDtoMapper.Map(meeting));
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
                var meeting = await _repository.FindByMeetingIdAsync(meetingId);

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

        public async ValueTask<OperationResult<Meeting>> FindMeeting(long id)
        {
            try
            {
                var meeting  = await _repository.FindAsync(id);
                if (meeting is null)
                    return OperationResult<Meeting>.OnFailed("Meeting Information is Null");

                return OperationResult<Meeting>.OnSuccess(meeting);
            }
            catch (Exception exception)
            {
                return OperationResult<Meeting>.OnException(exception);
            }
        }

        public async ValueTask<OperationResult> EditSession(long id, MeetingAddEditDto session)
        {
            try
            {
                await _repository.UpdateMeetingAsync(id, MeetingDtoMapper.Map(session));
                return new OperationResult();
            }
            catch (Exception exception)
            {
                return OperationResult.OnException(exception);
            }
        }

        public async ValueTask<OperationResult> StopRunning(long id)
        {
            try
            {
                await _repository.EndMeetingAsync(id);
                return new OperationResult();
            }
            catch (Exception exception)
            {
                return OperationResult.OnException(exception);
            }
        }
    }
}
