using DessertApp.Infraestructure.IdentityModels;
using DessertApp.Models.IdentityModels;
using DessertApp.Services.RoleStoreServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DessertApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleAdminController : Controller
    {
        private readonly IExtendedRoleStore<AppRole> _extendedRoleStore;
        private readonly IRoleStore<AppRole> _roleStore;
        public RoleAdminController(IExtendedRoleStore<AppRole> extendedRoleStore, IRoleStore<AppRole> roleStore)
        {
            _roleStore = roleStore;
            _extendedRoleStore = extendedRoleStore;
        }

        public async Task <IActionResult> Index(CancellationToken cancellationToken)
        {
            return View(await _extendedRoleStore.GetAllRolesAsync(cancellationToken));
        }

        // POST: RoleAdmin/Create
        [HttpPost]
        public async Task<IActionResult> Create(AppRole role)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleStore.CreateAsync(role, CancellationToken.None);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (var error  in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(role);
        }

        // GET: RoleAdmin/Edit/string_guid
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var role = await _roleStore.FindByIdAsync(id,CancellationToken.None);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        // POST: RoleAdmin/Edit/string_guid
        //public async Task<IActionResult> Edit(AppRole role);
    }
}
