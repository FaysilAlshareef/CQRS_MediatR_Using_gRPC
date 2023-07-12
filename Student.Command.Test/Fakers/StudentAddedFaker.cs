using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task1.CQRS_MediatR_Using_gRPC.Events;
using Task1.CQRS_MediatR_Using_gRPC.Events.DataTypes;

namespace Students.Command.Test.Fakers;
public class StudentAddedFaker : PrivateFaker<StudentAddedEvent>
{
    public StudentAddedFaker()
    {
        UsePrivateConstructor();
        RuleFor(r => r.AggregateId, f => f.Random.Guid());
        RuleFor(r => r.Sequence, 1);
        RuleFor(r => r.Version, 1);
        RuleFor(r => r.UserId, f => f.Random.Guid().ToString());
        RuleFor(r => r.DateTime, DateTime.UtcNow);
    }


}

public class StudentAddedDataFaker : PrivateFaker<StudentAddedData>
{
    public StudentAddedDataFaker()
    {
        UsePrivateConstructor();
        RuleFor(r => r.Name, f => f.Random.AlphaNumeric(10));
        RuleFor(r => r.Address, f => f.Random.AlphaNumeric(5));
        RuleFor(r => r.Phone_Number, f => f.Random.AlphaNumeric(10));
    }
}
