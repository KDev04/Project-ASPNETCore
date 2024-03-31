using LaptopStoreApi.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Data;

namespace LaptopStoreApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserGroupController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<SeedController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        public UserGroupController(ApiDbContext context, IWebHostEnvironment env, ILogger<SeedController> logger, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _context = context;
            _env = env;
            _logger = logger;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            var res = await _context.UserGroups
                                    .Include(gr => gr.User)
                                    .Include(gr => gr.GroupRole)
                                    .GroupBy(ll => ll.UserId)
                                    .Select(g => new UserRoles
                                    {
                                        UserId = g.Key,
                                        UserName = g.FirstOrDefault().User.UserName,
                                        FullName = g.FirstOrDefault().User.FullName,
                                        Address = g.FirstOrDefault().User.Address,
                                        AvatarUrl = g.FirstOrDefault().User.AvatarUrl,
                                        Email = g.FirstOrDefault().User.Email,
                                        PhoneNumber = g.FirstOrDefault().User.PhoneNumber,
                                        Roles = g.Select(ug => ug.GroupRole).ToList()


                                    })
                                    .ToListAsync();
            UserRolePage userRolePage = new UserRolePage()
            {
                Users = await _context.Users.ToListAsync(),
                Roles= res
            };
            return Ok(userRolePage);
        }
        [HttpGet]
        public async Task<IActionResult> GetUserWithGroupRole()
        {

            var users = await _context.Users.ToListAsync();
            var gr = await _context.GroupRoles.Include(g=>g.Roles).ToListAsync();
            UserGroupRolePage userRolePage = new UserGroupRolePage()
            {

                Users = users,
                GroupRoles = gr
            };
            return Ok(userRolePage);
        }

        [HttpGet]
        public async Task<IActionResult> GetGroupByUserId(string userId)
        {
            var res = await _context.UserGroups
                                    .Where(ug => ug.UserId == userId)
                                    .Include(g => g.GroupRole)
                                    .ToListAsync();

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            return Ok(JsonSerializer.Serialize(res, options));
        }
        [HttpGet]
        public async Task<IActionResult> GetUserByGroupId(int groupId)
        {
            var res = await _context.UserGroups
                                    .Where(ug => ug.GroupRoleId == groupId)
                                    .Include(ug => ug.User)
                                    .ToListAsync();

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            return Ok(JsonSerializer.Serialize(res, options));
        }
        [HttpGet]
        public async Task<IActionResult> GetRoleInUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound("Người dùng không tồn tại");
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var roles = new List<IdentityRole>();

            foreach (var role in userRoles)
            {
                var identityRole = await _roleManager.FindByNameAsync(role);
                if (identityRole != null)
                {
                    roles.Add(identityRole);
                }
            }

            // Trả về danh sách các đối tượng IdentityRole
            return Ok(roles);
        }

        [HttpPost]
        public async Task<IActionResult> AddRoleToUser(string userId, string roleId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var role = await _roleManager.FindByIdAsync(roleId);

            if (user == null || role == null)
            {
                return NotFound("Người dùng hoặc vai trò không tồn tại");
            }

            if (await _userManager.IsInRoleAsync(user, role.Name))
            {
                return BadRequest("Người dùng đã có sẵn quyền " + role.Name);
            }

            var result = await _userManager.AddToRoleAsync(user, role.Name);

            if (result.Succeeded)
            {
                return Ok("Đã thêm quyền vào người dùng");
            }
            else
            {
                return BadRequest("Lỗi khi thêm quyền vào người dùng");
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRoleToUser(string userId, string roleId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var role = await _roleManager.FindByIdAsync(roleId);

            if (user == null || role == null)
            {
                return NotFound("Người dùng hoặc vai trò không tồn tại");
            }

            if (!await _userManager.IsInRoleAsync(user, role.Name))
            {
                return BadRequest("Người dùng " + user.FullName+ " chưa có quyền " + role.Name);
            }

            var result = await _userManager.RemoveFromRoleAsync(user, role.Name);

            if (result.Succeeded)
            {
                return Ok("Đã gỡ quyền "+ role.Name+ " ra khỏi người dùng " + user.FullName);
            }
            else
            {
                return BadRequest("Lỗi khi gỡ quyền khỏi người dùng");
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateUserGroup(string userId, int groupId)
        {
            // Kiểm tra userId và groupId có null hoặc rỗng không
            if (string.IsNullOrEmpty(userId) || groupId <= 0)
            {
                return BadRequest("UserId hoặc GroupId không hợp lệ");
            }

            var user = await _userManager.FindByIdAsync(userId);

            Console.WriteLine(user.FullName + user.PhoneNumber);
            var group = await _context.GroupRoles.Include(g=>g.Roles).Where(g => g.Id == groupId).FirstOrDefaultAsync();

            Console.WriteLine("Tim thay nhom ");

            // Kiểm tra xem user và group có tồn tại không
            if (user == null || group == null)
            {
                return NotFound("Người dùng hoặc nhóm không tồn tại");
            }

            


            // Kiểm tra xem user đã thuộc nhóm này chưa
            var userGroupExists = await _context.UserGroups.AnyAsync(ug => ug.UserId == userId && ug.GroupRoleId == groupId);
            if (userGroupExists)
            {
                return BadRequest("Người dùng đã thuộc nhóm này");
            }
            Console.WriteLine("check    null?????????");

            if (user != null && group.Roles!=null)
            {
                Console.WriteLine("Chui  vao if ?????????");

                foreach (var role in group.Roles)
                {
                    if (!await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        await _userManager.AddToRoleAsync(user, role.Name);
                        Console.WriteLine($"Da them {role.Name}");
                    }
                    Console.WriteLine("Dang trong vong lap");
                }

            }
            Console.WriteLine("null?????????");
            // Tạo mới UserGroupRole và lưu vào cơ sở dữ liệu
            var newUserGroup = new UserGroupRole
            {
                UserId = userId,
                User = user,
                GroupRole = group,
                GroupRoleId = groupId,
            };

            _context.UserGroups.Add(newUserGroup);
            await _context.SaveChangesAsync();

            return Ok("Đã thêm người dùng vào nhóm thành công");
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteUserGroup(string userId, int groupId)
        {
            // Tìm người dùng và nhóm quyền dựa trên userId và groupId
            var user = await _userManager.FindByIdAsync(userId);
            var group = await _context.GroupRoles.Include(gr => gr.Roles).FirstOrDefaultAsync(g => g.Id == groupId);

            // Kiểm tra xem nhóm quyền và người dùng có tồn tại không
            if (group == null || user == null)
            {
                return NotFound();
            }

            // Duyệt qua các vai trò trong nhóm quyền
            foreach (var role in group.Roles)
            {
                // Kiểm tra xem người dùng có trong vai trò không
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    // Nếu có, loại bỏ người dùng khỏi vai trò
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
            }

            // Xóa UserGroup tương ứng từ cơ sở dữ liệu
            var userGroup = await _context.UserGroups.FindAsync(userId, groupId);
            if (userGroup == null)
            {
                return NotFound();
            }

            _context.UserGroups.Remove(userGroup);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
