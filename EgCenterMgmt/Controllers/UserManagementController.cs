
namespace EgCenterMgmt.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class UserManagementController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IUserManagementServices _Managerservices;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<UserManagementController> _logger;
        private readonly IMapper _mapper;
        private readonly ITenantService _tenantService;

        public UserManagementController(UserManager<ApplicationUser> userManager, IMapper mapper,
            ILogger<UserManagementController> logger,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            IPasswordHasher<ApplicationUser> passwordHash,
            IUserManagementServices Managerservices,
            ITenantService tenantService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _Managerservices = Managerservices;
            _passwordHasher = passwordHash;
            _signInManager = signInManager;
            _logger = logger;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _tenantService = tenantService;
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout([FromBody] object empty)
        {
            if (empty is not null)
            {
                // احصل على ID المستخدم الذي سجّل الدخول
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // قم بتسجيل الخروج
                await _signInManager.SignOutAsync();

                // يمكنك إرجاع ID المستخدم إذا كنت بحاجة لذلك
                return Ok(new { UserId = userId });
            }

            return Unauthorized();
        }


        [HttpGet]
        public async Task<IActionResult> GetUserInfo()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var currentTenant = _tenantService.GetCurrentTenant();
            var TenantName = currentTenant != null ? currentTenant.Name ?? string.Empty : string.Empty;

           // var TenantName = _tenantService.GetCurrentTenant()!.Name ?? string.Empty;
            var userInfo = new UserInfo
            {
                UserId = user.Id,
                Name = user.UserName!,
                Email = user.Email!,
                Roles = roles,
                TenantId = user.TenantId,
                TenantName = TenantName,
            };

            return Ok(userInfo);
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest login, [FromQuery] bool? useCookies, [FromQuery] bool? useSessionCookies)
        {
            var useCookieScheme = (useCookies == true) || (useSessionCookies == true);
            var isPersistent = (useCookies == true) && (useSessionCookies != true);
            _signInManager.AuthenticationScheme = useCookieScheme ? IdentityConstants.ApplicationScheme : IdentityConstants.BearerScheme;

            // Find the user
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null)
            {
                _logger.LogWarning("Login failed for {Email}: User not found.", login.Email);
                return Problem("Invalid email or password.", statusCode: StatusCodes.Status401Unauthorized);
            }

            // Check if the user is locked out
            if (await _userManager.IsLockedOutAsync(user))
            {
                return Problem("Your account is locked out. Please try again later.", statusCode: StatusCodes.Status403Forbidden);
            }

            // Attempt to sign in the user
            var result = await _signInManager.PasswordSignInAsync(user.UserName!, login.Password, isPersistent, lockoutOnFailure: true);
            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, login.Password);

            if (result.RequiresTwoFactor)
            {
                // Handle two-factor authentication
                if (!string.IsNullOrEmpty(login.TwoFactorCode))
                {
                    result = await _signInManager.TwoFactorAuthenticatorSignInAsync(login.TwoFactorCode, isPersistent, rememberClient: isPersistent);
                }
                else if (!string.IsNullOrEmpty(login.TwoFactorRecoveryCode))
                {
                    result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(login.TwoFactorRecoveryCode);
                }
            }

            if (!result.Succeeded)
            {
                _logger.LogWarning("Login failed for {Email}: Incorrect password.", login.Email);
                return Problem("Invalid email or password.", statusCode: StatusCodes.Status401Unauthorized);
            }

            return Ok(new { message = "Login successful." });
        }


        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userManager.Users
                .Where(u => u.isManage == false)
                .ToListAsync();

            var userDtos = new List<UserDto>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user); // استدعاء غير متزامن
                userDtos.Add(new UserDto
                {
                    UserId = user.Id.ToString(),
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = roles
                });
            }

            return Ok(userDtos);
        }

        [HttpGet]
        public IActionResult GetRoles()
        {
            var roles = _roleManager.Roles.Where(u => u.isManage == false).ToList();
            return Ok(roles);
        }
        [HttpGet]
        public async Task<IActionResult> GetRole(string userid)
        {
            var roles = _roleManager.Roles.Where(u => u.isManage == false).ToList();

            var user = await _userManager.FindByIdAsync(userid);
            if (user == null)
            {
                return NotFound("المستخدم غير موجود");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var rolesWithSelection = roles.Select(role => new RolesView
            {
                Id = role.Id,
                Name = role.Name,
                IsSelected = userRoles.Contains(role.Name!)
            }).ToList();

            return Ok(rolesWithSelection);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            var userDto = new EditUserDto
            {
                UserId = user.Id.ToString(),
                UserName = user.UserName!,
                Email = user.Email!,
                NatinalId = user.NatinalId,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                CreatedAt = user.CreatedAt,
            };

            return Ok(userDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] AddUserDto model)
        {
            try
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    CreatedAt = DateTime.UtcNow, // استخدم الوقت الحالي عند إنشاء المستخدم
                    Email = model.Email,
                    isManage = false,
                    isOwner = false
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                    return BadRequest(result.Errors);

                // إضافة المستخدم إلى الأدوار المحددة
                if (model.Roles != null && model.Roles.Count > 0)
                {
                    foreach (var role in model.Roles)
                    {
                        if (await _roleManager.RoleExistsAsync(role))
                        {
                            await _userManager.AddToRoleAsync(user, role);
                        }
                    }
                }
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }

        }


        // PUT: api/UserManagement/UpdateUser
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] EditUserDto model)
        {
            if (model == null)
            {
                return BadRequest("Invalid data.");
            }

            // البحث عن المستخدم باستخدام UserId
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // التحقق من صحة البريد الإلكتروني (اختياري، لضمان أن البريد الإلكتروني صحيح)
            if (!string.IsNullOrWhiteSpace(model.Email) && !_Managerservices.IsValidEmail(model.Email))
            {
                return BadRequest("Invalid email address.");
            }

            // التحقق من وجود بريد إلكتروني آخر بنفس الاسم
            if (!string.IsNullOrWhiteSpace(model.Email))
            {
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null && existingUser.Id.ToString() != model.UserId)
                {
                    return BadRequest("Email is already in use by another user.");
                }
            }

            user.UserName = model.UserName ?? user.UserName;
            user.Email = model.Email ?? user.Email;
            user.PhoneNumber = model.PhoneNumber ?? user.PhoneNumber;
            user.NatinalId = model.NatinalId ?? user.NatinalId;
            user.Address = model.Address ?? user.Address;
            user.CreatedAt = model.CreatedAt ?? user.CreatedAt;

            await _userManager.UpdateSecurityStampAsync(user);


            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("User updated successfully.");
        }


        // PUT: api/UserManagement/UpdateUserRole
        [HttpPut]
        public async Task<IActionResult> UpdateUserRole([FromBody] UpdateUserRoleDto model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId!.ToString());
            if (user == null)
                return NotFound();

            // الحصول على الأدوار الحالية وإزالتها
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            // إضافة الأدوار الجديدة
            if (model.Roles != null && model.Roles.Count > 0)
            {
                foreach (var role in model.Roles)
                {
                    if (await _roleManager.RoleExistsAsync(role))
                    {
                        await _userManager.AddToRoleAsync(user, role);
                    }
                }
            }

            return Ok();
        }

        // PUT: api/UserManagement/ResetPassword
        [HttpPut]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null)
                    return NotFound();

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword!);
                if (!result.Succeeded)
                    return BadRequest(result.Errors);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(error:ex.Message.ToString());
            }
        }
    }

}
