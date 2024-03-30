using LaptopStoreApi.Constants;
using LaptopStoreApi.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using LaptopStoreApi.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
        public List<Claim> claimsToAdd = new List<Claim>
        {

            // Prodcut Claim
            new Claim("ReadProduct", "false"),
            new Claim("CreateProduct", "false"),
            new Claim("UpdateProduct", "false"),
            new Claim("DeleteProduct", "false"),

            // Category
            new Claim("ReadCategory", "false"),
            new Claim("CreateCategory", "false"),
            new Claim("UpdateCategory", "false"),
            new Claim("DeleteCategory", "false"),

            // Order
            new Claim("ReadOrder", "false"),
            new Claim("CreateOrder", "false"),
            new Claim("UpdateOrder", "false"),
            new Claim("DeleteOrder", "false"),

            // User 
            new Claim("ReadUser", "false"),
            new Claim("CreateUser", "false"),
            new Claim("UpdateUser", "false"),
            new Claim("DeleteUser", "false"),

            // ... Thêm các claim khác vào danh sách ...
        };
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
        



        // Lấy danh sách claim của một user = userId 
        [HttpGet]
        public async Task<List<UserClaim>> GetClaimsWithUserId(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new List<UserClaim>(); // Thay đổi BadRequest() thành null để trả về kết quả không hợp lệ
            }

            var reqCl = await _userManager.GetClaimsAsync(user);
            var res = reqCl.Select(c => new UserClaim { Type = c.Type, Value = c.Value }).ToList();
            return res;
        }

        [HttpGet]
        public async Task<UserInfo> GetInfoWithUserId(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new UserInfo(); // Thay đổi BadRequest() thành null để trả về kết quả không hợp lệ
            }
            UserInfo res = new UserInfo();
            res.FullName = user.FullName;
            res.UserName = user.UserName;
            res.Email = user.Email;
            res.Address = user.Address;
            res.PhoneNumber = user.PhoneNumber;
            res.UserId = userId;
            res.AvatarUrl = user.AvatarUrl;
            var reqCl = await _userManager.GetClaimsAsync(user);
            res.Claims = reqCl.Select(c => new UserClaim { Type = c.Type, Value = c.Value }).ToList();
            return res;
        }
        // Lấy danh sách người dùng gồm cả Claim
        [HttpGet]
        public async Task<List<UserInfo>> GetAllUsersWithClaims()
        {
            List<UserInfo> usersWithClaims = new List<UserInfo>();

            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var claims = await _userManager.GetClaimsAsync(user);

                UserInfo userInfo = new UserInfo
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Email = user.Email,
                    Claims = claims.Select(c => new UserClaim { Type = c.Type, Value = c.Value }).ToList()
                };

                usersWithClaims.Add(userInfo);
            }

            return usersWithClaims;
        }

