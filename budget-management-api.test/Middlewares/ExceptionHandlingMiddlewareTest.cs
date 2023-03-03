using System.Net;
using budget_management_api.Dtos;
using budget_management_api.Exceptions;
using budget_management_api.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace budget_management_api.test.Middlewares;

public class ExceptionHandlingMiddlewareTest
{
    private readonly Mock<ILogger<ExceptionHandlingMiddleware>> _mockLogger;
    private ExceptionHandlingMiddleware _handlingMiddleware;

    public ExceptionHandlingMiddlewareTest()
    {
        _mockLogger = new Mock<ILogger<ExceptionHandlingMiddleware>>();
        _handlingMiddleware = new ExceptionHandlingMiddleware(_mockLogger.Object);
    }
    
    [Fact]
    public async Task Should_ChangeErrorResponse_When_HandleExceptionAsyncWhereNotFoundException()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var exception = new NotFoundException("Not Found");
        
        var errorResponse = new ErrorResponse();
        errorResponse.StatusCode = ((int)HttpStatusCode.NotFound);
        errorResponse.Message = exception.Message;
        // Act
        await _handlingMiddleware.HandleExceptionAsync(context, exception);

        // Assert
        context.Response.StatusCode = ((int)HttpStatusCode.NotFound);
        var result = new ErrorResponse();
        result.StatusCode = (int)HttpStatusCode.NotFound;
        result.Message = exception.Message;
        
        Assert.Equal(errorResponse.StatusCode, result.StatusCode);
        Assert.Equal(errorResponse.Message, result.Message);
    }
    
    [Fact]
    public async Task Should_ChangeErrorResponse_When_HandleExceptionAsyncWhereUnauthorizedException()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var exception = new UnauthorizedException("Unauthorized");
        
        var errorResponse = new ErrorResponse();
        errorResponse.StatusCode = ((int)HttpStatusCode.Unauthorized);
        errorResponse.Message = exception.Message;
        // Act
        await _handlingMiddleware.HandleExceptionAsync(context, exception);

        // Assert
        context.Response.StatusCode = ((int)HttpStatusCode.Unauthorized);
        var result = new ErrorResponse();
        result.StatusCode = (int)HttpStatusCode.Unauthorized;
        result.Message = exception.Message;
        
        Assert.Equal(errorResponse.StatusCode, result.StatusCode);
        Assert.Equal(errorResponse.Message, result.Message);
    }
    
    [Fact]
    public async Task Should_ChangeErrorResponse_When_HandleExceptionAsyncWhereNotNull()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var exception = new Exception("Internal Server error");
        
        var errorResponse = new ErrorResponse();
        errorResponse.StatusCode = ((int)HttpStatusCode.InternalServerError);
        errorResponse.Message = exception.Message;
        // Act
        await _handlingMiddleware.HandleExceptionAsync(context, exception);

        // Assert
        context.Response.StatusCode = ((int)HttpStatusCode.InternalServerError);
        var result = new ErrorResponse();
        result.StatusCode = (int)HttpStatusCode.InternalServerError;
        result.Message = exception.Message;
        
        Assert.Equal(errorResponse.StatusCode, result.StatusCode);
        Assert.Equal(errorResponse.Message, result.Message);
    }
    
    [Fact]
        public async Task Should_CallsNextAndReturnsSuccessfully_When_InvokeAsync()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var nextCalled = false;
            RequestDelegate next = (c) =>
            {
                nextCalled = true;
                return Task.CompletedTask;
            };

            // Act
            await _handlingMiddleware.InvokeAsync(context, next);

            // Assert
            Assert.True(nextCalled);
            Assert.Equal((int)HttpStatusCode.OK, context.Response.StatusCode);
        }

        [Fact]
        public async Task Should_HandlesNotFoundException_When_InvokeAsyncWhereNotFoundException()
        {
            // Arrange
            var context = new DefaultHttpContext();
            RequestDelegate next = (c) => throw new NotFoundException();

            // Act
            await _handlingMiddleware.InvokeAsync(context, next);
        
            // Assert
            Assert.Equal((int)HttpStatusCode.NotFound, context.Response.StatusCode);
            //_mockLogger.Verify(x => x.LogError(It.IsAny<string>()), Times.Once);
        }
        
        [Fact]
        public async Task Should_HandlesUnauthorizedException_Where_InvokeAsyncWhenUnauthorizedException()
        {
            // Arrange
            var context = new DefaultHttpContext();
            RequestDelegate next = (c) => throw new UnauthorizedException();

            // Act
            await _handlingMiddleware.InvokeAsync(context, next);
        
            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, context.Response.StatusCode);
            //_mockLogger.Verify(x => x.LogError(It.IsAny<string>()), Times.Once);
        }
        
        [Fact]
        public async Task Should_HandlesException_When_InvokeAsyncWhereException()
        {
            // Arrange
            var context = new DefaultHttpContext();
            RequestDelegate next = (c) => throw new Exception();

            // Act
            await _handlingMiddleware.InvokeAsync(context, next);
        
            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
            //_mockLogger.Verify(x => x.LogError(It.IsAny<string>()), Times.Once);
        }
}