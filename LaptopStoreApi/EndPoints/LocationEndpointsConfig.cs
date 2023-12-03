using LaptopStoreApi.Models;
namespace LaptopStoreApi.EndPoints
{
    public class LocationEndpointsConfig
    {
        public static void AddEndpoints (WebApplication app)
        {
            app.MapGet("api/find", () =>
            {
                return "Hello Word";
            });
            app.MapGet("api/testapi", () =>
            {
                return "test api";
            });
            app.MapPost("api/info/{id}", (int id) =>
            {
                return $"this is {id} product";
            });
            app.MapPut("api/put/{id}", (int id) =>
            {
                return $"this is put {id} product";
            });
        }
    }
}
