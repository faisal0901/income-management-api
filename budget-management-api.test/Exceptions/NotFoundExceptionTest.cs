using budget_management_api.Exceptions;

namespace budget_management_api.test.Exceptions;

public class NotFoundExceptionTest
{
    [Fact]
    public void Should_ReturnConstructor_When_NotFoundException()
    {
        var exception = new NotFoundException();
        Assert.IsType<NotFoundException>(exception);
    }

    [Fact]
    public void Should_ReturnMessageConstructor_When_NotFoundException()
    {
        var message = "Not Found Message";
        var exception = new NotFoundException(message);

        Assert.IsType<NotFoundException>(exception);
        Assert.Equal(message, exception.Message);
    }
}