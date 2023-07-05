namespace BigBlueApi.Models;
public class OperationResult<TResult>
{
    public OperationResult(Exception exception) => (Exception, Success,OnFailedMessage) = (exception, false,exception.Message);

    public OperationResult(TResult result) => (Result,Success) = (result,false);
    public OperationResult(string onFailedMessage) => (Success, OnFailedMessage) = (false, onFailedMessage);

    public bool Success { get; protected set; }
    public string OnFailedMessage { get; protected set; }
    public Exception Exception { get; protected set; }
    public TResult Result { get; protected set; }

    public OperationResult<TResult> OnSuccess(TResult result) => new OperationResult<TResult>(result);
    public OperationResult<TResult> OnFailed(string message) => new OperationResult<TResult>(message);
    public OperationResult<TResult> OnException(Exception exception) => new OperationResult<TResult>(exception);
}