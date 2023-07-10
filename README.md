# Acl.Net.Core

Acl.Net.Core is a C# library that provides a simple and flexible way to manage Access Control Lists (ACLs). It is designed to be used with Entity Framework Core and allows you to easily define and manage Users, Roles, and Resources in your application.

## Installation

To use `Acl.Net.Core`, you will need to add it as a dependency to your project.
You can do this by adding it as a NuGet package:

.NET CLI
```powershell
dotnet add package Acl.Net.Core
```

Package Manager
```powershell
Install-Package Acl.Net.Core
```

## Usage

The library provides several classes and interfaces that you can use to manage access control in your application.

## Entities
The library defines three main entities: `User`, `Role`, and `Resource`.
Each of these entities has a generic version that allows you to specify the type of the ID.
The library provides default implementations that use `int` as the ID type.

### User
The `User` entity represents a user in your application.
It has an `Id`, a `Name`, and a `RoleId` property.
The `RoleId` property is used to associate the user with a role.

**Example using generic version**
```csharp
var user = new User<Guid>
{
    Id = Guid.NewGuid(),
    Name = "John Doe",
    RoleId = ROLE_GUID_HERE
};
```

### Role
The `Role` entity represents a role in your application.
It has an `Id` and a `Name` property.

**Example using generic version**
```csharp
var role = new Role<Guid>
{
    Id = Guid.NewGuid(),
    Name = "MyRole"
};
```

### Resource
The `Resource` entity represents a resource in your application.
It has an `Id`, a `Name`, and a `RoleId` property.
The `RoleId` property is used to associate the resource with a role.

**Example using generic version**
```csharp
var resource = new Resource<Guid>
{
    Id = Guid.NewGuid(),
    Name = "MyResource",
    RoleId = ROLE_GUID_HERE
};
```

## AclDbContext

The library provides a `AclDbContext` class that you can use to manage the entities in your application.
This class is a subclass of `DbContext` and provides `DbSet` properties for the `User`, `Role`, and `Resource` entities.

The `AclDbContext` class also has a generic version that allows you to specify the types of the entities.
This can be useful if you want to use your own entity classes that inherit from the provided entities.

The `AclDbContext` class uses an `IInitialDataSeeder` to seed initial data into the database.
The library provides a `RoleDataSeeder` class that seeds two roles: `AdminRole` and `UserRole`.

**Example using generic version**
```csharp
public class MyDbContext : AclDbContext<Guid, MyUser, MyRole, MyResource>
{
    public MyDbContext(DbContextOptions options, IInitialDataSeeder<Guid, MyRole> seeder)
        : base(options, seeder)
    { }
}
```

In this example, `MyDbContext` is a subclass of `AclDbContext` that uses `Guid` as the ID type and `MyUser`, `MyRole`, and `MyResource` as the entity types.

## `IInitialDataSeeder` Interface

The `IInitialDataSeeder` interface is a crucial part of the `Acl.Net.Core` library.
It is used to seed initial data into the database when the migrations applied.
The library provides a default implementation, `RoleDataSeeder`, which seeds two roles: `AdminRole` and `UserRole`.

The `IInitialDataSeeder` interface is used in the `AclDbContext` and `AclManager` classes to seed the initial roles and to check permissions.
In the `AclManager` class, if a user has the `AdminRole`, they are allowed access to any resource.

The `IInitialDataSeeder` interface defines two methods: `SeedAdminRole` and `SeedUserRole`.
These methods return instances of the `Role` entity that represent the admin and user roles, respectively.

```csharp
public interface IInitialDataSeeder<TKey, out TRole>
    where TKey : IEquatable<TKey>
    where TRole : Role<TKey>
{
    TRole SeedAdminRole();
    TRole SeedUserRole();
}
```

In the `RoleDataSeeder` class, the `SeedAdminRole` method returns a new `Role` with Id set to 1 and `Name` set to "AdminRole",
and the `SeedUserRole` method returns a new `Role` with `Id` set to 2 and `Name` set to "UserRole".

```csharp
public class RoleDataSeeder : IInitialDataSeeder<int, Role<int>>
{
    public Role<int> SeedAdminRole()
    {
        return new Role<int> { Id = 1, Name = "AdminRole" };
    }

    public Role<int> SeedUserRole()
    {
        return new Role<int> { Id = 2, Name = "UserRole" };
    }
}
```

## Managers

The library provides a `AclManager` class that you can use to manage access control in your application.
This class provides methods to check if a user is permitted to access a resource.

The `AclManager` class also has a generic version that allows you to specify the types of the entities.
This can be useful if you want to use your own entity classes that inherit from the provided entities.

The `AclManager` class uses a `UserManager` and a `ResourceManager` to manage users and resources.
These classes provide methods to process users and resources, and to check if a user is permitted to access a resource.

```csharp
var aclManager = new AclManager<Guid, MyUser, MyRole, MyResource>(seeder, userManager, resourceManager);
bool isPermitted = aclManager.IsPermitted("userName", "resourceName");
```

In this example, `userName` is the name of the user and `resourceName` is the name of the resource.
The `IsPermitted` method returns `true` if the user is permitted to access the resource, and `false` otherwise.

## Conclusion

`Acl.Net.Core` is a powerful and flexible library for managing access control in C# applications.
It provides a simple and intuitive API, and it integrates seamlessly with Entity Framework Core.
Whether you are building a small application or a large enterprise system, `Acl.Net.Core` can help you manage access control effectively and efficiently.
