using IssueTracker.ApplicationTests.Helpers;
using Xunit;

namespace IssueTracker.ApplicationTests;

public class AutomapperTests
{
    [Fact]
    public void AutoMapper_AssertConfigurationIsValid()
    {
        // ARRANGE
        var sut = AutoMapperHelpers.CreateAutoMapper();


        // ACT
        // ASSERT
        sut.ConfigurationProvider.AssertConfigurationIsValid();
    }

}

