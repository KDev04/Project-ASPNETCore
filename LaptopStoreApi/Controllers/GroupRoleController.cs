using LaptopStoreApi.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LaptopStoreApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GroupRoleController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<SeedController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        public GroupRoleController(
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
        
        
        [HttpGet]
        public async Task<IActionResult> GetAllGroupRole()
        {
            var groupRoles = await _context.GroupRoles
                                           .Include(gr => gr.Roles) // Load danh sách IdentityRoles
                                           .ToListAsync();

            return Ok(groupRoles);
        }


        [HttpGet]
        public async Task<IActionResult> GetGroupRoleById(int id)
        {
            var groupRole = await _context.GroupRoles
                                          .Include(gr => gr.Roles) // Load danh sách các vai trò
                                          .FirstOrDefaultAsync(gr => gr.Id == id);

            if (groupRole == null)
            {
                return NotFound("Không tìm thấy nhóm này");
            }

            return Ok(groupRole);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroupRole(GroupRoleModel req)
        {
            // Kiểm tra xem các trường bắt buộc đã được điền đầy đủ hay không
            if (string.IsNullOrEmpty(req.Name))
            {
                return BadRequest("Tên nhóm không được để trống");
            }

            // Kiểm tra xem tên nhóm đã tồn tại trong cơ sở dữ liệu hay chưa
            var existingGroup = await _context.GroupRoles.FirstOrDefaultAsync(r => r.Name == req.Name);
            if (existingGroup != null)
            {
                return BadRequest("Tên nhóm đã tồn tại");
            }

            // Kiểm tra xem danh sách các vai trò được truyền vào có hợp lệ hay không
            if (req.Roles == null || req.Roles.Count == 0)
            {
                return BadRequest("Danh sách vai trò không được để trống");
            }

            // Thêm nhóm vai trò mới vào cơ sở dữ liệu
            GroupRole newGroupRole = new GroupRole
            {
                Name = req.Name,
                Description = req.Description,
                Roles = new List<IdentityRole>()
            };

            // Lặp qua danh sách các tên vai trò và tạo các đối tượng IdentityRole tương ứng
            foreach (var roleName in req.Roles)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    newGroupRole.Roles.Add(role);
                }
                else
                {
                    // Nếu vai trò không tồn tại, bạn có thể xử lý tùy ý, ví dụ: bỏ qua hoặc trả về lỗi
                    // Trong trường hợp này, tôi sẽ bỏ qua vai trò không tồn tại
                    continue;
                }
            }

            // Thêm nhóm vai trò mới vào cơ sở dữ liệu và lưu thay đổi
            _context.GroupRoles.Add(newGroupRole);
            await _context.SaveChangesAsync();

            return Ok("Đã tạo nhóm");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateGroupRole(int groupId, GroupRole updatedGroupRole)
        {
            // Tìm nhóm quyền dựa trên groupId
            var existingGroup = await _context.GroupRoles.FindAsync(groupId);

            // Kiểm tra xem nhóm quyền có tồn tại không
            if (existingGroup == null)
            {
                return NotFound("Không tìm thấy nhóm có ID: " + groupId);
            }

            // Kiểm tra xem tên nhóm mới đã tồn tại chưa (không tính nhóm hiện tại)
            var groupNameExists = await _context.GroupRoles.AnyAsync(r => r.Name == updatedGroupRole.Name && r.Id != groupId);
            if (groupNameExists)
            {
                return BadRequest("Tên nhóm đã tồn tại");
            }

            // Cập nhật thông tin của nhóm
            existingGroup.Name = updatedGroupRole.Name;
            existingGroup.Description = updatedGroupRole.Description;
            existingGroup.Roles = updatedGroupRole.Roles;

            // Cập nhật vai trò cho các người dùng trong nhóm
            foreach (var userGroup in existingGroup.UserGroups)
            {
                var user = await _userManager.FindByIdAsync(userGroup.UserId);
                if (user != null)
                {
                    foreach (var newRole in updatedGroupRole.Roles)
                    {
                        // Nếu vai trò mới không có trong vai trò của người dùng, thêm vào
                        if (!await _userManager.IsInRoleAsync(user, newRole.Name))
                        {
                            await _userManager.AddToRoleAsync(user, newRole.Name);
                        }
                    }

                    // Gỡ các vai trò bị xóa khỏi người dùng
                    foreach (var oldRole in userGroup.GroupRole.Roles)
                    {
                        if (!updatedGroupRole.Roles.Contains(oldRole))
                        {
                            await _userManager.RemoveFromRoleAsync(user, oldRole.Name);
                        }
                    }
                }
            }

            _context.GroupRoles.Update(existingGroup);
            await _context.SaveChangesAsync();

            return Ok("Đã cập nhật nhóm");
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

        [HttpPut("{roleName}")]
        public async Task<IActionResult> UpdateRole(string roleName, string newRoleName)
        {
            if (string.IsNullOrWhiteSpace(roleName) || string.IsNullOrWhiteSpace(newRoleName))
            {
                return BadRequest("Tên role không hợp lệ");
            }

            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return NotFound("Không tìm thấy role");
            }

            var existingRole = await _roleManager.FindByNameAsync(newRoleName);
            if (existingRole != null && existingRole.Name != roleName)
            {
                return BadRequest("Role đã tồn tại");
            }

            role.Name = newRoleName;

            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return Ok("Role đã được cập nhật thành công");
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(error => error.Description));
                return BadRequest($"Lỗi cập nhật role: {errors}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddRolesToGroupRole(int groupId, List<IdentityRole> roleNames)
        {
            // Tìm nhóm vai trò cần cập nhật từ ID
            var group = await _context.GroupRoles.FindAsync(groupId);

            if (group == null)
            {
                return NotFound("Không tìm thấy nhóm có ID: " + groupId);
            }

            foreach (var roleName in roleNames)
            {
                // Kiểm tra xem vai trò đã tồn tại trong danh sách roles của nhóm chưa
                if (!group.Roles!.Contains(roleName))
                {
                    // Thêm vai trò mới vào danh sách roles của nhóm
                    group.Roles.Add(roleName);
                }
            }

            _context.GroupRoles.Update(group);
            await _context.SaveChangesAsync();

            return Ok("Đã thêm các vai trò vào nhóm");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRolesFromGroupRole(int groupId, List<IdentityRole> roleNames)
        {
            // Tìm nhóm vai trò cần cập nhật từ ID
            var group = await _context.GroupRoles.FindAsync(groupId);

            if (group == null)
            {
                return NotFound("Không tìm thấy nhóm có ID: " + groupId);
            }

            foreach (var roleName in roleNames)
            {
                // Kiểm tra xem vai trò có tồn tại trong danh sách roles của nhóm không
                if (group.Roles!.Contains(roleName))
                {
                    // Xóa vai trò khỏi danh sách roles của nhóm
                    group.Roles.Remove(roleName);
                }
            }

            _context.GroupRoles.Update(group);
            await _context.SaveChangesAsync();

            return Ok("Đã xóa các vai trò khỏi nhóm");
        }

        [HttpDelete] 
        public async Task<IActionResult> DeleteGroupRole(int id)
        {
            var gr = await _context.GroupRoles.FindAsync(id);
            if (gr == null)
            {
                return NotFound("Không có nhóm này");
            }
            _context.GroupRoles.Remove(gr);
            await _context.SaveChangesAsync();
            return Ok("Đã xóa nhóm");

        }
    }
}
