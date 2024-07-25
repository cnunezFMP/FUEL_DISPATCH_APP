using FUEL_DISPATCH_API.DataAccess.DTOs;
using FUEL_DISPATCH_API.DataAccess.Models;
using Swashbuckle.AspNetCore.Filters;

namespace FUEL_DISPATCH_API.Swagger.SwaggerExamples
{
    public class UserSwaggerExample : IExamplesProvider<UserRegistrationDto>
    {
        public UserRegistrationDto GetExamples()
        {
            return new UserRegistrationDto
            {
                Email = "a@gmail.com",
                FullName = "Jhon Doe",
                Username = "jdoe",
                FullDirection = "Plaza Shipco",
                PhoneNumber = "849-845-6265",
                BirthDate = DateTime.Parse("2000-01-01 00:00:00.000"),
                DriverId = 1
            };
        }
    }
}
