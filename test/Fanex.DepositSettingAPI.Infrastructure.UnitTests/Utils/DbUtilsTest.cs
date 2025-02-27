namespace Fanex.DepositSettingAPI.Infrastructure.UnitTests.Utils;

using System.Collections;
using Constants;
using FluentAssertions;
using Infrastructure.Utils;

public class DbUtilsTest
{
    [Fact]
    public void GetErrorOutputParameters_Should_ReturnExpectedResult()
    {
        // Arrange
        var output = new Hashtable
        {
            { DbParam.ErrCode, 123 },
            { DbParam.ErrMsg, "Error message" }
        };

        // Act
        var result = DbUtils.GetErrorOutputParameters(output);

        // Assert
        result.code.Should().Be(123);
        result.message.Should().Be("Error message");
    }
}