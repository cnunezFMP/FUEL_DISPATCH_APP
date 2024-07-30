using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
namespace FUEL_DISPATCH_API.Utils.ResponseObjects
{
    public record ServiceResults
    {
        public ServiceResults(int objId, string message, bool isSuccess, int statusCode, object data)
        {
            Message = message;
            ObjId = objId;
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            Data = data;
        }
        public ServiceResults(int statusCode, string message = "An error ocurred. ", object data = null)
        {
            Message = message;
            StatusCode = statusCode;
            Data = data;
        }
        public string Message
        {
            get;
            init;
        }
        public int ObjId
        {
            get;
            init;
        }
        public bool IsSuccess 
        {
            get;
            init;
        }
        public int StatusCode 
        { 
            get; 
            set; 
        }
        public object Data
        {
            get;
            set;
        }
        /**************************
         ************************
         **
         ** DISPATCHES RESPONSES
         **
         ************************
         **************************/
        public static ServiceResults DataObtainedSuccessfully(int objId, string message, bool isSuccess, int statusCode, object data) =>
            new ServiceResults(objId, message, isSuccess, statusCode, data);
        public static ServiceResults NotFound(int objId) =>
            new ServiceResults(objId, $"Dispatch with Id: {objId} not exist. ");
        public static ServiceResults NewOdometerCantBeLowerThanPreviousDispatch() =>
            new ServiceResults(default, "New Dispatch Odometer can't be lower than the previous Dispatch. ");

        public static ServiceResults GallonsForDispatchCantBeZero() =>
            new ServiceResults(default, "Gallons for Dispatch can't be 0");


        public static ServiceResults DriverIsInactive(int objId) =>
            new ServiceResults(objId, $"Driver with Id: {objId} isn't active. ");

        public static ServiceResults VehicleForDispatchNotFound(string vehicleToken) =>
            new ServiceResults(default, $"Vehicle with Id: {vehicleToken} not found. ");
        public static ServiceResults VehicleIsInactive(string vehicleToken) =>
            new ServiceResults(default, $"Vehicle with Token: {vehicleToken} isn't active. ");

        public static ServiceResults DispatchSuccessfully(int objId) =>
            new ServiceResults(objId, $"Dispatch Id: {objId} created successfully. ");
        public static ServiceResults DefaultBadRequestResponse() =>
            new ServiceResults(default, "Bad Request. ");

    }
    
}
