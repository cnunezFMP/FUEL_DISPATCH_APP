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
        public ServiceResults(int objId, string message)
        {
            Message = message;
            ObjId = objId;
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
        /**************************
         ************************
         **
         ** DISPATCHES RESPONSES
         **
         ************************
         **************************/
        public static ServiceResults NotFound(int objId) =>
            new ServiceResults(objId, $"Dispatch with Id: {objId} not exist. ");
        public static ServiceResults NewOdometerCantBeLowerThanPreviousDispatch() =>
            new ServiceResults(default, "New Dispatch Odometer can't be lower than the previous Dispatch. ");

        public static ServiceResults NewDispatchOdometerCantBeEqualsToThePreviousDispatch() =>
            new ServiceResults(default, "New Dispatch Odometer can't be equals to the previous Dispatch. ");

        public static ServiceResults GallonsForDispatchCantBeZero() =>
            new ServiceResults(default, "Gallons for Dispatch can't be 0");

        public static ServiceResults DriverForDispatchNotFound(int objId) =>
            new ServiceResults(objId, $"Driver with Id: {objId} not found. ");

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
