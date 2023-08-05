using LIMS.Domain.Entities;
using LIMS.Domain.Entities;
using LIMS.Domain.Enumerables;

namespace LIMS.Domain.Entities;
public sealed class Meeting : BaseEntity
{
    public string MeetingId { get; }
    public string? ParentMeetingId { get; }
    public bool IsRecord { get; }
    public string Name { get; private set; }
    public string ModeratorPassword { get; private set; }
    public string AttendeePassword { get; private set; }
    public DateTime StartDateTime { get; }
    public DateTime EndDateTime { get; private set;  }
    public bool IsRunning { get; private set; } = true;
    public int LimitCapacity { get; private set; }
    public bool IsBreakout { get; private set; }
    public bool FreeJoinOnBreakout { get; private set; }
    public bool AutoStartRecording { get; private set; }
    public PlatformTypes Platform { get; private set; }
    public MeetingTypes Type { get; private set; } = MeetingTypes.Public;

    public Record Record { get; }
    public Server Server { get; private set; }
    public List<User> Users { get; private set; }
    public IReadOnlyList<MemberShip> MemberShips { get; }

    private Meeting() { }

   
    private void IsValid(
        string name,
        string moderatorPassword,
        string attendeePassword,
        int limitCapacity,
        DateTime inputDateTime
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException($"The {nameof(name)} is Null Or Invalid.");
        if (string.IsNullOrWhiteSpace(moderatorPassword))
            throw new ArgumentNullException($"The {nameof(moderatorPassword)} is Null Or Invalid.");
        if (string.IsNullOrWhiteSpace(attendeePassword))
            throw new ArgumentNullException($"The {nameof(attendeePassword)} is Null Or Invalid.");
        if (limitCapacity <= 0)
            throw new ArgumentException($"The {nameof(LimitCapacity)}  Cant be zero or less");
        if (inputDateTime < DateTime.UtcNow)
            throw new ArgumentException($"The {nameof(inputDateTime)} is Null Or Invalid.");
    }

    public Meeting(
        string meetingId,
        bool isRecord,
        string name,
        string moderatorPassword,
        string attendeePassword,
        DateTime startDateTime,
        DateTime endDateTime,
        int limitCapacity,
        string? parentMeetingId,
        bool isRunning,
        bool isBreakout,
        Server server,
        bool autoStartRecording,
        PlatformTypes platformType
        )
    {
        IsValid(name, moderatorPassword, attendeePassword, limitCapacity, startDateTime);

        if (server is null)
            throw new ArgumentException($"The {nameof(server)} is Null Or Invalid.");

        ParentMeetingId = !isBreakout
            ? null
            : parentMeetingId;

        MeetingId = meetingId;
        IsRecord = isRecord;
        Name = name;
        ModeratorPassword = moderatorPassword;
        AttendeePassword = attendeePassword;
        StartDateTime = startDateTime;
        EndDateTime = endDateTime;
        IsRunning = true;
        LimitCapacity = limitCapacity;
        IsRunning = isRunning;
        IsBreakout = isBreakout;
        Server = server;
        AutoStartRecording = autoStartRecording;
        Platform = platformType;
    }

    public Meeting(
        string meetingId,
        bool isRecord,
        string name,
        string moderatorPassword,
        string attendeePassword,
        DateTime endDateTime,
        int limitCapacity
        )
    {
        MeetingId = meetingId;
        IsRecord = isRecord;
        Name = name;
        ModeratorPassword = moderatorPassword;
        AttendeePassword = attendeePassword;
        EndDateTime = endDateTime;
        IsRunning = true;
        LimitCapacity = limitCapacity;
    }
    public void Update(
        string name,
        string moderatorPassword,
        string attendeePassword,
        int limitCapacity
        )
    {
        IsValid(
            name,
            moderatorPassword,
            attendeePassword,
            limitCapacity,
            new DateTime());

        Name = name;
        ModeratorPassword = moderatorPassword;
        AttendeePassword = attendeePassword;
        LimitCapacity = limitCapacity;
    }

    public void CanFreeJoinOnBreakout(bool canJoin) =>
            FreeJoinOnBreakout = canJoin;

    public void SetPrivateMeeting(List<User> accessedUsers)
    {
        Type = MeetingTypes.Private;
        Users = accessedUsers;
        LimitCapacity = accessedUsers.Count();
    }

    public void EndSession(DateTime endDateTime)
        => (IsRunning, EndDateTime) = (false, endDateTime);
}
