namespace budget_management_api.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(){}
    public NotFoundException(string? message) : base(message){}
}