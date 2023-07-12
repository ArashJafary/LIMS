﻿using LIMS.Application.DTOs;
using LIMS.Application.Mappers;
using LIMS.Domain;
using LIMS.Domain.IRepositories;
using LIMS.Domain.IRepositories;
using LIMS.Domain.Entities;
using LIMS.Application.Models;
using LIMS.Domain.Entities;
using LIMS.Domain;

namespace LIMS.Application.Services.Database.BBB
{
    public class BBBMeetingServiceImpl
    {
        private readonly IMeetingRepository _meetings;
        private readonly IUnitOfWork _unitOfWork;

        public BBBMeetingServiceImpl(IMeetingRepository meetings, IUnitOfWork unitOfWork) =>
            (_meetings,_unitOfWork
            ) = (meetings, unitOfWork);

        public async ValueTask<OperationResult<string>> CreateNewMeetingAsync(MeetingAddDto meeting)
        {
            try
            {
                var result = await _meetings.CreateMeetingAsync(MeetingDtoMapper.Map(meeting));
                await _unitOfWork
                    .SaveChangesAsync();

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

        public async ValueTask<OperationResult> UpdateMeeting(long id, MeetingEditDto meetingInput)
        {
            try
            {
                var meeting =await _meetings.FindAsync(id);
                await meeting.Update(meetingInput.Name,
                    meetingInput.ModeratorPassword,
                    meetingInput.AttendeePassword,
                    meetingInput.limitCapacity);
                await _unitOfWork
                    .SaveChangesAsync();
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
                await _unitOfWork
                    .SaveChangesAsync();
                return new OperationResult();
            }
            catch (Exception exception)
            {
                return OperationResult.OnException(exception);
            }
        }
    }
}
