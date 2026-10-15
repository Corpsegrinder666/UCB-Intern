// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Validation message dialog
document.addEventListener("DOMContentLoaded", function () {
    const modal = document.getElementById("validationModal");

    if (!modal) {
        return;
    }

    const closeButtons = modal.querySelectorAll("[data-validation-close]");

    function closeModal() {
        modal.classList.remove("show");
        modal.setAttribute("aria-hidden", "true");
    }

    closeButtons.forEach(function (button) {
        button.addEventListener("click", closeModal);
    });

    modal.addEventListener("click", function (event) {
        if (event.target === modal) {
            closeModal();
        }
    });

    document.addEventListener("keydown", function (event) {
        if (event.key === "Escape" && modal.classList.contains("show")) {
            closeModal();
        }
    });

    if (modal.dataset.show === "true") {
        modal.classList.add("show");
        modal.setAttribute("aria-hidden", "false");
    }
});
