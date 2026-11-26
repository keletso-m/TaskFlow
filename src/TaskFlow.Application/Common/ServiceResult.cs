namespace TaskFlow.Application.Common;

public enum ServiceStatus { Ok, NotFound, Forbidden, Invalid }

public class ServiceResult<T>
{
    public ServiceStatus Status { get; init; }
    public T? Value { get; init; }
    public string? Error { get; init; }

    public static ServiceResult<T> Ok(T value) => new() { Status = ServiceStatus.Ok, Value = value };
    public static ServiceResult<T> Fail(ServiceStatus status, string error) => new() { Status = status, Error = error };
}
