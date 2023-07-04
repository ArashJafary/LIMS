using LIMS.Domain.Entities;

namespace LIMS.Domain.Entity;

public sealed class Meeting : BaseEntity
{
    public string MeetingId { get; private set; }
    public bool Record { get; private set; }
    public string Name { get; private set; }
    public string ModeratorPassword { get; private set; }
    public string AttendeePassword { get; private set; }
    public DateTime StartDateTime { get; private set; }
    public DateTime EndDateTime { get; private set; }
    public bool IsRunning { get; private set; }
    public int LimitCapacity { get; private set; }

    public Server Server { get; }
    public IReadOnlyList<User> Users { get; }
    public List<MemberShip> MemberShips { get; }

    private Meeting() { }

    public Meeting(bool record, string name, string moderatorPassword, string attendeePassword)
    {
        Record = record;
        Name = name;
        ModeratorPassword = moderatorPassword;
        AttendeePassword = attendeePassword;
    }
    public Meeting(
        string meetingId,
        bool recorded,
        string name,
        string moderatorPassword,
        string attendeePassword,
        DateTime startDateTime,
        DateTime endDateTime,
        int limitCapacity
    )
    {
        IsValid(recorded, name, moderatorPassword, attendeePassword);
        if (startDateTime < DateTime.UtcNow)
            throw new ArgumentException($"The {nameof(startDateTime)} is Null Or Invalid.");
        if (endDateTime < DateTime.UtcNow)
            throw new ArgumentException($"The {nameof(endDateTime)} is Null Or Invalid.");
        if (limitCapacity <= 0)
            throw new ArgumentException($"The {nameof(LimitCapacity)}  Cant be zero or less");

        StartDateTime = startDateTime;
        IsRunning = true;
        LimitCapacity = limitCapacity;
    }
    public void Update(
        bool record,
        string name,
        string moderatorPassword,
        string attendeePassword)
    {
        IsValid(
            record,
            name,
            moderatorPassword,
            attendeePassword);
        Name = name;
        Record = record;
        ModeratorPassword = moderatorPassword;
        AttendeePassword = attendeePassword;
    }

    public void EndSession(DateTime now) => (IsRunning, EndDateTime) = (false, now);

    private void IsValid(
    bool recorded,
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
}
