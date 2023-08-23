using System.Runtime.Serialization;

namespace Acl.Net.Core.Managers.Exceptions;

/// <summary>
/// Represents an exception that is thrown when a specific resource is not found.
/// </summary>
[Serializable]
public class ResourceNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceNotFoundException"/> class with the specified resource name.
    /// </summary>
    /// <param name="resourceName">The name of the resource that could not be found.</param>
    public ResourceNotFoundException(string resourceName)
        : base($"Resource with name '{resourceName}' not found.")
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceNotFoundException"/> class with serialized data.
    /// </summary>
    /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
    protected ResourceNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    { }
}