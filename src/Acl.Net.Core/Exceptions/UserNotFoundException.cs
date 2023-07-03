using System.Runtime.Serialization;

namespace Acl.Net.Core.Exceptions;

[Serializable]
public class UserNotFoundException : Exception
{
    public UserNotFoundException(string message)
        : base(message)
    {
    }

    protected UserNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}