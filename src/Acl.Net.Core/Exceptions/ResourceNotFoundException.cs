using System.Runtime.Serialization;

namespace Acl.Net.Core.Exceptions;

[Serializable]
public class ResourceNotFoundException : Exception
{
    public ResourceNotFoundException(string resourceName)
        : base($"Resource with name '{resourceName}' not found.")
    { }

    protected ResourceNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    { }
}