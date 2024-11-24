# Acl.Net.Core.Managers

The `Acl.Net.Core.Managers` library is a part of the `Acl.Net.Core` namespace designed to simplify the management of Access Control Lists (ACLs) in .NET Core applications.

## Installation

To use `Acl.Net.Core.Managers`, you will need to add it as a dependency to your project.
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

- `AclManager`: Manages permissions and provides methods to check permissions for resource.

## Managers

### AclManager

`AclManager` serves as the core class for managing permissions. `AclManager` method list:
```csharp
public bool IsPermitted(string userName, string resourceName);
public bool IsPermitted(TUser user, TResource resource);
public bool IsPermitted(TRole role, TResource resource);
```

## Usage Examples

Below is a simple example of how you might use AclManager to check if a user has permission for a specific resource.

```csharp
var context = new AclDbContext();
var aclManager = new AclManager(context);

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
