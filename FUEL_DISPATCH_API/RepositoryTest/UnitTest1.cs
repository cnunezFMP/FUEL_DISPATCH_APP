using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using System.Security.Cryptography.X509Certificates;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace RepositoryTest
{
    [TestClass]
    public class UnitTest1
    {
        private readonly IGenericInterface<Dispatch> _genericInterface;
        public UnitTest1(IGenericInterface<Dispatch> genericInterface) 
        { 
            _genericInterface = genericInterface;
        }
        
        public ResultPattern<Paging<Dispatch>> GetAll(GridifyQuery query)
        {
            return  _genericInterface.GetAll(query);
        }
        [TestMethod]
        public void TestMethod()
        {
            // Arrange <- Donde inicializaremos variables.

            var dispatchF01 = new Dispatch
            {
                Id = 1,
                VehicleToken = "F-01",
                DriverId = 1,
                CreatedAt = DateTime.Now
            };
            var dispatchF02 = new Dispatch
            {
                Id = 2,
                VehicleToken = "F-02",
                DriverId = 2,
                CreatedAt = DateTime.Now
            };
            var gq = new GridifyQuery
            {
                Filter = "vehicleToken=F-02",
                Page = 1,
                PageSize = 1
            };

            // Act <- Donde ejecutaremos los metodos que queremos testear
            var expected = GetAll(gq);
            // Assert <- Para validar que lo que queremos que se cumpla, se cumple.
            Assert.AreEqual<ResultPattern<Dispatch>, Dispatch>(expected, dispatchF02);
        }
    }
}