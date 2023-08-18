namespace Acl.Net.Core.Managers;

public interface IAclManager
{
    public bool IsPermitted(string userName, string resourceName);

    public Task<bool> IsPermittedAsync(string userName, string resourceName);
}