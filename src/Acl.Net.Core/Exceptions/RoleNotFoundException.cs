using System.Runtime.Serialization;

namespace Acl.Net.Core.Exceptions;

[Serializable]
public class RoleNotFoundException : Exception
{
    public RoleNotFoundException(string message)
        : base(message)
    {
    }

    protected RoleNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}