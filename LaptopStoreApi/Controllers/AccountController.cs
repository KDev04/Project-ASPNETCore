using LaptopStoreApi.Constants;
using LaptopStoreApi.Database;
using LaptopStoreApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LaptopStoreApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

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
        public AccountController(
            ApiDbContext context,
            ILogger<AccountController> logger,
            IConfiguration configuration,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager
        )
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        [HttpPost]
        public async Task<ActionResult> CreateUser([FromForm] UserModel model)
        {
            try
            {
                var user = new User();

                if (model.Image != null || model?.Image?.Length > 0)
                {
                    string imgFileName = Guid.NewGuid().ToString() + Path.GetExtension(model?.Image?.FileName);
                    string imgFolderPath = Path.Combine("wwwroot/Avatars"); // Thư mục "wwwroot/Image"
                    string imgFilePath = Path.Combine(imgFolderPath, imgFileName);
                    if (!Directory.Exists(imgFolderPath))
                    {
                        Directory.CreateDirectory(imgFolderPath);
                    }
                    var stream = new FileStream(imgFilePath, FileMode.Create);
                    await model!.Image.CopyToAsync(stream);
                    user.AvatarUrl = "Avatars/" + imgFileName;
                }
                else
                {
                    user.AvatarUrl = "Avatars/user1.jpg";
                }
                user.UserName = model?.UserName;
                user.Email = model?.Email;
                user.FullName = model?.FullName;
                user.PhoneNumber = model?.PhoneNumber;
                user.Address = model?.Address;
                if (model?.Password != null)
                {
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        // Thêm các claim cho người dùng
                        var claimResult = await _userManager.AddClaimsAsync(user, claimsToAdd);

                        if (claimResult.Succeeded)
                        {
                            return Ok(result);
                        }
                        else
                        {
                            // Xảy ra lỗi khi thêm claim
                            return BadRequest(claimResult.Errors);
                        }
                    }
                    else
                    {
                        // Xảy ra lỗi khi tạo người dùng
                        return BadRequest(result.Errors);
                    }
                }
                else
                {
                    throw new Exception("Vui lòng nhập password");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [ResponseCache(CacheProfileName = "NoCache")]
        public async Task<ActionResult> Register([FromForm] RegisterModel input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newUser = new User();
                    newUser.UserName = input.UserName;
                    newUser.Email = input.Email;
                    var result = await _userManager.CreateAsync(newUser, input.Password!);

                    if (result.Succeeded)
                    {
                        // Thêm các claim cho người dùng
                        var claimResult = await _userManager.AddClaimsAsync(newUser, claimsToAdd);

                        if (claimResult.Succeeded)
                        {
                            _logger.LogInformation(
                                "User {userName} ({email}) has been created.",
                                newUser.UserName,
                                newUser.Email
                            );
                            return StatusCode(201, $"User '{newUser.UserName}' has been created.");

                        }
                        else
                        {
                            // Xảy ra lỗi khi thêm claim
                            return BadRequest(claimResult.Errors);
                        }

                    }
                    else
                        throw new Exception(
                            string.Format(
                                "Error: {0}",
                                string.Join(" ", result.Errors.Select(e => e.Description))
                            )
                        );
                }
                else
                {
                    var details = new ValidationProblemDetails(ModelState);
                    details.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                    details.Status = StatusCodes.Status400BadRequest;
                    return new BadRequestObjectResult(details);
                }
            }
            catch (Exception e)
            {
                var exceptionDetails = new ProblemDetails();
                exceptionDetails.Detail = e.Message;
                exceptionDetails.Status = StatusCodes.Status500InternalServerError;
                exceptionDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
                return StatusCode(StatusCodes.Status500InternalServerError, exceptionDetails);
            }
        }

        [HttpPost]
        [ResponseCache(CacheProfileName = "NoCache")]
        public async Task<ActionResult> Login([FromForm] LoginModel input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByNameAsync(input.UserName!);
                    if (
                        user == null
                        || !await _userManager.CheckPasswordAsync(user, input.Password!)
                    )
                        throw new Exception("Invalid login attempt.");
                    else
                    {
                        var signingCredentials = new SigningCredentials(
                            new SymmetricSecurityKey(
                                System.Text.Encoding.UTF8.GetBytes(_configuration["JWT:SigningKey"]!)
                            ),
                            SecurityAlgorithms.HmacSha256
                        );
                        var claims = new List<Claim>();
                        claims.AddRange(
                            (await _userManager.GetRolesAsync(user)).Select(
                                r => new Claim(ClaimTypes.Role, r)
                            )
                        );
                        /*                        claims.AddRange(
                                                        (await _userManager.GetClaimsAsync(user)).Select(c=> new Claim(c.Type, c.Value))
                                                    );*/
                        var roles = await _userManager.GetRolesAsync(user);

                        foreach (var role in roles)
                        {
                            // Xử lý vai trò tại đây
                            // Tìm kiếm các claim của vai trò
                            claims.AddRange(
                                    (await _roleManager.GetClaimsAsync(new IdentityRole(role))).Select(c => new Claim(c.Type, c.Value))
                                );
                            Console.WriteLine("Co quyen");
                        }
                        claims.Add(new Claim(ClaimTypes.Name, user.UserName!));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                        Console.WriteLine(claims);
                        var jwtObject = new JwtSecurityToken(
                            issuer: _configuration["JWT:Issuer"],
                            audience: _configuration["JWT:Audience"],
                            claims: claims,
                            expires: DateTime.Now.AddMinutes(
                                180
                            ), /*DateTime.Now.AddSeconds(300),*/ /*chuyển từ 300 giây sang 30 phút*/
                            signingCredentials: signingCredentials
                        );

                        var jwtString = new JwtSecurityTokenHandler().WriteToken(jwtObject);

                        return StatusCode(StatusCodes.Status200OK, jwtString);
                    }
                }
                else
                {
                    var details = new ValidationProblemDetails(ModelState);
                    details.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                    details.Status = StatusCodes.Status400BadRequest;
                    return new BadRequestObjectResult(details);
                }
            }
            catch (Exception e)
            {
                var exceptionDetails = new ProblemDetails();
                exceptionDetails.Detail = e.Message;
                exceptionDetails.Status = StatusCodes.Status401Unauthorized;
                exceptionDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
                return StatusCode(StatusCodes.Status401Unauthorized, exceptionDetails);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUserInfo()
        {
            var userId = User.Claims.FirstOrDefault(
                c => c.Type == ClaimTypes.NameIdentifier
            )?.Value;
            var user = await _userManager.FindByIdAsync(userId!);
            if (user == null)
            {
                return NotFound();
            }
            var UserInfo = new User()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                AvatarUrl = user.AvatarUrl,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
            };
            return Ok(UserInfo);
        }

        [Authorize]
        [HttpGet]
        public async Task<string> GetUserId()
        {
            var userId = User.Claims.FirstOrDefault(
                c => c.Type == ClaimTypes.NameIdentifier
            )?.Value;
            var user = await _userManager.FindByIdAsync(userId!);
            if (user == null)
            {
                var testbase = await _userManager.FindByNameAsync("Base");
                user = testbase;
            }
            string Id = user!.Id;
            return Id;
        }

        [Authorize(Roles = RoleNames.Administrator)]
        [HttpPost("{UserId}")]
        public async Task<IActionResult> AddRoleModerator(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                return NotFound("không tìm thấy người dùng");
            }
            else if (user != null && !await _userManager.IsInRoleAsync(user, RoleNames.Moderator))
            {
                await _userManager.AddToRoleAsync(user, RoleNames.Moderator);
                return new JsonResult(new
                {
                    Result = "Thêm Role thành công"
                });
            }
            else
            {
                return new JsonResult(new
                {
                    Result = "Người dùng đã có sẵn quyền Moderator"
                });
            }
        }

        /*[Authorize(Roles =RoleNames.Moderator)]
        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {
           var users = await _context.Users.ToListAsync();
            return Ok(users);
        }*/
        [HttpGet]
        /*        public async Task<IActionResult> GetAllUser()
                {
                    var currentUser = await _userManager.GetUserAsync(User);

                    if (currentUser == null)
                    {
                        return Unauthorized(); // Người dùng chưa đăng nhập
                    }

                    if (await _userManager.IsInRoleAsync(currentUser, RoleNames.Administrator))
                    {
                        // Người dùng đăng nhập có vai trò Administrator, trả về tất cả danh sách người dùng
                        var users = await _context.Users.ToListAsync();
                        return Ok(users);
                    }
                    else if (await _userManager.IsInRoleAsync(currentUser, RoleNames.Moderator))
                    {
                        // Lấy danh sách tất cả người dùng
                        var users = await _userManager.Users.ToListAsync();

                        // Lọc danh sách người dùng không có vai trò Moderator và không có vai trò Administrator
                        var filteredUsers = users.Where(user =>
                        {
                            var isInModeratorRole = _userManager.IsInRoleAsync(user, RoleNames.Moderator).GetAwaiter().GetResult();
                            var isInAdministratorRole = _userManager.IsInRoleAsync(user, RoleNames.Administrator).GetAwaiter().GetResult();
                            return !isInModeratorRole && !isInAdministratorRole;
                        }).ToList();

                        return Ok(filteredUsers);
                    }
                    else
                    {
                        return Forbid(); // Người dùng không có vai trò Administrator hoặc Moderator, trả về mã lỗi 403 (Forbidden)
                    }
                }*/
        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {
            var users = await _userManager.Users.ToListAsync();
            return Ok(users);
        }
        [HttpGet]
        public IActionResult GetUserRoles()
        {
            var currentUser = _userManager.GetUserAsync(User).Result;
            var userRoles = _userManager.GetRolesAsync(currentUser!).Result;

            if (userRoles != null && userRoles.Any())
            {
                return Ok(userRoles);
            }
            else
            {
                return NoContent(); // Hoặc trả về giá trị tùy chỉnh khác thể hiện rằng người dùng không có vai trò nào
            }
        }
        /*        [Authorize]*/
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromForm] UserModel updatedUser)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound(); // Trả về lỗi 404 nếu không tìm thấy người dùng với Id tương ứng
            }
            if (!string.IsNullOrEmpty(updatedUser.UserName))
            {
                user.UserName = updatedUser.UserName;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return Ok(); // Trả về thành công nếu cập nhật thành công
                }
                else
                {
                    return BadRequest(result.Errors); // Trả về lỗi 400 nếu có lỗi trong quá trình cập nhật
                }
            }
            if (!string.IsNullOrEmpty(updatedUser.FullName))
            {
                user.FullName = updatedUser.FullName;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return Ok(); // Trả về thành công nếu cập nhật thành công
                }
                else
                {
                    return BadRequest(result.Errors); // Trả về lỗi 400 nếu có lỗi trong quá trình cập nhật
                }
            }
            if (!string.IsNullOrEmpty(updatedUser.Email))
            {
                user.Email = updatedUser.Email;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return Ok(); // Trả về thành công nếu cập nhật thành công
                }
                else
                {
                    return BadRequest(result.Errors); // Trả về lỗi 400 nếu có lỗi trong quá trình cập nhật
                }
            }
            if (!string.IsNullOrEmpty(updatedUser.Address))
            {
                user.Address = updatedUser.Address;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return Ok(); // Trả về thành công nếu cập nhật thành công
                }
                else
                {
                    return BadRequest(result.Errors); // Trả về lỗi 400 nếu có lỗi trong quá trình cập nhật
                }
            }
            if (updatedUser.PhoneNumber != null)
            {
                user.PhoneNumber = updatedUser.PhoneNumber.Trim();
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return Ok(); // Trả về thành công nếu cập nhật thành công
                }
                else
                {
                    return BadRequest(result.Errors); // Trả về lỗi 400 nếu có lỗi trong quá trình cập nhật
                }
            }
            if (updatedUser.Image == null || updatedUser.Image.Length == 0)
            {
                user.AvatarUrl = user.AvatarUrl;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return Ok(); // Trả về thành công nếu cập nhật thành công
                }
                else
                {
                    return BadRequest(result.Errors); // Trả về lỗi 400 nếu có lỗi trong quá trình cập nhật
                }
            }
            else
            {
                string imgFileName = Guid.NewGuid().ToString() + Path.GetExtension(updatedUser.Image.FileName);
                string imgFolderPath = Path.Combine("wwwroot/Avatars"); // Thư mục "wwwroot/Image"
                string imgFilePath = Path.Combine(imgFolderPath, imgFileName);

                if (!Directory.Exists(imgFolderPath))
                {
                    Directory.CreateDirectory(imgFolderPath);
                }

                using (var stream = new FileStream(imgFilePath, FileMode.Create))
                {
                    await updatedUser.Image.CopyToAsync(stream);
                }

                user.AvatarUrl = "Avatars/" + imgFileName;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return Ok(); // Trả về thành công nếu cập nhật thành công
                }
                else
                {
                    return BadRequest(result.Errors); // Trả về lỗi 400 nếu có lỗi trong quá trình cập nhật
                }
            }


        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string userId, string currentPassword, string newPassword, string confirmPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                // Người dùng không tồn tại
                return NotFound();
            }

            // Kiểm tra mật khẩu hiện tại
            var passwordCheckResult = await _userManager.CheckPasswordAsync(user, currentPassword);
            if (!passwordCheckResult)
            {
                // Mật khẩu hiện tại không đúng
                return BadRequest("Incorrect current password");
            }

            if (newPassword != confirmPassword)
            {
                // Mật khẩu mới và xác nhận mật khẩu không khớp
                return BadRequest("Passwords do not match");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (result.Succeeded)
            {
                // Mật khẩu đã được thay đổi thành công
                return Ok("Password changed successfully");
            }
            else
            {
                // Đã xảy ra lỗi khi thay đổi mật khẩu
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(errors);
            }
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(string username, string newPassword, string confirmPassword)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                // Người dùng không tồn tại
                return NotFound();
            }

            // Kiểm tra newPassword và confirmPassword
            if (newPassword != confirmPassword)
            {
                // Mật khẩu mới và xác nhận mật khẩu không khớp
                return BadRequest("Passwords do not match");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (result.Succeeded)
            {
                // Mật khẩu đã được đặt lại thành công
                return Ok("Password reset successfully");
            }
            else
            {
                // Đã xảy ra lỗi khi đặt lại mật khẩu
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(errors);
            }
        }


        [HttpDelete("{Id}")]

        public async Task<IActionResult> DeleteUser(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                // Người dùng không tồn tại
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);

            await _context.SaveChangesAsync();
            return Ok(result);
        }
    }
}
