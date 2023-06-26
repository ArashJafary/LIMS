using Microsoft.Identity.Client;

namespace BigBlueApi.Domain;

public class Session
{
    public string MeetingId { get; private set; }
    public bool Recorded { get; private set; }
    public string Name { get; private set; }
    public string ModeratorPassword { get; private set; }
    public string AttendeePassword { get; private set; }
    public DateTime StartDateTime { get; private set; }
    public DateTime EndDateTime { get; private set; }
    public bool IsRunning { get; set; }
    public Server Server { get; }
    public IReadOnlyList<User> Users { get; }
    public int LimitCapacity { get; set; }

    private Session() { }

    public Session(bool record,string name,string moderatorpassword,string attendeepassword)
    {
        Recorded = record;
        Name = name;
        ModeratorPassword = moderatorpassword;
        AttendeePassword = attendeepassword;
    }
    public Session(
        string meetingId,
        bool recorded,
        string name,
        string moderatorPassword,
        string attendeePassword,
        DateTime startDateTime,
        DateTime endDateTime,
        int limitcapacity
    )
    {
        IsValid(meetingId, recorded, name, moderatorPassword, attendeePassword);
        if (startDateTime < DateTime.UtcNow)
            throw new ArgumentException($"The {nameof(startDateTime)} is Null Or Invalid.");
        if (endDateTime < DateTime.UtcNow)
            throw new ArgumentException($"The {nameof(endDateTime)} is Null Or Invalid.");
        if (LimitCapacity <= 0)
            throw new ArgumentException($"The {nameof(LimitCapacity)}  Cant be zero or less");

        StartDateTime = startDateTime;
        IsRunning = true;
        LimitCapacity = limitcapacity;
    }
    public void Update(string meetingId,
        bool recorded,
        string name,
        string ModertorPass,
        string attendeePass)
    {
        IsValid(meetingId,
            recorded,
            name,
            ModertorPass,
            attendeePass);
    }


    private void IsValid(
    string meetingId,
    bool recorded,
    string name,
    string moderatorPassword,
    string attendeePassword
)
    {
        if (string.IsNullOrWhiteSpace(meetingId))
            throw new ArgumentNullException($"The {nameof(meetingId)} is Null Or Invalid.");
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException($"The {nameof(name)} is Null Or Invalid.");
        if (string.IsNullOrWhiteSpace(moderatorPassword))
            throw new ArgumentNullException($"The {nameof(moderatorPassword)} is Null Or Invalid.");
        if (string.IsNullOrWhiteSpace(attendeePassword))
            throw new ArgumentNullException($"The {nameof(attendeePassword)} is Null Or Invalid.");
        MeetingId = meetingId;
        Name = name;
        Recorded = recorded;
        ModeratorPassword = moderatorPassword;
        AttendeePassword = attendeePassword;
    }
}
