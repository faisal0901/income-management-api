namespace budget_management_api.Dtos;

public class CommonResponse<T>
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }

}