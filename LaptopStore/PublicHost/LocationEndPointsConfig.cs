using Microsoft.AspNetCore.Mvc;

namespace LaptopStore.PublicHost
{
    public class LocationEndPointsConfig
    {
        public static void AddEndpoints(WebApplication app)
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
