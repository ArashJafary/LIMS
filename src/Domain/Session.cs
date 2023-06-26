namespace BigBlueApi.Domain;

public class Session
{
    public int Id { get; }
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
    


    public List<MemberShip> MemberShips { get; set; }

    private Session() { }

    public Session(
        string meetingId,
        bool recorded,
        string name,
        string moderatorPassword,
        string attendeePassword,
        DateTime startDateTime,
        DateTime endDateTime
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
        if (startDateTime < DateTime.UtcNow)
            throw new ArgumentException($"The {nameof(startDateTime)} is Null Or Invalid.");
        if (endDateTime < DateTime.UtcNow)
            throw new ArgumentException($"The {nameof(endDateTime)} is Null Or Invalid.");
        MeetingId = meetingId;
        Name = name;
        ModeratorPassword = moderatorPassword;
        AttendeePassword = attendeePassword;
        StartDateTime = startDateTime;
    }
}
