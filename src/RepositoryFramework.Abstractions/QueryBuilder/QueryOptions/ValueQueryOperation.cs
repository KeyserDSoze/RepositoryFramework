namespace RepositoryFramework
{
    public record ValueQueryOperation(QueryOperations Operation, long? Value) : QueryOperation(Operation);
}