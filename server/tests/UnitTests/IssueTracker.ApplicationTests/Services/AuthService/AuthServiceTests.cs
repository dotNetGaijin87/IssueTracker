using FluentAssertions;
using IssueTracker.Application.Attributes;
using IssueTracker.Application.Services.IdentityService;
using IssueTracker.Domain.Models.Enums;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace IssueTracker.ApplicationTests.Services;

public class AuthServiceTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void IsAuthenticated_Success(bool isAuthenticated)
    {
        // ARRANGE
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(x => x.HttpContext.User.Identity.IsAuthenticated).Returns(isAuthenticated); 
        var sut = new AuthService(httpContextAccessorMock.Object);

        // ACT
        bool _isAuthenticated = sut.IsAuthenticated();

        // ASSERT
        _isAuthenticated.Should().Be(isAuthenticated);
    }

    [Theory]
    [InlineData(UserRole.manager, UserRole.manager, true)]
    [InlineData(UserRole.admin, UserRole.admin, true)]
    [InlineData(UserRole.manager, UserRole.admin, false)]
    [InlineData(UserRole.admin, UserRole.manager, false)]
    public void HasRequiredRole_Success(UserRole requiredRole, UserRole userRole, bool hasRequiredRole)
    {
        // ARRANGE
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var attributes = new List<AuthorizeUserAttribute> { new AuthorizeUserAttribute(requiredRole) };
        var sut = new AuthService(httpContextAccessorMock.Object);

        // ACT
        bool _hasRequiredRole = sut.HasRequiredRole(attributes, userRole);

        // ASSERT
        _hasRequiredRole.Should().Be(hasRequiredRole);
    }
}

