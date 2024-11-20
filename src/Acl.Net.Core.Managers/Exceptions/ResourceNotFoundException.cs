namespace Acl.Net.Core.Managers.Exceptions;

/// <summary>
/// Represents an exception that is thrown when a specific resource is not found.
/// </summary>
public class ResourceNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceNotFoundException"/> class with the specified resource name.
    /// </summary>
    /// <param name="resourceName">The name of the resource that could not be found.</param>
    public ResourceNotFoundException(string resourceName)
        : base($"Resource with name '{resourceName}' not found.")
    { }
}