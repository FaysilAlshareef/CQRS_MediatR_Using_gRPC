using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Students.Command.Test.Fakers;
using Students.Command.Test.Live.Asserts;
using Students.Command.Test.Protos;
using Task1.Command.Domain.Events;
using Task1.Command.Infra.Persistence;
using Xunit.Abstractions;

namespace Students.Command.Test.Live.Tests;
public class UpdateTest : TestBase
{
    private const int Delay = 10000;

    public UpdateTest(ITestOutputHelper output) : base(output)
    {
    }

    [Theory]
    [InlineData("b0896758-04de-4fd1-bff1-ce1c7be877b1", "Ali", "0912223344")]
    public async Task Update_UpdateStudentWithValidData_ReturnValid(
    string studentId,
    string name,
    string phoneNumber)
    {
        #region Arrange

        Initialize(configureTopic: s => { });

        var configuration = Factory.Services.GetRequiredService<IConfiguration>();

        var listener = new Listener(configuration);

        var grpcClient = new Student.StudentClient(Channel);
        using var scope = Factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var createEvent = new StudentAddedFaker()
            .RuleFor(c => c.AggregateId, Guid.Parse(studentId))
            .RuleFor(c => c.Data, new StudentAddedDataFaker()
            .Generate())
            .Generate();

        await context.EventStore.AddAsync(createEvent);
        await context.SaveChangesAsync();
        await context.BuildUniqueRecordsAsync();

        context.ChangeTracker.Clear();

        var UpdateRequest = new UpdateStudentRequest
        {
            StudentId = createEvent.AggregateId.ToString(),
            Name = name,
            PhoneNumber = phoneNumber
        };

        #endregion

        #region Act

        var response = await grpcClient.UpdateAsync(UpdateRequest);

        await Task.Delay(Delay);

        await listener.CloseAsync();


        var studentUpdated = await context.EventStore
                                .OfType<StudentUpdatedEvent>()
                                .SingleOrDefaultAsync();

        var outboxMessage = await context.OutboxMessages.SingleOrDefaultAsync();
        var message = listener.Messages.SingleOrDefault();

        #endregion

        #region Assert



        Assert.Null(outboxMessage);
        MessageAssert.AssertEquality(studentUpdated, message);

        #endregion
    }
}
