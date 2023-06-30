using System.Runtime.Serialization;

namespace Acl.Net.Core.Exceptions;

internal class RoleNotFoundException : Exception
{
    public RoleNotFoundException()
    {
    }

    public RoleNotFoundException(string message)
        : base(message)
    {
    }

    public RoleNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected RoleNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}