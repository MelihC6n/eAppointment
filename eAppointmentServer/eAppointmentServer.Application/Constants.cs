using eAppointmentServer.Domain.Entities;

namespace eAppointmentServer.Application;
public static class Constants
{
    public static List<AppRole> Roles = new()
    {
        new()
        {
            Id = Guid.Parse("77b814bd-c248-418c-8544-36fa07217420"),
            Name = "Admin"
        },
        new()
        {
            Id = Guid.Parse("eeaf75de-0e23-4ff7-83b1-29b990aea117"),
            Name = "Doctor"
        },
        new()
        {
            Id = Guid.Parse("e988e526-a47a-4f92-9da6-603e0d8906e7"),
            Name = "Personel"
        },
    };
}