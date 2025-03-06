using DessertApp.Models.Entities;
using DessertApp.Services.Infraestructure.RepositoriesServices.DomainRepositories;
using DessertApp.Services.Infraestructure.RepositoriesServices.EntityRepositories;
using DessertApp.Services.Infraestructure.UnitOfWorkServices;
using DessertApp.ViewModels.EntitiesVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DessertApp.Controllers.Desserts
{
    [Authorize]
    public class DessertController : Controller
    {
        private readonly IDessertCategoryRepository _dessertCategoryService;
        private readonly IDessertRepository _dessertService;
        private readonly ILogger<DessertController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public DessertController(IDessertCategoryRepository dessertCategoryService, IDessertRepository dessertService, ILogger<DessertController> logger, IUnitOfWork unitOfWork)
        {
            _dessertCategoryService = dessertCategoryService;
            _dessertService = dessertService;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var desserts = await _unitOfWork.Desserts.GetAllAsync(cancellationToken);
            return View(desserts);
        }

        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            var viewmodel = new DessertVM
            {
                DessertCategories = new SelectList(await _dessertCategoryService.GetAllDessertCategoriesAsync(cancellationToken), "Id","Name")
            };

            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DessertVM model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                model.DessertCategories = new SelectList(await _dessertCategoryService.GetAllDessertCategoriesAsync(cancellationToken), "Id", "Name");
                return View(model);
            }

            var dessert = new Dessert
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Stock = model.Stock,
                IsAvailable = model.IsAvailable,
                AnnualDemand = model.AnnualDemand,
                DessertCategoryId = model.SelectedCategory
            };

            await _unitOfWork.Desserts.AddAsync(dessert, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        //GET: Delete a Dessert
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var dessert = await _unitOfWork.Desserts.GetByIdAsync(id, cancellationToken);
            if (dessert == null)
            {
                return NotFound();
            }
            return View(dessert);
        }

        //POST: Delete a Dessert (Confirm deletion)
        [HttpPost, ActionName(nameof(Delete))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
        {
            var dessert = await _unitOfWork.Desserts.GetByIdAsync(id, cancellationToken);

            if (dessert == null) return NotFound();

            await _unitOfWork.Desserts.DeleteAsync(dessert, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            TempData["SuccessMessage"] = "Dessert deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        //Get a Dessert
        public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
        {
            var dessert = await _dessertService.GetDessertWithCategoryAsync(id, cancellationToken);
            if (dessert == null)
            {
                return NotFound();
            }
            return View(dessert);
        }

        //Edit a Dessert: GET
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var dessert = await _dessertService.GetDessertWithCategoryAsync(id, cancellationToken);
            if (dessert == null)
            {
                return NotFound();
            }

            var viewModel = new DessertEditVM
            {
                Id = dessert.Id,
                Name = dessert.Name,
                Description = dessert.Description, 
                Price = dessert.Price,
                Stock = dessert.Stock,
                IsAvailable = dessert.IsAvailable,
                AnnualDemand = dessert.AnnualDemand ?? 0,
                SelectedCategory = dessert.DessertCategoryId,
                DessertCategories =
                new SelectList(
                    await _dessertCategoryService.GetAllDessertCategoriesAsync(cancellationToken),
                    "Id", "Name")
            };
            return View(viewModel);
        }

        //Edit an ingredient : POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DessertEditVM model, CancellationToken cancellationToken)
        {
            // Validate ID model matches ID Ingredient
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var dessert = new Dessert
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    Stock = model.Stock,
                    IsAvailable = model.IsAvailable,
                    AnnualDemand = model.AnnualDemand,
                    DessertCategoryId = model.SelectedCategory
                };

                await _unitOfWork.Desserts.UpdateAsync(dessert, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return RedirectToAction(nameof(Index));
            }
            model.DessertCategories = new SelectList(
                await _dessertCategoryService.GetAllDessertCategoriesAsync(cancellationToken),
                "Id", "Name");
            return View(model);
        }
    }
}
