
namespace EgCenterMgmt.Controllers
{
    [Route("[controller]/[Action]")]
    [ApiController]

    public class RoleController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize]
        [HttpGet("get-roles")]
        public IActionResult GetRoles()
        {
            try
            {
                var roles = _roleManager.Roles.ToList();
                if (roles == null || !roles.Any())
                {
                    return NotFound("No roles found.");
                }
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("role")]
        public async Task<IActionResult> GetUserRole([FromQuery] Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest("User ID is required.");
            }

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            if (roles == null || !roles.Any())
            {
                return NotFound("User has no roles.");
            }

            return Ok(roles);
        }



        [HttpPost("roles")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return BadRequest("Role name cannot be empty.");
            }

            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                var roleResult = await _roleManager.CreateAsync(new ApplicationRole(roleName));
                if (roleResult.Succeeded)
                {
                    return Ok("Role created successfully.");
                }
                return BadRequest("Error creating role.");
            }

            return BadRequest("Role already exists.");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("roles/{roleName}")]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return Ok("Role deleted successfully.");
                }
                return BadRequest("Error deleting role.");
            }

            return NotFound("Role not found.");
        }

        [HttpPost("users/{userId}/roles")]
        public async Task<IActionResult> AssignRoleToUser(Guid userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                var result = await _userManager.AddToRoleAsync(user, roleName);
                if (result.Succeeded)
                {
                    return Ok("Role assigned to user successfully.");
                }
                return BadRequest("Error assigning role to user.");
            }

            return NotFound("User not found.");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("users/{userId}/roles/{roleName}")]
        public async Task<IActionResult> RemoveRoleFromUser(Guid userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                var result = await _userManager.RemoveFromRoleAsync(user, roleName);
                if (result.Succeeded)
                {
                    return Ok("Role removed from user successfully.");
                }
                return BadRequest("Error removing role from user.");
            }

            return NotFound("User not found.");
        }
        
    }
}
