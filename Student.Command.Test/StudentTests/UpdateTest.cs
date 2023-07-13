using Bogus.DataSets;
using Calzolari.Grpc.Net.Client.Validation;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Students.Command.Test.Asserts;
using Students.Command.Test.Fakers;
using Students.Command.Test.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task1.CQRS_MediatR_Using_gRPC.Data;
using Task1.CQRS_MediatR_Using_gRPC.Enums;
using Task1.CQRS_MediatR_Using_gRPC.Events;
using Task1.CQRS_MediatR_Using_gRPC.Extensions;

using Xunit;
using Xunit.Abstractions;
using Student = Students.Command.Test.Protos.Student;


namespace Students.Command.Test.StudentTests;
public class UpdateTest : TestBase
{
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

        var grpcClient = new Student.StudentClient(Channel);
        using var scope = Factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var createEvent = new StudentAddedFaker()
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

        var studentUpdated = await context.EventStore
                                .SingleOrDefaultAsync(e => e.Type == EventType.StudentUpdated);

        var message = await context.OutboxMessages.Include(o => o.Event).SingleOrDefaultAsync();
        var uniqueReferences = await context.UniqueReferences.SingleOrDefaultAsync();
        #endregion

        #region Assert

        EventAssert.AssertEquality(
            UpdateRequest,
            studentUpdated,
            2);
        EventAssert.AssertEquality(message, studentUpdated);

