using DessertApp.Models.Entities;
using DessertApp.Services.CacheServices;
using DessertApp.Services.RepositoriesServices.DomainRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DessertApp.Controllers.Desserts
{
    [Authorize(Roles = "Admin")]
    public class DessertCategoryController : Controller
    {
        private readonly IDessertCategoryRepository _dessertCategoryService;
        private readonly IDessertCategoryCacheService _dessertCategoryCacheService;

        public DessertCategoryController(IDessertCategoryRepository dessertCategoryService, IDessertCategoryCacheService dessertCategoryCacheService)
        {
            _dessertCategoryService = dessertCategoryService;
            _dessertCategoryCacheService = dessertCategoryCacheService;
        }

        //GET: List all Dessert Categories
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            IEnumerable<DessertCategory> categories;
            categories = await _dessertCategoryCacheService.GetCategoriesWithDessertsAsync(cancellationToken);
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

            //Create the category
            await _dessertCategoryService.CreateDessertCategoryAsync(dessertCategory, cancellationToken);

            //Clear the chache memory
            _dessertCategoryCacheService.RemoveCategoriesCache();

            //Redirect to Index Action
            TempData["SuccessMessage"] = "Category created successfully!";
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
            _dessertCategoryCacheService.RemoveCategoriesCache();
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
            try
            {
                var success = await _dessertCategoryService.DeleteDessertCategoryAsync(id, cancellationToken);
                if (!success)
                {
                    return NotFound();
                }
                //Delete cache after delete dessert category
                _dessertCategoryCacheService.RemoveCategoriesCache();
                TempData["SuccessMessage"] = "Category deleted successfully!";
            }
            catch (DbUpdateException)
            {
                TempData["ErrorMessage"] = "The specified category cannot be deleted, currently is associated with one or more desserts";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
