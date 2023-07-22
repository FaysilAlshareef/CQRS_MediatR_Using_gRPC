using Task1.Command.Domain.Events;
using Task1.Command.Domain.Events.DataTypes;

namespace Students.Command.Test.Fakers;
public class StudentUpdatedFaker : PrivateFaker<StudentUpdatedEvent>
{
    public StudentUpdatedFaker()
    {
        UsePrivateConstructor();
        RuleFor(r => r.Version, 1);
        RuleFor(r => r.UserId, f => f.Random.Guid().ToString());
        RuleFor(r => r.DateTime, DateTime.UtcNow);
    }
}

public class StudentUpdatedDataFaker : PrivateFaker<StudentUpdatedData>
{
    public StudentUpdatedDataFaker()
    {
        UsePrivateConstructor();
        RuleFor(r => r.Name, f => f.Random.AlphaNumeric(10));
        RuleFor(r => r.Phone_Number, f => f.Random.AlphaNumeric(10));
    }
}
