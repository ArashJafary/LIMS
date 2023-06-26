namespace BigBlueApi.Application.Models;

public class OperationResult<TResult>
{
    protected OperationResult()
    {
        this.Success = true;
    }

    protected OperationResult(string message)
    {
        this.Success = false;
        this.FailureMessage = message;
    }

    protected OperationResult(Exception exception)
    {
        this.Success = false;
        this.Exception = exception;
    }

    public bool Success { get; protected set; }
    public TResult Data { get; set; }
    public string FailureMessage { get; protected set; }
    public Exception Exception { get; protected set; }

    public bool IsException() => this.Exception != null;

    public static OperationResult<TResult> SuccessResult(TResult result) =>
        new OperationResult<TResult> { Success = true, Data = result };

    public static OperationResult<TResult> FailureResult(string message) =>
        new OperationResult<TResult>(message);

    public static OperationResult<TResult> ExceptionResult(Exception exception) =>
        new OperationResult<TResult>(exception);
}
