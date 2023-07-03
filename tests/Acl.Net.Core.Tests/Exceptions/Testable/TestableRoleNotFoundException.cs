using Acl.Net.Core.Exceptions;
using System.Runtime.Serialization;

namespace Acl.Net.Core.Tests.Exceptions.Testable;

public class TestableRoleNotFoundException : RoleNotFoundException
{
    public TestableRoleNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    { }
}
