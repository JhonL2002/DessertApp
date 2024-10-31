using DessertApp.Models;
using DessertApp.Services.RoleStoreServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DessertApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleAdminController : Controller
    {
        private readonly IExtendedRoleStore<AppRole> _roleStore;
        public RoleAdminController(IExtendedRoleStore<AppRole> roleStore)
        {
            _roleStore = roleStore;
        }
        public async Task <IActionResult> Index(CancellationToken cancellationToken)
        {
            return View(await _roleStore.GetAllRolesAsync(cancellationToken));
        }
    }
}
