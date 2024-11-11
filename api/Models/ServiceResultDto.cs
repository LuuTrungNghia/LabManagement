namespace api.Models
{
    public class ServiceResultDto<T>
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }
        public T Data { get; private set; }

        private ServiceResultDto(bool isSuccess, string message, T data)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }

        public static ServiceResultDto<T> Success(T data, string message = null)
        {
            return new ServiceResultDto<T>(true, message, data);
        }

        public static ServiceResultDto<T> Failure(string message, T data = default)
        {
            return new ServiceResultDto<T>(false, message, data);
        }
    }
}
