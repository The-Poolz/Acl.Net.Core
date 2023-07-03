﻿using Xunit;
using Acl.Net.Core.Exceptions;
using System.Runtime.Serialization;
using Acl.Net.Core.Tests.Exceptions.Testable;

namespace Acl.Net.Core.Tests.Exceptions;

public class RoleNotFoundExceptionTests : ExceptionSerializationTestBase<RoleNotFoundException, TestableRoleNotFoundException>
{
    [Fact]
    internal void GasPriceExceededException_SerializationTest()
    {
        const string expectedMessage = "exception message";

        RunSerializationTest(expectedMessage);

        var actualMessage = infoForGetObjectData.GetString("Message");
        Assert.Equal(expectedMessage, actualMessage);
    }

    protected override TestableRoleNotFoundException CreateTestableException(SerializationInfo info, StreamingContext context) =>
        new(info, context);
}