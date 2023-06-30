using System.Runtime.Serialization;

namespace Acl.Net.Core.Exceptions;

internal class RoleParentNotFoundException : RoleNotFoundException
{
    public RoleParentNotFoundException()
    {
    }

    public RoleParentNotFoundException(string message)
        : base(message)
    {
    }

    public RoleParentNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected RoleParentNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}