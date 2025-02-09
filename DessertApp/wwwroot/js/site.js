//Logic for show or hide banner (Error or Success)
document.addEventListener("DOMContentLoaded", function () {
    setTimeout(function () {
        document.querySelectorAll('.alert').forEach(alert => {
            alert.style.transition = "opacity 1s ease-out"; //Add soft transition
            alert.style.opacity = "0"; //Reduces opacity to 0

            //Wait 1s before delete element from DOM
            setTimeout(() => alert.style.display = "none", 1000);
        });
    }, 8000);
});

//Logic for...
document.addEventListener("DOMContentLoaded", function () {
    let container = document.getElementById("ingredients-container");
    let addIngredientBtn = document.getElementById("add-ingredient");
    let dessertSelector = document.getElementById("dessert-selector");
    let ingredientTemplate = document.getElementById("ingredient-template").content;

    // Obtener valores iniciales de los ingredientes y unidades
    let ingredientOptions = document.querySelector("[name='models[0].IngredientId']").innerHTML;
    let unitOptions = document.querySelector("[name='models[0].UnitId']").innerHTML;

    addIngredientBtn?.addEventListener("click", function () {
        let index = document.querySelectorAll(".ingredient-group").length;
        let newIngredient = document.importNode(ingredientTemplate, true);
        let ingredientDiv = newIngredient.querySelector(".ingredient-group");

        // Configurar el postre seleccionado automáticamente
        let dessertSelect = newIngredient.querySelector(".dessert-select");
        let hiddenDessertInput = newIngredient.querySelector(".hidden-dessert");
        dessertSelect.innerHTML = dessertSelector.innerHTML; // Copiar las opciones
        dessertSelect.value = dessertSelector.value; // Fijar el valor actual
        hiddenDessertInput.name = `models[${index}].DessertId`;
        hiddenDessertInput.value = dessertSelector.value;

        // Configurar los demás campos con nombres dinámicos
        newIngredient.querySelector(".ingredient-select").name = `models[${index}].IngredientId`;
        newIngredient.querySelector(".ingredient-select").innerHTML = ingredientOptions;

        newIngredient.querySelector(".quantity-input").name = `models[${index}].QuantityRequired`;

        newIngredient.querySelector(".unit-select").name = `models[${index}].UnitId`;
        newIngredient.querySelector(".unit-select").innerHTML = unitOptions;

        // Agregar evento para eliminar ingrediente
        newIngredient.querySelector(".remove-ingredient").addEventListener("click", function () {
            ingredientDiv.classList.add("fade-out");
            setTimeout(() => ingredientDiv.remove(), 500); // Se remueve después de la animación
        });

        // Agregar el nuevo ingrediente al formulario
        container.appendChild(newIngredient);

        // **Animación para resaltar el nuevo formulario**
        ingredientDiv.classList.add("highlight");
        setTimeout(() => ingredientDiv.classList.remove("highlight"), 1500);
    });

    // Actualizar los ingredientes nuevos si el usuario cambia el postre en el formulario original
    dessertSelector.addEventListener("change", function () {
        document.querySelectorAll(".dessert-select").forEach(select => {
            select.value = dessertSelector.value;
        });
        document.querySelectorAll(".hidden-dessert").forEach(input => {
            input.value = dessertSelector.value;
        });
    });
});


