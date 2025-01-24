﻿using DessertApp.Models.Entities;
using DessertApp.Services.RepositoriesServices.DomainRepositories;
using DessertApp.ViewModels.DomainVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DessertApp.Controllers.IngredientEntity
{
    [Authorize(Roles = "Admin")]
    public class IngredientEntityController : Controller
    {
        private readonly IIngredientService _ingredientService;
        private readonly IMeasurementUnitService _measurementUnitService;
        private readonly ILogger<IngredientEntityController> _logger;

        public IngredientEntityController(
            IIngredientService ingredientService,
            IMeasurementUnitService measurementUnitService,
            ILogger<IngredientEntityController> logger)
        {
            _ingredientService = ingredientService;
            _measurementUnitService = measurementUnitService;
            _logger = logger;
        }

        //List all ingredients
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var ingredients = await _ingredientService.GetAllIngredientsAsync(cancellationToken);
            return View(ingredients);
        }

        //Get an ingredient detail
        public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
        {
            var ingredient = await _ingredientService.GetIngredientWithUnitsAsync(id, cancellationToken);
            if (ingredient == null)
            {
                return NotFound();
            }
            return View(ingredient);
        }

        //Create a new ingredient
        public IActionResult Create()
        {
            return View();
        }

        //Create a new ingredient with a default Ingredient Unit : POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ingredient ingredient, IngredientUnit ingredientUnit, CancellationToken cancellationToken)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _ingredientService.CreateIngredientAsync(ingredient, ingredientUnit, cancellationToken);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var state in ModelState)
                    {
                        if (state.Value.Errors.Count > 0)
                        {
                            _logger.LogError($"Property {state.Key} failed validation: {string.Join(", ", state.Value.Errors.Select(e => e.ErrorMessage))}");
                        }
                    }
                    return View(ingredient);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to save changes, try again. If the problem persists see your system administrator.");
                _logger.LogInformation($"Unable to save changes, see details: {ex.Message}");
            }
            return View(ingredient);
        }

        

        //GET: Ingredients/Delete/id
        //Delete an ingredient (Included Ingredient Units)
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var ingredient = await _ingredientService.GetIngredientByIdAsync(id, cancellationToken);
            if (ingredient == null)
            {
                return NotFound();
            }
            return View(ingredient);
        }

        //POST: Ingredients/DeleteConfirmed/id
        //Confirm the deletion of ingredient (Included Ingredient Units)
        [HttpPost, ActionName(nameof(Delete))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
        {
            var success = await _ingredientService.DeleteIngredientWhitUnitsAsync(id, cancellationToken);
            if (!success)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        //Edit an ingredient: GET
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var ingredient = await _ingredientService.GetIngredientWithUnitsAsync(id, cancellationToken);
            if (ingredient == null)
            {
                return NotFound();
            }

            var viewModel = new IngredientEditVM
            {
                Id = ingredient.Id,
                Name = ingredient.Name,
                Stock = ingredient.Stock,
                IngredientUnitVM = new IngredientUnitVM
                {
                    Id = ingredient.IngredientUnit!.Id,
                    IngredientId = ingredient.Id,
                    ItemsPerUnit = ingredient.IngredientUnit.ItemsPerUnit,
                    CostPerUnit = ingredient.IngredientUnit?.CostPerUnit,
                    OrderingCost = ingredient.IngredientUnit?.OrderingCost,
                    MonthlyHoldingCostRate = ingredient.IngredientUnit?.MonthlyHoldingCostRate,
                    AnnualDemand = ingredient.IngredientUnit?.AnnualDemand
                },
                AvailableUnits =
                new SelectList(
                    await _measurementUnitService.GetMeasurementUnitsAsync(cancellationToken),
                    "Id", "Name")
            };
            return View(viewModel);
        }

        //Edit an ingredient : POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IngredientEditVM model, CancellationToken cancellationToken)
        {
            // Validar que el ID del modelo coincida con el ID del ingrediente
            if (id != model.Id)
            {
                return BadRequest();
            }

            // Verificar si el modelo es válido
            if (ModelState.IsValid)
            {
                // Mapear el ViewModel al modelo de dominio
                var ingredient = new Ingredient
                {
                    Id = model.Id,
                    Name = model.Name,
                    Stock = model.Stock ?? 0,
                };

                var updatedUnits = new IngredientUnit
                {
                    Id = model.IngredientUnitVM.Id,
                    IngredientId = ingredient.Id,
                    UnitId = model.IngredientUnitVM.SelectedUnitId,
                    ItemsPerUnit = model.IngredientUnitVM.ItemsPerUnit ?? 0,
                    CostPerUnit = model.IngredientUnitVM.CostPerUnit,
                    OrderingCost = model.IngredientUnitVM.OrderingCost,
                    MonthlyHoldingCostRate = model.IngredientUnitVM.MonthlyHoldingCostRate,
                    AnnualDemand = model.IngredientUnitVM.AnnualDemand
                };


                // Actualizar el ingrediente y sus unidades
                await _ingredientService.UpdateIngredientAsync(ingredient, cancellationToken, updatedUnits);

                return RedirectToAction(nameof(Index));
            }

            model.AvailableUnits = new SelectList(
                await _measurementUnitService.GetMeasurementUnitsAsync(cancellationToken),
                "Id", "Name");
            return View(model);
        }
    }
}
