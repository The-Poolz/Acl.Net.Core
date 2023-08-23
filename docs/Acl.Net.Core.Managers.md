# Acl.Net.Core.Managers

The `Acl.Net.Core.Managers` library is a part of the Acl.Net.Core namespace designed to simplify the management of Access Control Lists (ACLs) in .NET Core applications.
The package provides user, resource, and ACL management classes, supporting both synchronous and asynchronous operations.

## Installation

To use Acl.Net.Core.Database, you will need to add it as a dependency to your project.
You can do this by adding it as a NuGet package:

### .NET CLI
```powershell
dotnet add package Acl.Net.Core.Managers
```

### Package Manager
```powershell
Install-Package Acl.Net.Core.Managers
```

## Overview

The main components of this library include:

- `AclManager`: Manages permissions and provides methods to check permissions for resource(s).
- `ResourceManager`: Manages resources and provides methods to check permissions for resources.
- `UserManager`: Manages users and their roles.

## Managers

### AclManager

AclManager serves as the core class for managing permissions.
Also AclManager implement `IDisposable` interface, because of this important use `using` statement, or call `Dispose()` method.

AclManager method list:
```csharp
public virtual bool IsPermitted(string userName, string resourceName)
public virtual bool IsPermitted(TUser user, string resourceName)
public virtual bool IsPermitted(string userName, TResource resource)
public virtual bool IsPermitted(TUser user, TResource resource)
public virtual IEnumerable<TResource> IsPermitted(string userName, IEnumerable<string> resourceNames)
public virtual async Task<bool> IsPermittedAsync(string userName, string resourceName)
public virtual async Task<bool> IsPermittedAsync(TUser user, string resourceName)
public virtual async Task<bool> IsPermittedAsync(string userName, TResource resource)
public virtual async Task<bool> IsPermittedAsync(TUser user, TResource resource)
public virtual async Task<IEnumerable<TResource>> IsPermittedAsync(string userName, IEnumerable<string> resourceNames)
public void Dispose()
```

### ResourceManager

ResourceManager handles the retrieval and permission checking related to resources.
Also ResourceManager implement `IDisposable` interface, because of this important use `using` statement, or call `Dispose()` method.

ResourceManager method list:
```csharp
public virtual bool IsPermitted(TUser user, string resourceName)
public virtual bool IsPermitted(TUser user, TResource resource)
public virtual IEnumerable<TResource> IsPermitted(TUser user, IEnumerable<string> resourceNames)
public virtual IEnumerable<TResource> IsPermitted(TUser user, IEnumerable<TResource> resources)
public virtual async Task<bool> IsPermittedAsync(TUser user, string resourceName)
public virtual async Task<bool> IsPermittedAsync(TUser user, TResource resource)
public virtual async Task<IEnumerable<TResource>> IsPermittedAsync(TUser user, IEnumerable<string> resourceNames)
public virtual async Task<IEnumerable<TResource>> IsPermittedAsync(TUser user, IEnumerable<TResource> resources)
public virtual TResource GetResourceByName(string resourceName)
public virtual async Task<TResource> GetResourceByNameAsync(string resourceName)
public void Dispose()
```

### UserManager

UserManager manages user retrieval and processing.
When try to find a user, an existing one will be returned, or a new one will be created.
Also UserManager implement `IDisposable` interface, because of this important use `using` statement, or call `Dispose()` method.

UserManager method list:
```csharp
public virtual TUser UserProcessing(string userName, TRole roleForNewUsers)
public virtual async Task<TUser> UserProcessingAsync(string userName, TRole roleForNewUsers)
public void Dispose()
```

## Usage Examples

Below is a simple example of how you might use AclManager to check if a user has permission for a specific resource.

```csharp
using var context = new Acl.Net.Core.Database.AclDbContext();
using var userManager = new UserManager(context);
using var resourceManager = new ResourceManager(context);
using var aclManager = new AclManager(userManager, resourceManager);

bool isPermitted = aclManager.IsPermitted("userName", "resourceName");

if (isPermitted)
{
    // Access granted
}
else
{
    // Access denied
}
```

Below is a simple example of how you might use AclManager to check if a user has permission for a resource list.

```csharp
using var context = new Acl.Net.Core.Database.AclDbContext();
using var userManager = new UserManager(context);
using var resourceManager = new ResourceManager(context);
using var aclManager = new AclManager(userManager, resourceManager);
var resources = new[] { "privateResource", "publicResource" };

var permittedResources = aclManager.IsPermitted("userName", resources);

if (permittedResources.Any())
{
    // Access granted
}
else
{
    // Access denied
}
```

## Async Support

Most of the methods in this library have asynchronous counterparts, enabling seamless integration with async/await patterns.

## Dependency Injection

This library is designed to work well with dependency injection, allowing you to easily wire up the dependencies in a modern .NET application.