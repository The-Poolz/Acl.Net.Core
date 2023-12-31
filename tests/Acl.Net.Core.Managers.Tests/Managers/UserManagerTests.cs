﻿using Xunit;
using Acl.Net.Core.Database.Entities;
using Acl.Net.Core.Managers.Tests.Mock;

namespace Acl.Net.Core.Managers.Tests.Managers;

public class UserManagerTests
{
    private readonly UserManager _userManager;
    private readonly Role _roleForNewUsers;

    public UserManagerTests()
    {
        _userManager = new UserManager(InMemoryAclDbContext.CreateContext());
        _roleForNewUsers = new Role { Id = 3, Name = "NewUserRole" };
    }

    [Fact]
    public void UserProcessing_ShouldReturnExistingUser_WhenUserExists()
    {
        var user = _userManager.UserProcessing("UserAccount", _roleForNewUsers);
        Assert.Equal(1, user.Id);
    }

    [Fact]
    public void UserProcessing_ShouldAddNewUser_WhenUserNotExists()
    {
        var user = _userManager.UserProcessing("NewUser", _roleForNewUsers);
        Assert.Equal(_roleForNewUsers.Id, user.RoleId);
    }

    [Fact]
    public async Task UserProcessingAsync_ShouldReturnExistingUser_WhenUserExists()
    {
        var user = await _userManager.UserProcessingAsync("UserAccount", _roleForNewUsers);
        Assert.Equal(1, user.Id);
    }

    [Fact]
    public async Task UserProcessingAsync_ShouldAddNewUser_WhenUserNotExists()
    {
        var user = await _userManager.UserProcessingAsync("NewUser", _roleForNewUsers);
        Assert.Equal(_roleForNewUsers.Id, user.RoleId);
    }
}
