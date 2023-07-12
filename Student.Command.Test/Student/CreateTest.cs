using Student = Students.Command.Test.Protos.Student;
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

    }

    [Theory]
    [InlineData("", "Sabha", "0915555555")]
    [InlineData("Ali", "", "0915555555")]
    [InlineData("Ali", "Sabha", "091555555542422")]
    [InlineData("Ali", "Sabha", "224")]
    public async Task Create_CreateStudentWithInValidData_ReturnInvalidArgument(
        string Name,
        string Address,
        string PhoneNumber
        )
    {

    }

    [Fact]
    public async Task Create_CreateStudentWithDuplicateName_ReturnAlreadyExists()
    {

    }
}
