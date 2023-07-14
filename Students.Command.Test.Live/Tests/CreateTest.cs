using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Students.Command.Test.Asserts;
using Students.Command.Test.Live.Asserts;
using Students.Command.Test.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task1.CQRS_MediatR_Using_gRPC.Data;
using Task1.CQRS_MediatR_Using_gRPC.Data.Entities;
using Task1.CQRS_MediatR_Using_gRPC.Events;
using Xunit.Abstractions;

namespace Students.Command.Test.Live.Tests;
public class CreateTest : TestBase
{
    private const int Delay = 10000;

    public CreateTest(ITestOutputHelper output) : base(output)
    {
    }

    [Theory]
    [InlineData("Ali", "Sabha", "0915555555")]
    public async Task Create_CreateStudentWithValidData_ReturnValid(
    string Name,
    string Address,
    string PhoneNumber
    )
    {
        //Arrange 
        Initialize(configureTopic: s => { });

        var configuration = Factory.Services.GetRequiredService<IConfiguration>();

        var listener = new Listener(configuration);

        var grpcRequest = new Student.StudentClient(Channel);
        var createRequest = new CreateRequest
        {
            Name = Name,
            Address = Address,
            PhoneNumber = PhoneNumber
        };

        //Act
        var response = await grpcRequest.CreateAsync(createRequest);
        await Task.Delay(Delay);

        using var scope = Factory.Services.CreateScope();
        await listener.CloseAsync();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var studentEvent = await context.EventStore
                                    .OfType<StudentAddedEvent>().SingleOrDefaultAsync();
        var outboxMessage = await context.OutboxMessages.Include(o => o.Event).SingleOrDefaultAsync();

        var message = listener.Messages.SingleOrDefault();


        // Assert

        Assert.Null(outboxMessage);

        Assert.Equal(1, await context.EventStore.CountAsync());

        MessageAssert.AssertEquality(studentEvent, message);

    }

}
