namespace budget_management_api.Repositories;

public interface IPersistence
{
    Task SaveChangesAsync();
    Task<TResult> ExecuteTransactionAsync<TResult>(Func<Task<TResult>> func);
}