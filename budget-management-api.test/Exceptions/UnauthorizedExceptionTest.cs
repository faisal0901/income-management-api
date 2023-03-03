using budget_management_api.Exceptions;

namespace budget_management_api.test.Exceptions;

public class UnauthorizedExceptionTest
{
    [Fact]
    public void Should_ReturnConstructor_When_UnauthorizedException()
    {
        var exception = new UnauthorizedException();
        Assert.IsType<UnauthorizedException>(exception);
    }

    [Fact]
    public void Should_ReturnMessageConstructor_When_UnauthorizedException()
    {
        var message = "Unauthorized message";
        var exception = new UnauthorizedException(message);
        Assert.IsType<UnauthorizedException>(exception);
        Assert.Equal(message, exception.Message);
    }
}