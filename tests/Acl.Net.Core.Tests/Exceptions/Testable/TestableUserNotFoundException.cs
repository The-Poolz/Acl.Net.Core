using Acl.Net.Core.Exceptions;
using System.Runtime.Serialization;

namespace Acl.Net.Core.Tests.Exceptions.Testable;

public class TestableUserNotFoundException : UserNotFoundException
{
    public TestableUserNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    { }
}
