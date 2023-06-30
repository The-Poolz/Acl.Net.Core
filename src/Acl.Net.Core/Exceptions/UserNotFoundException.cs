using System.Runtime.Serialization;

namespace Acl.Net.Core.Exceptions;

internal class UserNotFoundException : Exception
{
    public UserNotFoundException()
    {
    }

    public UserNotFoundException(string message)
        : base(message)
    {
    }

    public UserNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected UserNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}