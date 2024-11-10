public class ServiceResultDto<T>
{
    public bool IsSuccess { get; private set; }
    public string Message { get; private set; }
    public T Data { get; private set; }

    public static ServiceResultDto<T> Success(T data)
    {
        return new ServiceResultDto<T> { IsSuccess = true, Data = data };
    }

    public static ServiceResultDto<T> Failure(string message, T data = default)
    {
        return new ServiceResultDto<T> { IsSuccess = false, Message = message, Data = data };
    }
}
