namespace lekping.server.Domain.Enums
{
    public enum IntakeStatus
    {
        Planned = 0,   // zaplanowane, jeszcze nie wysłane przypomnienie
        Sent = 1,      // wysłane przypomnienie
        Taken = 2,     // użytkownik potwierdził przyjęcie
        Snoozed = 3,   // odłożone np. na 10 minut
        Missed = 4     // termin minął i lek nie został przyjęty
    }
}
