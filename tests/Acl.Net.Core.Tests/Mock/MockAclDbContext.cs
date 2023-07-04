using Moq;
using Acl.Net.Core.Entities;
using TestableDbContext.Mock;
using Acl.Net.Core.Cryptography;
using Acl.Net.Core.DataProvider;

namespace Acl.Net.Core.Tests.Mock;

internal static class MockAclDbContext
{
    internal static Mock<AclDbContext> MockContext
    {
        get
        {
            var mockDbSetClaims = MockDbContext.GetMockDbSet(Claims);
            var mockDbSetUsers = MockDbContext.GetMockDbSet(Users);
            var mockDbSetRoles = MockDbContext.GetMockDbSet(Roles);
            var mockDbSetResources = MockDbContext.GetMockDbSet(Resources);

            var mockContext = MockDbContext<AclDbContext>.GetMockContext(MockBehavior.Strict);

            mockContext.Setup(x => x.Claims).Returns(mockDbSetClaims.Object);
            mockContext.Setup(x => x.Users).Returns(mockDbSetUsers.Object);
            mockContext.Setup(x => x.Roles).Returns(mockDbSetRoles.Object);
            mockContext.Setup(x => x.Resources).Returns(mockDbSetResources.Object);

            return mockContext;
        }
    }

    internal static List<Claim> Claims => new()
    {
        new Claim
        {
            Id = 1,
            Token = UserTokenManager.GenerateToken(1),
            DateOfCreation = DateTime.UtcNow.AddDays(-1),
            UserId = 1
        },
        new Claim
        {
            Id = 2,
            Token = UserTokenManager.GenerateToken(1),
            DateOfCreation = DateTime.UtcNow,
            UserId = 1
        },
        new Claim
        {
            Id = 3,
            Token = UserTokenManager.GenerateToken(2),
            DateOfCreation = DateTime.UtcNow.AddDays(-1),
            UserId = 2
        },
        new Claim
        {
            Id = 4,
            Token = UserTokenManager.GenerateToken(2),
            DateOfCreation = DateTime.UtcNow,
            UserId = 2
        }
    };

    internal static List<User> Users => new()
    {
        new User { Id = 1 },
        new User { Id = 2 }
    };

    internal static List<Role> Roles => new()
    {
        new Role { Id = 1, UserId = 1, Name = "User" },
        new Role { Id = 2, UserId = 2, Name = "Admin" }
    };

    internal static List<Resource> Resources => new()
    {
        new Resource { Id = 1, Name = "publicResource", RoleId = 1 },
        new Resource { Id = 2, Name = "privateResource", RoleId = 2 }
    };
}