using LIMS.Domain.Entities;

namespace LIMS.Domain.Entity;

public sealed class Meeting : BaseEntity
{
    public string MeetingId { get; private set; }
    public bool IsRecord { get; private set; }
    public string Name { get; private set; }
    public string ModeratorPassword { get; private set; }
    public string AttendeePassword { get; private set; }
    public DateTime StartDateTime { get; private set; }
    public DateTime EndDateTime { get; private set; }
    public bool IsRunning { get; private set; }
    public int LimitCapacity { get; private set; }

    public List<Record> Records { get; private set; }
    public Server Server { get; }
    public IReadOnlyList<User> Users { get; }
    public IReadOnlyList<MemberShip> MemberShips { get; }

    private Meeting() { }

    private void IsValid(
        string name,
        string moderatorPassword,
        string attendeePassword
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException($"The {nameof(name)} is Null Or Invalid.");
        if (string.IsNullOrWhiteSpace(moderatorPassword))
            throw new ArgumentNullException($"The {nameof(moderatorPassword)} is Null Or Invalid.");
        if (string.IsNullOrWhiteSpace(attendeePassword))
            throw new ArgumentNullException($"The {nameof(attendeePassword)} is Null Or Invalid.");
    }

    public Meeting(
        string meetingId,
        bool isRecord,
        string name,
        string moderatorPassword,
        string attendeePassword,
        DateTime startDateTime,
        DateTime endDateTime,
        int limitCapacity
    )
    {

    }

    public Meeting(
        string meetingId,
        bool isRecord,
        string name,
        string moderatorPassword,
        string attendeePassword,
        DateTime startDateTime,
        DateTime endDateTime,
        int limitCapacity
    )
    {
        IsValid(name, moderatorPassword, attendeePassword);
        if (startDateTime < DateTime.UtcNow)
            throw new ArgumentException($"The {nameof(startDateTime)} is Null Or Invalid.");
        if (endDateTime < DateTime.UtcNow)
            throw new ArgumentException($"The {nameof(endDateTime)} is Null Or Invalid.");
        if (limitCapacity <= 0)
            throw new ArgumentException($"The {nameof(LimitCapacity)}  Cant be zero or less");

        MeetingId = meetingId;
        IsRecord = isRecord;
        Name = name;
        ModeratorPassword = moderatorPassword;
        AttendeePassword = attendeePassword;
        StartDateTime = startDateTime;
        EndDateTime = endDateTime;
        IsRunning = true;
        LimitCapacity = limitCapacity;
    }
    public void Update(
        string name,
        string moderatorPassword,
        string attendeePassword,
        DateTime endDateTime, 
        int limitCapacity
        )
    {
        IsValid(
            name,
            moderatorPassword,
            attendeePassword);
        Name = name;
        ModeratorPassword = moderatorPassword;
        AttendeePassword = attendeePassword;
        EndDateTime = endDateTime;
        LimitCapacity = limitCapacity;
    }

    public void EndSession(DateTime now) => (IsRunning, EndDateTime) = (false, now);
}
