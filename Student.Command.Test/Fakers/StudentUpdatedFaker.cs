using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task1.CQRS_MediatR_Using_gRPC.Events;
using Task1.CQRS_MediatR_Using_gRPC.Events.DataTypes;

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
