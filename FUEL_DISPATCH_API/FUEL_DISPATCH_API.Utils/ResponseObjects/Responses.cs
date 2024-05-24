using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;

namespace FMP_MATEINANCEA_API.Utils
{
    public class Responses
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public int Code { get; set; }
        public object Data { get; set; }
        public object PageInformation { get; set; }
        public Responses() { }

        public Responses(object data)
        {
            Data = data;
        }

        public static Responses GetBadRequestResponse(string menssage = "BadRequest", int code = StatusCodes.Status400BadRequest)
        {
            return new Responses()
            {
                Message = menssage,
                Success = false,
                Code = code
            };
        }
    }


    public class Responses<T> where T : class
    {
        public string Message { get; set; } = "Accion exitosa";
        public bool Success { get; set; } = true;
        public T Data { get; set; }
        public int Code { get; set; }

        public Responses() { }

        public Responses(T data)
        {
            Data = data;
        }
    }
}
