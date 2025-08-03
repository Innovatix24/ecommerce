
namespace Application.Common;

public class Result
{
    public bool IsSuccess { get; set; }
    public string Error { get; set; }

    protected Result(bool isSuccess, string error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, null);
    public static Result Failure(string error) => new(false, error);
}

public class Result<TData> : Result
{
    public TData? Data { get; set; }
    protected Result(bool isSuccess, TData value, string error) : base(isSuccess, error)
    {
        Data = value;
    }

    public static Result<TData> Success(TData value) => new(true, value, null);
    public static new Result<TData> Failure(string error) => new(false, default, error);
}