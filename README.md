# Acl.Net.Core

[![SonarCloud](https://sonarcloud.io/images/project_badges/sonarcloud-white.svg)](https://sonarcloud.io/summary/new_code?id=The-Poolz_Acl.Net.Core)

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=The-Poolz_Acl.Net.Core&metric=alert_status&token=ef989df8d43ee416fe8b310fb6b251226d57208b)](https://sonarcloud.io/summary/new_code?id=The-Poolz_Acl.Net.Core)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=The-Poolz_Acl.Net.Core&metric=bugs&token=ef989df8d43ee416fe8b310fb6b251226d57208b)](https://sonarcloud.io/summary/new_code?id=The-Poolz_Acl.Net.Core)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=The-Poolz_Acl.Net.Core&metric=security_rating&token=ef989df8d43ee416fe8b310fb6b251226d57208b)](https://sonarcloud.io/summary/new_code?id=The-Poolz_Acl.Net.Core)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=The-Poolz_Acl.Net.Core&metric=coverage&token=ef989df8d43ee416fe8b310fb6b251226d57208b)](https://sonarcloud.io/summary/new_code?id=The-Poolz_Acl.Net.Core)

[![GitHub Action](https://github.com/The-Poolz/Acl.Net.Core/actions/workflows/sonarcloud.yml/badge.svg)](https://github.com/The-Poolz/Acl.Net.Core/actions/workflows/sonarcloud.yml)
[![CodeFactor](https://www.codefactor.io/repository/github/the-poolz/acl.net.core/badge)](https://www.codefactor.io/repository/github/the-poolz/acl.net.core)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/The-Poolz/Acl.Net.Core/blob/master/LICENSE)

[![Acl.Net.Core.Database](https://badge.fury.io/nu/Acl.Net.Core.Database.svg)](https://badge.fury.io/nu/Acl.Net.Core.Database)
[![Acl.Net.Core.Managers](https://badge.fury.io/nu/Acl.Net.Core.Managers.svg)](https://badge.fury.io/nu/Acl.Net.Core.Managers)

Acl.Net.Core is a C# library that provides a simple and flexible way to manage Access Control Lists (ACLs). It is designed to be used with Entity Framework Core and allows you to easily define and manage Users, Roles, and Resources in your application.

> **Note**
> 
> `Acl.Net.Core` NuGet package not supporting starting with v1.0.0. This package has been split to two package `Acl.Net.Core.Database` and `Acl.Net.Core.Managers`

## Installation

To use `Acl.Net.Core`, need to install one of two package `Acl.Net.Core.Database` or `Acl.Net.Core.Managers`

- `Acl.Net.Core.Database`: provides EFCore DbContext for ACL system.
- `Acl.Net.Core.Managers`: provides AclManager for AclDbContext.

.NET CLI
```powershell
dotnet add package Acl.Net.Core.Database
dotnet add package Acl.Net.Core.Managers
```

Package Manager
```powershell
Install-Package Acl.Net.Core.Database
Install-Package Acl.Net.Core.Managers
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
The library provides a `RoleDataSeeder` class that seeds two roles: `Admin` and `User`.

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
The library provides a default implementation, `RoleDataSeeder`, which seeds role: `Admin`.

The `IInitialDataSeeder` interface is used in the `AclDbContext` and `AclManager` classes to seed the initial roles and to check permissions.
In the `AclManager` class, if a user has the `Admin` role, they are allowed access to any resource.

The `IInitialDataSeeder` interface defines method: `SeedAdminRole`.
These methods return instances of the `Role` entity that represent the admin role, respectively.

```csharp
public interface IInitialDataSeeder<TKey, out TRole>
    where TKey : IEquatable<TKey>
    where TRole : Role<TKey>
{
    TRole SeedAdminRole();
}
```

In the `RoleDataSeeder` class, the `SeedAdminRole` method returns a new `Role` with Id set to 1 and `Name` set to "Admin".

```csharp
public class RoleDataSeeder : IInitialDataSeeder<int, Role<int>>
{
    public Role<int> SeedAdminRole()
    {
        return new Role<int> { Id = 1, Name = "Admin" };
    }
}
```

## Managers

> **Note**
> 
> By default, for `Admin` allows any resource!

The library provides a `AclManager` class that you can use to manage access control in your application.
This class provides methods to check if a user is permitted to access a resource or resource list.

The `AclManager` class also has a generic version that allows you to specify the types of the entities.
This can be useful if you want to use your own entity classes that inherit from the provided entities.

The `AclManager` class provide methods to process users and resources, and to check if a user is permitted to access a resource.

```csharp
// Check if user "userName" permitted for call "resourceName"
var context = new MyDbContext();
var aclManager = new AclManager<Guid, MyUser, MyRole, MyResource>(context);
bool isPermitted = aclManager.IsPermitted("userName", "resourceName");
```

## Other documentation

- Acl.Net.Core.Database [docs](https://github.com/The-Poolz/Acl.Net.Core/blob/master/docs/Acl.Net.Core.Database.md)
- Acl.Net.Core.Managers [docs](https://github.com/The-Poolz/Acl.Net.Core/blob/master/docs/Acl.Net.Core.Managers.md)
