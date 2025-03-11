//Logic for...
document.addEventListener("DOMContentLoaded", function () {
    let container = document.getElementById("ingredients-container");
    let addIngredientBtn = document.getElementById("add-ingredient");
    let dessertSelector = document.getElementById("dessert-selector");
    let ingredientTemplate = document.getElementById("ingredient-template");

    if (!ingredientTemplate) {
        console.error("Error: No se encontró el template con id 'ingredient-template'");
        return;
    }

    let ingredientOptions = document.querySelector("[name='models[0].IngredientId']")?.innerHTML || "";
    let unitOptions = document.querySelector("[name='models[0].UnitId']")?.innerHTML || "";

    addIngredientBtn?.addEventListener("click", function () {
        let index = document.querySelectorAll(".ingredient-group").length;
        let newIngredient = document.importNode(ingredientTemplate.content, true);
        let ingredientDiv = newIngredient.querySelector(".ingredient-group");
        let dessertSelect = newIngredient.querySelector(".dessert-select");
        let hiddenDessertInput = newIngredient.querySelector(".hidden-dessert");

        dessertSelect.innerHTML = dessertSelector.innerHTML;
        dessertSelect.value = dessertSelector.value;
        hiddenDessertInput.name = `models[${index}].DessertId`;
        hiddenDessertInput.value = dessertSelector.value;

        newIngredient.querySelector(".ingredient-select").name = `models[${index}].IngredientId`;
        newIngredient.querySelector(".ingredient-select").innerHTML = ingredientOptions;

        newIngredient.querySelector(".quantity-input").name = `models[${index}].QuantityRequired`;

        newIngredient.querySelector(".unit-select").name = `models[${index}].UnitId`;
        newIngredient.querySelector(".unit-select").innerHTML = unitOptions;

        newIngredient.querySelector(".remove-ingredient").addEventListener("click", function () {
            ingredientDiv.classList.add("fade-out");
            setTimeout(() => ingredientDiv.remove(), 500);
        });

        container.appendChild(newIngredient);

        ingredientDiv.classList.add("highlight");
        setTimeout(() => ingredientDiv.classList.remove("highlight"), 1500);
    });

    dessertSelector.addEventListener("change", function () {
        document.querySelectorAll(".dessert-select").forEach(select => {
            select.value = dessertSelector.value;
        });
        document.querySelectorAll(".hidden-dessert").forEach(input => {
            input.value = dessertSelector.value;
        });
    });
});
