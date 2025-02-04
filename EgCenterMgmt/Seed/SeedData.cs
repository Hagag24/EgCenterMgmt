
namespace EgCenterMgmt.Seed
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider,
            UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,
            string TenantId, string TenantName)
        {
            try
            {
                if (!await roleManager.RoleExistsAsync("SuperAdmin"))
                {
                    var superAdminRole = new ApplicationRole("SuperAdmin") { isManage = true };
                    await roleManager.CreateAsync(superAdminRole);
                }

                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    var adminRole = new ApplicationRole("Admin") { isManage = false };
                    await roleManager.CreateAsync(adminRole);
                }

                if (!await roleManager.RoleExistsAsync("Assistant"))
                {
                    var assistantRole = new ApplicationRole("Assistant") { isManage = false };
                    await roleManager.CreateAsync(assistantRole);
                }


                // إنشاء مستخدم أدمن ممتاز
                var superadminUser = await userManager.FindByEmailAsync(($"{TenantName}@superadmin.com").ToLower());
                if (superadminUser == null)
                {
                    superadminUser = new ApplicationUser
                    {
                        UserName = $"{TenantName} SuperAdmin",
                        Email = ($"{TenantName}@superadmin.com").ToLower(),
                        isManage = true,
                        TenantId = TenantId,
                        CreatedAt = DateTime.Now
                    };
                    await userManager.CreateAsync(superadminUser, "superadminP@ssword@2025#");
                    await userManager.AddToRoleAsync(superadminUser, "SuperAdmin");
                    await userManager.AddToRoleAsync(superadminUser, "Admin");

                    // إضافة Claims أو صلاحيات للأدمن (إن لزم الأمر)
                    await userManager.AddClaimAsync(superadminUser, new Claim("Permission", "FullAccess"));
                }
                // إنشاء مستخدم أدمن
                var adminUser = await userManager.FindByEmailAsync(($"{TenantName}@admin.com").ToLower());
                if (adminUser == null)
                {
                    adminUser = new ApplicationUser
                    {
                        UserName = $"{TenantName} Admin",
                        Email = ($"{TenantName}@admin.com").ToLower(),
                        isManage = true,
                        TenantId = TenantId,
                        CreatedAt = DateTime.Now
                    };
                    await userManager.CreateAsync(adminUser, "adminP@ssword@2025#");
                    await userManager.AddToRoleAsync(adminUser, "Admin");

                    // إضافة Claims أو صلاحيات للأدمن (إن لزم الأمر)
                    await userManager.AddClaimAsync(adminUser, new Claim("Permission", "FullAccess"));
                }


                // إنشاء مستخدم مساعد
                var assistantUser = await userManager.FindByEmailAsync(($"{TenantName}@assistant.com").ToLower());
                if (assistantUser == null)
                {
                    assistantUser = new ApplicationUser
                    {
                        UserName = $"{TenantName} Assistant",
                        Email = ($"{TenantName}@assistant.com").ToLower(),
                        isManage = true,
                        TenantId = TenantId,
                        CreatedAt = DateTime.Now
                    };
                    await userManager.CreateAsync(assistantUser, "AssistantP@ssword@2025#");
                    await userManager.AddToRoleAsync(assistantUser, "Assistant");

                    // إضافة Claims أو صلاحيات محددة للمساعد
                    await userManager.AddClaimAsync(assistantUser, new Claim("Permission", "LimitedAccess"));
                }

            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }
    }
}
