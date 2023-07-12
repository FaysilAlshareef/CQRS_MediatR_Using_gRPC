﻿using Student = Students.Command.Test.Protos.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;
using Students.Command.Test.Protos;
using Microsoft.Extensions.DependencyInjection;
using Task1.CQRS_MediatR_Using_gRPC.Data;
using Microsoft.EntityFrameworkCore;
using Students.Command.Test.Asserts;
using Grpc.Core;
using Students.Command.Test.Fakers;
using Task1.CQRS_MediatR_Using_gRPC.Data.Entities;
using Task1.CQRS_MediatR_Using_gRPC.Models;

namespace Students.Command.Test.StudentTest;
public class CreateTest : TestBase
{
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
        var grpcRequest = new Student.StudentClient(Channel);
        var createRequest = new CreateRequest
        {
            Name = Name,
            Address = Address,
            PhoneNumber = PhoneNumber
        };

        //Act
        var response = await grpcRequest.CreateAsync(createRequest);
        using var scope = Factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var studentEvents = await context.EventStore.SingleOrDefaultAsync();
        var uniqueStudent = await context.UniqueReferences.SingleOrDefaultAsync();
        var messages = await context.OutboxMessages.SingleOrDefaultAsync();

        // Assert

        EventAssert.AssertEquality(
            createRequest,
            response.Output,
            studentEvents);

        Assert.NotEmpty(response.Message);
        Assert.Equal(response.Output.Id, uniqueStudent.Id.ToString());
        Assert.Equal(response.Output.Name, uniqueStudent.Name);

        EventAssert.AssertEquality(studentEvents, messages);

    }

    [Theory]
    [InlineData("", "Sabha", "0915555555", "Name")]
    [InlineData("Ali", "", "0915555555", "Address")]
    [InlineData("Ali", "Sabha", "091555555542422", "PhoneNumber")]
    [InlineData("Ali", "Sabha", "224", "PhoneNumber")]
    public async Task Create_CreateStudentWithInValidData_ReturnInvalidArgument(
        string Name,
        string Address,
        string PhoneNumber,
           string error
        )
    {
        //Arrange 
        var grpcRequest = new Student.StudentClient(Channel);
        var createRequest = new CreateRequest
        {
            Name = Name,
            Address = Address,
            PhoneNumber = PhoneNumber,
        };

        //Act

        var exeption = await Assert.ThrowsAsync<RpcException>(
                        async () => await grpcRequest.CreateAsync(createRequest));

        using var scope = Factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var studentEvents = await context.EventStore.SingleOrDefaultAsync();

        //Assert 
        Assert.Null(studentEvents);

        Assert.NotEmpty(exeption.Status.Detail);
        Assert.Equal(StatusCode.InvalidArgument, exeption.StatusCode);

        //Assert.Contains(
        //    exeption.GetValidationErrors()
        //    , e => e.PropertyName.EndsWith(error)
        //    );

    }

    [Fact]
    public async Task Create_CreateStudentWithDuplicateName_ReturnAlreadyExists()
    {
        // Arrange
        var name = "Faisal";
        using var scope = Factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var createdEvent = new StudentAddedFaker()
            .RuleFor(c => c.Data, new StudentAddedDataFaker()
            .RuleFor(d => d.Name, name)
            .Generate()).Generate();

        await context.EventStore.AddRangeAsync(createdEvent);

        await context.SaveChangesAsync();

    }
}