        Assert.NotNull(uniqueReferences);
        Assert.NotEmpty(response.Message);
        #endregion
    }

    [Fact]
    public async Task Update_UpdateStudentNoDataChange_ReturnValid()
    {
        #region Arrange

        var grpcClient = new Student.StudentClient(Channel);
        using var scope = Factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var createEvent = new StudentAddedFaker()
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
            Name = createEvent.Data.Name,
            PhoneNumber = createEvent.Data.Phone_Number
        };

        #endregion

        #region Act

        var response = await grpcClient.UpdateAsync(UpdateRequest);

        var studentUpdated = await context.EventStore
                                .SingleOrDefaultAsync(e => e.Type == EventType.StudentUpdated);

        var message = await context.OutboxMessages.Include(o => o.Event).SingleOrDefaultAsync();
        var uniqueReferences = await context.UniqueReferences.SingleOrDefaultAsync();
        #endregion


        #region Assers

        Assert.Null(studentUpdated);
        Assert.Equal(0, await context.OutboxMessages.CountAsync());

        Assert.Equal(UpdateRequest.StudentId, response.Output.Id);
        Assert.Equal(UpdateRequest.Name, response.Output.Name);
        Assert.Equal(UpdateRequest.PhoneNumber, response.Output.PhoneNumber);

        Assert.NotEmpty(response.Message);

        #endregion
    }


    [Fact]
    public async Task Update_UpdateStudentWithSameDataTwice_SingleEventSaved()
    {
        #region Arrange

        var grpcClient = new Student.StudentClient(Channel);
        using var scope = Factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var createEvent = new StudentAddedFaker()
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
            Name = createEvent.Data.Name,
            PhoneNumber = createEvent.Data.Phone_Number
        };

        #endregion

        #region Act

        await grpcClient.UpdateAsync(UpdateRequest);
        await grpcClient.UpdateAsync(UpdateRequest);

        #endregion
        #region Assert

        Assert.Equal(2, await context.EventStore.CountAsync());
        Assert.Equal(1, await context.OutboxMessages.CountAsync());

        #endregion
    }

    [Fact]
    public async Task Update_UpdateStudentWithValidData_SaveTheCorrectSequence()
    {
        #region Arrange
        var grpcClient = new Student.StudentClient(Channel);
        using var scope = Factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var createEvent = new StudentAddedFaker()
            .RuleFor(c => c.Data, new StudentAddedDataFaker()
            .Generate())
            .Generate();

        var updateEvent = new StudentUpdatedFaker()
            .RuleFor(c => c.AggregateId, createEvent.AggregateId)
            .RuleFor(c => c.Sequence, 1)
            .RuleFor(c => c.Data, new StudentUpdatedDataFaker()
            .Generate())
            .Generate();

        await context.EventStore.AddAsync(createEvent);
        await context.EventStore.AddAsync(updateEvent);

        await context.SaveChangesAsync();
        await context.BuildUniqueRecordsAsync();

        context.ChangeTracker.Clear();

        var UpdateRequest = new UpdateStudentRequest
        {
            StudentId = createEvent.AggregateId.ToString(),
            Name = "ali",
            PhoneNumber = "0959992244"
        };

        #endregion

        #region Act

        var response = await grpcClient.UpdateAsync(UpdateRequest);

        var studentUpdated = await context.EventStore
                                            .OrderByDescending(e => e.Sequence)
                                            .FirstOrDefaultAsync(e => e.Type == EventType.StudentUpdated);
        #endregion

        #region Assert
        EventAssert.AssertEquality(
            UpdateRequest, studentUpdated, 3);

        Assert.NotEmpty(response.Message);

        #endregion
    }


    [Theory]
    [InlineData("", "Faysil", "0915554433", "StudentId")]
    [InlineData("e477cd72-541c-4a4a-bf9d-5e86c21232d7", "", "0915554433", "Name")]
    [InlineData("e477cd72-541c-4a4a-bf9d-5e86c21232d7", "Faysil", "091555", "PhoneNumber")]
    [InlineData("e477cd72-541c-4a4a-bf9d-5e86c21232d7", "Faysil", "09155555454545", "PhoneNumber")]
    public async Task Update_UpdateStudentWithInvalidDate_ReturnInvalidArgument(
         string studentId,
        string name,
        string phoneNumber,
        string error)
    {
        #region Arrange

        var grpcClient = new Student.StudentClient(Channel);
        using var scope = Factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var createEvent = new StudentAddedFaker()
            .RuleFor(c => c.Data, new StudentAddedDataFaker()
            .Generate())
            .Generate();

        await context.EventStore.AddAsync(createEvent);
        await context.SaveChangesAsync();
        await context.BuildUniqueRecordsAsync();

        context.ChangeTracker.Clear();

        var updateRequest = new UpdateStudentRequest
        {
            StudentId = createEvent.AggregateId.ToString(),
            Name = name,
            PhoneNumber = phoneNumber
        };

        #endregion

        #region Act
        var exception = await Assert.ThrowsAsync<RpcException>(
                async () => await grpcClient.UpdateAsync(updateRequest));

        #endregion

        #region Assert
        Assert.NotEmpty(exception.Status.Detail);

        Assert.Equal(StatusCode.InvalidArgument, exception.StatusCode);

        Assert.Contains(
            exception.GetValidationErrors(),
            e => e.PropertyName.EndsWith(error)
        );

        var count = await context.EventStore.CountAsync();

        Assert.Equal(1, count);
        #endregion
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Update_UpdateForNonExistedStudent_ReturnNotFound(bool emptyGuid)
    {

        #region Arrange
        var grpcClient = new Student.StudentClient(Channel);

        var updateRequest = new UpdateStudentRequest
        {
            StudentId = (emptyGuid ? Guid.Empty : Guid.NewGuid()).ToString(),
            Name = "Faisal",
            PhoneNumber = "0915556644"
        };
        #endregion

        #region Act
        var exception = await Assert.ThrowsAsync<RpcException>(
                async () => await grpcClient.UpdateAsync(updateRequest));

        #endregion

        #region Assert
        Assert.NotEmpty(exception.Status.Detail);

        Assert.Equal(StatusCode.NotFound, exception.StatusCode);

        #endregion
    }


    [Fact]
    public async Task Update_UpdateStudentWithDuplicateName_ReturnAlreadyExists()
    {
        #region Arrange


        var firstName = "Faisal";
        var secondName = "Faisal";
        var grpcClient = new Student.StudentClient(Channel);

        using var scope = Factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var createdFirstEvent = new StudentAddedFaker()
            .RuleFor(c => c.Data, new StudentAddedDataFaker()
            .RuleFor(d => d.Name, firstName)
            .Generate()).Generate();

        var createdSecondEvent = new StudentAddedFaker()
           .RuleFor(c => c.Data, new StudentAddedDataFaker()
           .RuleFor(d => d.Name, secondName)
           .Generate()).Generate();

        await context.EventStore.AddAsync(createdFirstEvent);
        await context.EventStore.AddAsync(createdSecondEvent);
        await context.SaveChangesAsync();
        await context.BuildUniqueRecordsAsync();

        context.ChangeTracker.Clear();

        var updateRequest = new UpdateStudentRequest
        {
            StudentId = createdSecondEvent.AggregateId.ToString(),
            Name = firstName,
            PhoneNumber = createdSecondEvent.Data.Phone_Number
        };

        #endregion

        #region Act
        var exception = await Assert.ThrowsAsync<RpcException>(
         async () => await grpcClient.UpdateAsync(updateRequest));


        #endregion
        #region Assert
        Assert.NotEmpty(exception.Status.Detail);

        Assert.Equal(StatusCode.AlreadyExists, exception.StatusCode);
        #endregion

    }
}
