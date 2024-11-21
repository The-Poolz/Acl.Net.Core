namespace Acl.Net.Core.Managers.Exceptions;

/// <summary>
/// Represents an exception that is thrown when a specific user is not found.
/// </summary>
public class UserNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserNotFoundException"/> class with the specified user-name.
    /// </summary>
    /// <param name="userName">The name of the user that could not be found.</param>
    public UserNotFoundException(string userName)
        : base($"User with name '{userName}' not found.")
    { }
}