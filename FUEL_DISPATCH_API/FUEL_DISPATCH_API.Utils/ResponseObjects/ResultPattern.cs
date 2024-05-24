namespace FUEL_DISPATCH_API.Utils.ResponseObjects
{
    public record ResultPattern<T>
    {
        public ResultPattern(T value, int statusCode, bool isSuccess, string message)
        {
            Value = value;
            StatusCode = statusCode;
            IsSuccess = isSuccess;
            Message = message;
        }
        public T Value { get; init; }
        public int StatusCode { get; init; }
        public bool IsSuccess { get; init; }
        public string Message { get; init; }
        public static ResultPattern<T> Success(T value, int statusCode, string message) =>
            new ResultPattern<T>(value, statusCode, true, message);

        public static ResultPattern<T> Failure(int statuscode, string message) =>
            new ResultPattern<T>(default!, statuscode, false, message);
    }
}