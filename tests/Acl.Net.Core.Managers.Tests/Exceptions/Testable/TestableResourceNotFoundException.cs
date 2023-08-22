using System.Runtime.Serialization;
using Acl.Net.Core.Managers.Exceptions;

namespace Acl.Net.Core.Managers.Tests.Exceptions.Testable;

public class TestableResourceNotFoundException : ResourceNotFoundException
{
    public TestableResourceNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    { }
}
