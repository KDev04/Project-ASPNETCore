using LaptopStoreApi.Constants;
using LaptopStoreApi.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LaptopStoreApi.Controllers
{
    /*    [Authorize(Roles = RoleNames.Administrator)]*/
    
    [Route("[controller]/[action]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly IWebHostEnvironment _env;

        private readonly ILogger<SeedController> _logger;

        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly UserManager<User> _userManager;

        public SeedController(
            ApiDbContext context,
            IWebHostEnvironment env,
            ILogger<SeedController> logger,
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager)
        {
            _context = context;
            _env = env;
            _logger = logger;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        [Authorize]
        [HttpPost]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> AuthData()
        {
            int rolesCreated = 0;
            int usersAddedToRoles = 0;

            if (!await _roleManager.RoleExistsAsync(RoleNames.Moderator))
            {
                await _roleManager.CreateAsync(
                    new IdentityRole(RoleNames.Moderator));
                rolesCreated++;
            }
            if (!await _roleManager.RoleExistsAsync(RoleNames.Administrator))
            {
                await _roleManager.CreateAsync(
                    new IdentityRole(RoleNames.Administrator));
                rolesCreated++;
            }

            var testModerator = await _userManager
                .FindByNameAsync("Moderator");
            if (testModerator != null
                && !await _userManager.IsInRoleAsync(
                    testModerator, RoleNames.Moderator))
            {
                await _userManager.AddToRoleAsync(testModerator, RoleNames.Moderator);
                usersAddedToRoles++;
            }

            var testAdministrator = await _userManager
                .FindByNameAsync("Admin");
            if (testAdministrator != null
                && !await _userManager.IsInRoleAsync(
                    testAdministrator, RoleNames.Administrator))
            {
                await _userManager.AddToRoleAsync(
                    testAdministrator, RoleNames.Moderator);
                await _userManager.AddToRoleAsync(
                    testAdministrator, RoleNames.Administrator);
                usersAddedToRoles++;
            }

            return new JsonResult(new
            {
                RolesCreated = rolesCreated,
                UsersAddedToRoles = usersAddedToRoles
            });
        }

        [HttpPost]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> CreateAccount()
        {
            int Acc = 0;
            string testAd = "Admin chưa được khởi tạo.";
            string testMod = "Moderator chưa được khởi tạo";
            string testCli = "Client chưa được khởi tạo";

            var testAdministrator = await _userManager.FindByNameAsync("Admin");
            if (testAdministrator != null) 
            {
                Acc++; 
                testAd = "Admin đã được khởi tạo."; 
            } 
            else
            {
                var Admin = new User();
                Admin.UserName = "Admin";
                Admin.Email = "Admin@gmail.com";
                Admin.FirstName = "Nguyễn Phúc";
                Admin.LastName = "Thịnh";
                Admin.AvatarUrl = "Avatars/user1.jpg";
                Admin.BirthDay = DateTime.Now;
                Admin.Address = "Cao Đẳng Sài Gòn";
                Admin.Age = 22;
                string AdminPassword = "Admin123";
                var resultAdmin = await _userManager.CreateAsync(Admin, AdminPassword);
            }


            
            var testModerator = await _userManager.FindByNameAsync("Moderator");
            if (testModerator != null) 
            { 
                Acc++; 
                testMod = "Moderator đã được khởi tạo";
            }
            else
            {
                var Moderator = new User();
                Moderator.UserName = "Moderator";
                Moderator.Email = "Moderator@gmail.com";
                Moderator.FirstName = "Huỳnh Duy";
                Moderator.LastName = "Khánh";
                Moderator.AvatarUrl = "Avatars/user2.jpg";
                Moderator.BirthDay = DateTime.Now;
                Moderator.Address = "Cao Đẳng Sài Gòn";
                Moderator.Age = 22;
                string ModPassword = "Mod123";
                var resultMod = await _userManager.CreateAsync(Moderator, ModPassword);
            }


            
            var testClient = await _userManager.FindByNameAsync("Client");
            if (testClient != null) 
            { 
                Acc++; 
                testCli = "Client đã được khởi tạo";
            }
            else
            {
                var Client = new User();
                Client.UserName = "Client";
                Client.Email = "Client@gmail.com";
                Client.FirstName = "Mr .";
                Client.LastName = "Gpt";
                Client.AvatarUrl = "Avatars/user3.jpg";
                Client.BirthDay = DateTime.Now;
                Client.Address = "Cao Đẳng Sài Gòn";
                Client.Age = 22;
                string ClientPassword = "Client123";
                var resultClient = await _userManager.CreateAsync(Client, ClientPassword);
            }


            return new JsonResult(new
            {
                AccountCreated = "Số tài khoản được tạo là" + Acc,
                TestAd = testAd,
                TestMod = testMod,
                TestCli = testCli
            }) ;
        }
    }
}
