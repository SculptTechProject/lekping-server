namespace lekping.server.Domain.Enums
{
    public enum IntakeStatus
    {
        Planned = 0,   // planned, not sent
        Sent = 1,      // sent, not taken
        Taken = 2,     // user took the medicine
        Snoozed = 3,   // snoozed, for later (ex. 10 min)
        Missed = 4     // user missed the medicine
    }
}
