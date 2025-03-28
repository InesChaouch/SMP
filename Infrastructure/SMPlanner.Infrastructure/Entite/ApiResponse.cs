
namespace SMPlanner.Infrastructure.Entite;
public  class ApiResponse<T>
{
    public ApiResponse()
    {
        Succeeded = true;
    }

    public ApiResponse(T result)
    {
        Succeeded = true;
        Message = string.Empty;
        Errors = new();
        Result = result;
    }

    public ApiResponse(T result, string message)
    {
        Result = result;
        Message = message;
        Succeeded = true;
    }

    public ApiResponse(T result, bool IsSucceeded)
    {
        Result = result;
        Succeeded = IsSucceeded;
    }
    public ApiResponse(T result, string message, bool IsSucceeded)
    {
        Result = result;
        Message = message;
        Succeeded = IsSucceeded;
    }

    public ApiResponse(T result, string message, bool IsSucceeded, List<string> errors)
    {
        Result = result;
        Message = message;
        Succeeded = IsSucceeded;
        Errors = errors;
    }

    public ApiResponse(string message)
    {
        Message = message;
        Succeeded = false;
    }

    public T Result { get; set; } = default!;
    public bool Succeeded { get; set; } = false;
    public List<string> Errors { get; set; } = new();
    public string Message { get; set; } = string.Empty;

}
