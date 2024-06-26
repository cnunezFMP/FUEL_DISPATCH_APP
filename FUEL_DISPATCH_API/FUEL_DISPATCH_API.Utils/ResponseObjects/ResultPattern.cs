namespace FUEL_DISPATCH_API.Utils.ResponseObjects
{
    public record ResultPattern<T>
    {
        public ResultPattern(object data, int statusCode, bool isSuccess, string message)
        {
            Data = data;
            StatusCode = statusCode;
            IsSuccess = isSuccess;
            Message = message;
        }
        public object Data { get; init; }
        public int StatusCode { get; init; }
        public bool IsSuccess { get; init; }
        public string Message { get; init; }
        public static ResultPattern<T> Success(object data, int statusCode, string message) =>
            new ResultPattern<T>(data, statusCode, true, message);

        public static ResultPattern<T> Failure(int statuscode, string message, object data) =>
            new ResultPattern<T>(data, statuscode, false, message);
    }
}