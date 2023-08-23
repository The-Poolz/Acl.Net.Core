using Xunit;
using System.Runtime.Serialization;
using Acl.Net.Core.Managers.Exceptions;
using Acl.Net.Core.Managers.Tests.Exceptions.Testable;

namespace Acl.Net.Core.Managers.Tests.Exceptions;

public class ResourceNotFoundExceptionTests : ExceptionSerializationTestBase<ResourceNotFoundException, TestableResourceNotFoundException>
{
    [Fact]
    internal void GasPriceExceededException_SerializationTest()
    {
        const string expectedMessage = "exception message";

        RunSerializationTest(expectedMessage);

        var actualMessage = infoForGetObjectData.GetString("Message");
        Assert.Equal(expectedMessage, actualMessage);
    }

    protected override TestableResourceNotFoundException CreateTestableException(SerializationInfo info, StreamingContext context) => new(info, context);
}