using Acl.Net.Core.Cryptography;
using Acl.Net.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using TestableDbContext.Mock;

namespace Acl.Net.Core.Tests;

public class AclManagerTests
{

    private void SetupMockDbContext()
    {
        var mockClaims = new List<Claim>
        {
            new()
            {
                Id = 1,
                Token = UserTokenManager.GenerateToken(1),
                DateOfCreation = DateTime.UtcNow.AddDays(-1),
                UserId = 1
            },
            new()
            {
                Id = 2,
                Token = UserTokenManager.GenerateToken(1),
                DateOfCreation = DateTime.UtcNow,
                UserId = 1
            },
            new()
            {
                Id = 3,
                Token = UserTokenManager.GenerateToken(2),
                DateOfCreation = DateTime.UtcNow.AddDays(-1),
                UserId = 2
            },
            new()
            {
                Id = 4,
                Token = UserTokenManager.GenerateToken(2),
                DateOfCreation = DateTime.UtcNow,
                UserId = 2
            }
        }.AsQueryable();

        var mockUsers = new List<User>
        {
            new() { Id = 1 },
            new() { Id = 2 }
        }.AsQueryable();

        var mockRoles = new List<Role>
        {
            new() { Id = 1, UserId = 1, Name = "User" },
            new() { Id = 2, UserId = 2, Name = "Admin" }
        }.AsQueryable();

        var mockResources = new List<Resource>
        {
            new() { Id = 1, Name = "publicResource", RoleId = 1 },
            new() { Id = 2, Name = "privateResource", RoleId = 2 }
        }.AsQueryable();
    }
}