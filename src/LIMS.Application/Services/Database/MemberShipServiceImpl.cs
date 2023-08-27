using LIMS.Domain.IRepositories;
using LIMS.Application.Models;
using Microsoft.Extensions.Logging;

namespace LIMS.Application.Services.Database;

public class MemberShipServiceImpl
{
    private readonly IUserRepository _users;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMeetingRepository _meetings;
    private readonly IMemberShipRepository _memberShips;
    private readonly ILogger<MemberShipServiceImpl> _logger;

    public MemberShipServiceImpl(
        IUnitOfWork unitOfWork,
        IMemberShipRepository memberShips,
        IUserRepository users,
        IMeetingRepository meetings,
         ILogger<MemberShipServiceImpl> logger)
            => (_unitOfWork, _memberShips, _users, _meetings, _logger)
                = (unitOfWork, memberShips, users, meetings, logger);

    public async ValueTask<OperationResult<bool>> CanJoinUserOnMeeting(long meetingId)
    {
        var meeting = await _meetings.GetAsync(meetingId);

        if (meeting is null)
            return OperationResult<bool>.OnFailed("Meeting Not Found.");

        var memberShips = await _memberShips.GetAllAsync();

        if (memberShips is null)
            return OperationResult<bool>.OnFailed("No Joining Found.");

        var userCountOfMeeting = memberShips.Where(memberShip => memberShip.Meeting.Id == meetingId)!.Count();

        if (meeting.LimitCapacity <= userCountOfMeeting)
        {
            _logger.LogWarning($"Meeting {meeting.Name} is Full Capacity and Cannot Another Join On That.");

            return OperationResult<bool>.OnFailed("Cannot Join On Meeting Because of Meeting Capacity is Fulled.");
        }

        return OperationResult<bool>.OnSuccess(true);
    }

    public async ValueTask<OperationResult<long>> JoinUserOnMeeting(long userId, string meetingId)
    {
        try
        {
            var user = await _users.GetByIdAsync(userId);

            if (user is null)
                return OperationResult<long>.OnFailed("User is Not Valid.");

            var meeting = await _meetings.GetByMeetingIdAsync(meetingId);

            if (meeting is null)
                return OperationResult<long>.OnFailed("Meeting is Not Valid.");

            var result = await _memberShips.CreateForMeetingAsync(meeting, user);

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"User {user.FullName} is Joined on {meeting.Name}.");

            return OperationResult<long>.OnSuccess(result);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);

            return OperationResult<long>.OnException(exception);
        }
    }

    public async ValueTask<OperationResult> BanUserFromMeeting(long userId, string meetingId)
    {
        try
        {
            var user = await _users.GetByIdAsync(userId);

            if (user is null)
                return OperationResult.OnFailed("User is Not Valid.");

            var meeting = await _meetings.GetByMeetingIdAsync(meetingId);

            if (meeting is null)
                return OperationResult.OnFailed("Meeting is Not Valid.");

            var member = await _memberShips.GetAsync(user.Id, meeting.Id);

            if (member is null)
                return OperationResult.OnFailed("Your Considered Joining Was Not Found.");

            member.BanUser();

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"User {user.FullName} is Banned From {meeting.Name} and Cannot Join Another.");

            return new OperationResult();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);

            return OperationResult.OnException(exception);
        }
    }
}
