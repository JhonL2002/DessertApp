using DessertApp.Models.Entities;
using DessertApp.Services.Infraestructure.RepositoriesServices.DomainRepositories;
using DessertApp.Services.Infraestructure.RepositoriesServices.EntityRepositories;
using DessertApp.Services.Infraestructure.UnitOfWorkServices;
using DessertApp.ViewModels.EntitiesVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DessertApp.Controllers.Desserts
{
    public class DessertIngredientController : Controller
    {
        private readonly IDessertIngredientRepository _dessertIngredientService;
        private readonly IDessertRepository _dessertService;
        private readonly IIngredientRepository _ingredientService;
        private readonly IMeasurementUnitRepository _measurementUnitService;
        private readonly IUnitOfWork _unitOfWork;

        public DessertIngredientController(IDessertIngredientRepository dessertIngredientService,
            IDessertRepository dessertService,
            IIngredientRepository ingredientService,
            IMeasurementUnitRepository measurementUnitService,
            IUnitOfWork unitOfWork)
        {
            _dessertIngredientService = dessertIngredientService;
            _dessertService = dessertService;
            _ingredientService = ingredientService;
            _measurementUnitService = measurementUnitService;
            _unitOfWork = unitOfWork;
        }

        //GET: List all recipes
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var recipes = await _dessertIngredientService.GetAllDessertIngredientsAsync(cancellationToken);
            return View(recipes);
        }

        //GET: Show the creation form
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            var viewmodel = new DessertIngredientVM
            {
                Ingredients = new SelectList(await _ingredientService.GetAllIngredientsAsync(cancellationToken), "Id", "Name"),
                Desserts = new SelectList(await _unitOfWork.Desserts.GetAllAsync(cancellationToken), "Id", "Name"),
                Units = new SelectList(await _measurementUnitService.GetMeasurementUnitsAsync(cancellationToken), "Id", "Name"),
            };
            return View(new List<DessertIngredientVM> { viewmodel});
        }

        //POST: Send data to databse
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(List<DessertIngredientVM>? models, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid || models == null || models.Count == 0)
            {
                var viewmodel = new DessertIngredientVM
                {
                    Ingredients = new SelectList(await _ingredientService.GetAllIngredientsAsync(cancellationToken), "Id", "Name"),
                    Desserts = new SelectList(await _unitOfWork.Desserts.GetAllAsync(cancellationToken), "Id", "Name"),
                    Units = new SelectList(await _measurementUnitService.GetMeasurementUnitsAsync(cancellationToken), "Id", "Name")
                };
                return View(new List<DessertIngredientVM> { viewmodel });
            }

            //Create a list for ingredients in the recipe to save
            var dessertIngredients = models.Select(m => new DessertIngredient
            {
                DessertId = m.DessertId,
                IngredientId = m.IngredientId,
                QuantityRequired = m.QuantityRequired,
                UnitId = m.UnitId
            }).ToList();

            //Save all ingredients
            await _dessertIngredientService.CreateDessertIngredientAsync(dessertIngredients, cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        //GET: Show the edit form
        public async Task<IActionResult> Edit(int dessertId, CancellationToken cancellationToken)
        {
            var recipe = await _dessertIngredientService.GetDessertIngredientsByDessertIdAsync(dessertId, cancellationToken);
            if (recipe == null)
            {
                return NotFound();
            }
            return View(recipe);
        }

        //POST: Send updated data to database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DessertIngredient recipe, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(recipe);
            }

            await _dessertIngredientService.UpdateDessertIngredientAsync(recipe, cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        //GET: Get the recipe to delete
        public async Task<IActionResult> Delete(int dessertId, CancellationToken cancellationToken)
        {
            var recipe = await _dessertIngredientService.GetDessertIngredientsByDessertIdAsync(dessertId, cancellationToken);
            if (recipe == null || !recipe.Any())
            {
                return NotFound();
            }
            return View(recipe);
        }

        //POST: Delete the recipe
        [HttpPost, ActionName(nameof(Delete))]
        public async Task<IActionResult> DeleteConfirmed(int dessertId, CancellationToken cancellationToken)
        {
            await _dessertIngredientService.DeleteDessertIngredientAsync(dessertId, cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        //GET: Get all ingredients from recipe
        public async Task<IActionResult> Details(int dessertId, CancellationToken cancellationToken)
        {
            var ingredients = await _dessertIngredientService.GetDessertIngredientsByDessertIdAsync(dessertId, cancellationToken);

            if (ingredients == null || !ingredients.Any())
            {
                return NotFound("There's nothing ingredients to show for this dessert");
            }

            return View(ingredients);
        }

    }
}
