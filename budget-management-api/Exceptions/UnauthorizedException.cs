namespace budget_management_api.Exceptions;

public class UnauthorizedException : Exception
{
    public UnauthorizedException(){}
    public UnauthorizedException(string? message) : base(message){}
}