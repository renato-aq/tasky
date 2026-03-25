namespace TaskTracker.Api.Models;

public class ApiResponse<T>
{
    public T? Data { get; init; }
    public string? Error { get; init; }
    public bool Success => Error is null;

    public static ApiResponse<T> Ok(T data) => new() { Data = data };
    public static ApiResponse<T> Fail(string error) => new() { Error = error };
}

public class ApiResponse
{
    public string? Error { get; init; }
    public bool Success => Error is null;

    public static ApiResponse Ok() => new();
    public static ApiResponse Fail(string error) => new() { Error = error };
}