/*        [HttpGet]
        public async Task<List<UserRoleClaim>> GetAllUsersRoleClaims()
        {
            List<UserRoleClaim> usersWithClaims = new List<UserRoleClaim>();

            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var roleNames = await _userManager.GetRolesAsync(user);

                var userRoleClaims = new UserRoleClaim
                {
                    UserName = user.UserName,
                    RoleClaims = new List<RoleClaim>()
                };

                foreach (var roleName in roleNames)
                {
                    var role = await _roleManager.FindByNameAsync(roleName);
                    if (role != null)
                    {
                        var roleClaim = new RoleClaim
                        {
                            RoleName = role.Name,
                            Claims = (await _roleManager.GetClaimsAsync(role))
                                .Select(c => new UserClaim { Type = c.Type, Value = c.Value })
                                .ToList()
                        };

                        userRoleClaims.RoleClaims.Add(roleClaim);
                    }
                }

                usersWithClaims.Add(userRoleClaims);
            }

            return usersWithClaims;
        }*/


        // Thêm hoặc cập nhật quyền (claim) vào người dùng

        [HttpPost]
        public async Task<IActionResult> AddOrUpdateClaimModels(UserInfo Claims)
        {
            /*var userName = Claims.UserName;*/
            var userId = Claims.UserId;
            if (userId != null)
            {
                // Tìm kiếm người dùng bằng UserName
                /*var user = await _userManager.FindByNameAsync(userName);*/

                // Tìm kiếm người dùng bằng Id 
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    var existingClaims = await _userManager.GetClaimsAsync(user);
                    foreach (var cl in Claims.Claims!)
                    {
                        var claimToUpdate = existingClaims.FirstOrDefault(c => c.Type == cl.Type);

                        if (claimToUpdate != null)
                        {
                            // Xóa claim cũ
                            var removeResult = await _userManager.RemoveClaimAsync(user, claimToUpdate);
                            if (!removeResult.Succeeded)
                            {
                                // Xảy ra lỗi khi xóa claim cũ
                                return BadRequest();
                            }
                        }

                        // Thêm claim mới
                        Claim newClaim = new Claim(cl.Type!, cl.Value!);
                        var addResult = await _userManager.AddClaimAsync(user, newClaim);

                        if (!addResult.Succeeded)
                        {
                            // Xảy ra lỗi khi thêm/cập nhật claim
                            return BadRequest();
                        }
                    }
                    var reqCl = await _userManager.GetClaimsAsync(user);
                    var res = reqCl.Select(c => new UserClaim { Type = c.Type, Value = c.Value }).ToList();
                    // Trường hợp thành công
                    return Ok(res); // Hoặc giá trị thành công phù hợp
                }
                else
                {
                    // Không tìm thấy người dùng
                    return NotFound();
                }
            }
            else
            {
                // Không tìm thấy tên người dùng
                return NotFound(ModelState);
            }
        }



        // Thêm hoặc cập nhật quyền cho role 
        [HttpPost]
        public async Task<IActionResult> AddOrUpdateClaimToRole(List<RoleClaim> req)
        {
            if (req == null)
            {
                return BadRequest();
            }

            List<string> rolesWithNull = new List<string>(); // Danh sách các role có giá trị null

            foreach (var role in req)
            {
                var checkRole = await _roleManager.FindByNameAsync(role.RoleName!);
                if (checkRole != null)
                {
                    var existingClaims = await _roleManager.GetClaimsAsync(checkRole);

                    foreach (var claim in role.Claims!)
                    {
                        // Kiểm tra xem claim đã tồn tại trong role chưa
                        var existingClaim = existingClaims.FirstOrDefault(c => c.Type == claim.Type);
                        if (existingClaim != null)
                        {
                            // Nếu claim đã tồn tại, xóa claim khỏi role
                            var result = await _roleManager.RemoveClaimAsync(checkRole, existingClaim);
                            if (!result.Succeeded)
                            {
                                // Xử lý lỗi khi xóa claim khỏi role
                                return BadRequest("Lỗi xóa");
                            }
                        }

                        // Tạo mới claim và thêm vào role
                        var newClaim = new Claim(claim.Type!, claim.Value!);
                        var addResult = await _roleManager.AddClaimAsync(checkRole, newClaim);
                        if (!addResult.Succeeded)
                        {
                            // Xử lý lỗi khi thêm claim vào role
                            return BadRequest("Lỗi thêm");
                        }
                    }
                }
                else
                {
                    rolesWithNull.Add(role.RoleName!); // Thêm role có giá trị null vào danh sách
                }
            }

            // Trả về OK với danh sách role có giá trị null
            return Ok(rolesWithNull);
        }


        // Lấy danh sách Role
        [HttpGet]

        public async Task<IActionResult> GetAllRole()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return Ok(roles);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return BadRequest("Tên role không hợp lệ");
            }

            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (roleExists)
            {
                return BadRequest("Role đã tồn tại");
            }

            var newRole = new IdentityRole(roleName);
            var result = await _roleManager.CreateAsync(newRole);
            if (result.Succeeded)
            {
                return Ok("Role đã được tạo thành công");
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(error => error.Description));
                return BadRequest($"Lỗi tạo role: {errors}");
            }
        }
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
                Admin.FullName = "Nguyễn Phúc Thịnh";
                Admin.AvatarUrl = "Avatars/user1.jpg";
                Admin.Address = "Cao Đẳng Sài Gòn";
                string AdminPassword = "Admin123";
                await _userManager.CreateAsync(Admin, AdminPassword);
                await _userManager.AddClaimsAsync(Admin, claimsToAdd);
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
                Moderator.FullName = "Huỳnh Duy Khánh";
                Moderator.AvatarUrl = "Avatars/user2.jpg";
                Moderator.Address = "Cao Đẳng Sài Gòn";
                string ModPassword = "Mod123";
                await _userManager.CreateAsync(Moderator, ModPassword);
                await _userManager.AddClaimsAsync(Moderator, claimsToAdd);
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
                Client.FullName = "GPT";
                Client.AvatarUrl = "Avatars/user3.jpg";
                Client.Address = "Cao Đẳng Sài Gòn";
                string ClientPassword = "Client123";
                await _userManager.CreateAsync(Client, ClientPassword);
                await _userManager.AddClaimsAsync(Client, claimsToAdd);
            }

            var testbase = await _userManager.FindByNameAsync("Base");
            if (testbase != null)
            {
                Acc++;
            }
            else
            {
                var Base = new User();
                Base.UserName = "Base";
                Base.Email = "Base@gmail.com";
                Base.FullName = "Base";
                Base.AvatarUrl = "Avatars/user3.jpg";
                Base.Address = "Cao Đẳng Sài Gòn";
                string BasePassword = "Base123";
                await _userManager.CreateAsync(Base, BasePassword);
                await _userManager.AddClaimsAsync(Base, claimsToAdd);
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
