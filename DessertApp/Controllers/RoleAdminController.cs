using DessertApp.Infraestructure.IdentityModels;
using DessertApp.Services.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DessertApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleAdminController : Controller
    {
        private readonly IGenericIdentityRepository<AppRole, IdentityResult,string> _roleRepository;
        public RoleAdminController(IGenericIdentityRepository<AppRole, IdentityResult,string> roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _roleRepository.GetAllAsync(CancellationToken.None));
        }

        // GET: RoleAdmin/Details/string_guid
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var role = await _roleRepository.GetDetailsAsync(id, CancellationToken.None);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: RoleAdmin/Create
        [HttpPost]
        public async Task<IActionResult> Create(Infraestructure.IdentityModels.AppRole role)
        {
            if (!ModelState.IsValid)
            {
                return View(role);
            }
            var result = await _roleRepository.CreateAsync(role, CancellationToken.None);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            return View(result);
        }

        // GET: RoleAdmin/Edit/string_guid
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var role = await _roleRepository.GetByIdAsync(id,CancellationToken.None);
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
