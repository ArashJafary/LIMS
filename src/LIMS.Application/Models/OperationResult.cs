namespace LIMS.Application.Models;
public class OperationResult
{
    public OperationResult() { }
    public OperationResult(Exception exception)
        => (Exception, Success, OnFailedMessage) = (exception, false, exception.Message);
    public OperationResult(string onFailedMessage)
        => (Success, OnFailedMessage) = (false, onFailedMessage);

    public bool Success { get; protected set; } = true;
    public string OnFailedMessage { get; protected set; }
    public Exception Exception { get; protected set; }

    public static OperationResult OnFailed(string message)
        => new(message);
    public static OperationResult OnException(Exception exception)
        => new(exception);
}
public class OperationResult<TResult> : OperationResult
{
    public OperationResult(TResult result) : base()
        => (Result) = (result);
    public OperationResult(string message) : base(message) { }
    public OperationResult(Exception exception) : base(exception) { }

    public TResult Result { get; protected set; }

    public static OperationResult<TResult> OnSuccess(TResult result)
        => new OperationResult<TResult>(result);

    public static OperationResult<TResult> OnFailed(string message)
        => new OperationResult<TResult>(message);

    public static OperationResult<TResult> OnException(Exception exception)
        => new OperationResult<TResult>(exception);
}