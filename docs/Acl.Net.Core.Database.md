# Acl.Net.Core.Database

The `Acl.Net.Core.Database` library is a part of the Acl.Net.Core namespace and provides the database handling components to manage Access Control Lists (ACLs) within your application.
It is designed to be used with Entity Framework Core, allowing you to define and manage entities such as Users, Roles, and Resources.

## Installation

To use Acl.Net.Core.Database, you will need to add it as a dependency to your project.
You can do this by adding it as a NuGet package:

### .NET CLI
```powershell
dotnet add package Acl.Net.Core.Database
```

### Package Manager
```powershell
Install-Package Acl.Net.Core.Database
```

## Usage
The library provides classes, interfaces, and Entity Framework context for managing access control in your application.

## Entities

### `Resource`
The `Resource` entity represents a resource in your application and is defined as follows:

```csharp
public class Resource : Resource<int> { }

public class Resource<TKey>
    where TKey : IEquatable<TKey>
{
    public TKey Id { get; set; }
    public string Name { get; set; }
    public virtual TKey RoleId { get; set; }
}
```

### `Role`
The `Role` entity represents a role in your application:

```csharp
public class Role : Role<int> { }

public class Role<TKey>
    where TKey : IEquatable<TKey>
{
    public TKey Id { get; set; }
    public string Name { get; set; }
}
```

### `User`
The `User` entity represents a user in your application:

```csharp
public class User : User<int> { }

public class User<TKey>
    where TKey : IEquatable<TKey>
{
    public TKey Id { get; set; }
    public string Name { get; set; }
    public TKey RoleId { get; set; }
}
```

## Data Seeding
### `IInitialDataSeeder` Interface
The `IInitialDataSeeder` interface is used to seed initial data into the database for roles:

```csharp
public interface IInitialDataSeeder<TKey, out TRole>
    where TKey : IEquatable<TKey>
    where TRole : Role<TKey>
{
    TRole SeedAdminRole();
    TRole SeedUserRole();
}
```

`RoleDataSeeder` Class
A default implementation of `IInitialDataSeeder` is provided, which seeds two roles: `AdminRole` and `UserRole`.

```csharp
public class RoleDataSeeder : IInitialDataSeeder<int, Role<int>>
{
    public Role<int> SeedAdminRole() { return new Role<int> { Id = 1, Name = "AdminRole" }; }
    public Role<int> SeedUserRole() { return new Role<int> { Id = 2, Name = "UserRole" }; }
}
```

## DbContext
### `AclDbContext` Classes
The library provides various `AclDbContext` classes that you can use to manage the entities in your application:

```csharp
// Default implementation using 'int' as the ID type
public class AclDbContext : AclDbContext<int>
{
    public AclDbContext() : base(new RoleDataSeeder()) { }
    public AclDbContext(DbContextOptions options) : base(options, new RoleDataSeeder()) { }
}

// Generic implementation for custom types
public abstract class AclDbContext<TKey> : AclDbContext<TKey, User<TKey>, Role<TKey>, Resource<TKey>> where TKey : IEquatable<TKey> { /* ... */ }
public abstract class AclDbContext<TKey, TUser, TRole, TResource> : DbContext where TKey : IEquatable<TKey> where TUser : User<TKey> where TRole : Role<TKey> where TResource : Resource<TKey> { /* ... */ }
```

These classes manage the `DbSet` properties for the `User`, `Role`, and `Resource` entities and handle entity configurations and seeding.