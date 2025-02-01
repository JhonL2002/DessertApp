using DessertApp.Models.Entities;
using DessertApp.Services.RepositoriesServices.DomainRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DessertApp.Controllers.Desserts
{
    [Authorize(Roles = "Admin")]
    public class DessertCategoryController : Controller
    {
        private readonly IDessertCategoryService _dessertCategoryService;

        public DessertCategoryController(IDessertCategoryService dessertCategoryService)
        {
            _dessertCategoryService = dessertCategoryService;
        }

        //GET: List all Dessert Categories
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var categories = await _dessertCategoryService.GetAllDessertCategoriesAsync(cancellationToken);
            return View(categories);
        }

        //GET: Show creation form
        public IActionResult Create()
        {
            return View();
        }

        //POST: Create a new Dessert Category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DessertCategory dessertCategory, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return View(dessertCategory);

            await _dessertCategoryService.CreateDessertCategoryAsync(dessertCategory, cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        //GET: Edit a Dessert Category
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var category = await _dessertCategoryService.GetDessertCategoryByIdAsync(id, cancellationToken);
            if (category == null) return NotFound();

            return View(category);
        }

        //POST: Update a Dessert Category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DessertCategory dessertCategory, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return View(dessertCategory);

            await _dessertCategoryService.UpdateDessertCategoryAsync(dessertCategory, cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        //GET: Delete a Dessert Category
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var category = await _dessertCategoryService.GetDessertCategoryByIdAsync(id, cancellationToken);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        //POST: Delete a Dessert Category (Confirm deletion)
        [HttpPost, ActionName(nameof(Delete))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
        {
            var success = await _dessertCategoryService.DeleteDessertCategoryAsync(id, cancellationToken);
            if (!success)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
