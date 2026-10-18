document.addEventListener("DOMContentLoaded", function () {

    const messageSelector =
        "#validationModal, #messageModal, #clientMessageModal, " +
        ".message-overlay:not(#deleteConfirmModal)";

    const titles = {
        error: "Error",
        warning: "Please Check",
        success: "Success",
        info: "Information"
    };

    /*
     * Close an ordinary message popup.
     * The delete-confirmation dialog is deliberately excluded.
     */
    function closeMessage(modal) {
        if (!modal || modal.id === "deleteConfirmModal") {
            return;
        }

        if (modal.id === "validationModal") {
            modal.classList.remove("show");
        } else {
            modal.classList.add("hidden");
        }

        modal.style.display = "none";
        modal.setAttribute("aria-hidden", "true");
    }

    function isVisible(modal) {
        return !!modal &&
            modal.isConnected &&
            modal.getAttribute("aria-hidden") !== "true" &&
            !modal.classList.contains("hidden") &&
            window.getComputedStyle(modal).display !== "none";
    }

    /*
     * Initialize server-rendered message popups.
     * Do not initialize delete-confirmation dialogs here.
     */
    function initializeMessage(modal) {
        if (
            !modal ||
            modal.id === "deleteConfirmModal" ||
            modal.dataset.popupInitialized === "true"
        ) {
            return;
        }

        modal.dataset.popupInitialized = "true";

        modal.querySelectorAll(
            "[data-validation-close], " +
            ".message-close, .message-ok, .message-button, " +
            ".validation-modal-close, .validation-modal-footer button"
        ).forEach(function (button) {
            button.addEventListener("click", function (event) {
                event.preventDefault();
                closeMessage(modal);
            });
        });

        // Backdrop clicks close the message, not clicks inside its dialog.
        modal.addEventListener("click", function (event) {
            if (event.target === modal) {
                closeMessage(modal);
            }
        });

        if (
            modal.id === "validationModal" &&
            modal.dataset.show === "true"
        ) {
            modal.classList.add("show");
            modal.style.display = "flex";
            modal.setAttribute("aria-hidden", "false");
        } else if (modal.style.display !== "none") {
            modal.setAttribute("aria-hidden", "false");
        }

        // Success messages are temporary; other messages remain until closed.
        const isSuccess = modal.querySelector(
            ".message-header.success, .message-body.success, " +
            ".message-button.success, .message-ok.success"
        );

        if (isSuccess && isVisible(modal)) {
            window.setTimeout(function () {
                closeMessage(modal);
            }, 3000);
        }
    }

    document.querySelectorAll(messageSelector).forEach(initializeMessage);

    /*
     * Shared popup API.
     * Pages can call window.showSharedMessage(type, message).
     */
    window.showSharedMessage = function (type, message) {
        const popupType = Object.prototype.hasOwnProperty.call(titles, type)
            ? type
            : "error";

        // Remove an earlier dynamically generated message, if present.
        document.querySelectorAll(".shared-dynamic-message").forEach(
            function (popup) {
                popup.remove();
            }
        );

        const overlay = document.createElement("div");
        overlay.className =
            "message-overlay shared-dynamic-message";
        overlay.setAttribute("role", "dialog");
        overlay.setAttribute("aria-modal", "true");
        overlay.setAttribute("aria-label", titles[popupType]);

        const dialog = document.createElement("div");
        dialog.className = "message-dialog";

        const header = document.createElement("div");
        header.className = "message-header " + popupType;

        const heading = document.createElement("h3");
        heading.textContent = titles[popupType];

        const closeButton = document.createElement("button");
        closeButton.type = "button";
        closeButton.className = "message-close";
        closeButton.setAttribute("aria-label", "Close message");
        closeButton.textContent = "\u00D7";

        const body = document.createElement("div");
        body.className = "message-body " + popupType;
        body.textContent = message == null ? "" : String(message);

        const footer = document.createElement("div");
        footer.className = "message-footer";

        const okButton = document.createElement("button");
        okButton.type = "button";
        okButton.className = "message-button " + popupType;
        okButton.textContent = "OK";

        function closePopup() {
            overlay.remove();
        }

        closeButton.addEventListener("click", closePopup);
        okButton.addEventListener("click", closePopup);

        overlay.addEventListener("click", function (event) {
            if (event.target === overlay) {
                closePopup();
            }
        });

        header.appendChild(heading);
        header.appendChild(closeButton);
        footer.appendChild(okButton);

        dialog.appendChild(header);
        dialog.appendChild(body);
        dialog.appendChild(footer);
        overlay.appendChild(dialog);

        document.body.appendChild(overlay);

        if (popupType === "success") {
            window.setTimeout(function () {
                if (overlay.isConnected) {
                    overlay.remove();
                }
            }, 3000);
        }

        return overlay;
    };

    /*
     * Escape closes ordinary messages only.
     * Delete confirmation stays separate and requires its own controls.
     */
    document.addEventListener("keydown", function (event) {
        if (event.key !== "Escape") {
            return;
        }

        document.querySelectorAll(messageSelector).forEach(function (modal) {
            if (isVisible(modal)) {
                closeMessage(modal);
            }
        });

        document.querySelectorAll(".shared-dynamic-message").forEach(
            function (modal) {
                modal.remove();
            }
        );
    });

    /*
     * Delete confirmation: backdrop and Cancel close it.
     * Yes, Delete is still handled by the page's confirmDelete() function.
     */
    const deleteModal = document.getElementById("deleteConfirmModal");

    if (deleteModal) {
        deleteModal.querySelectorAll(
            ".confirm-cancel, [data-delete-cancel]"
        ).forEach(function (button) {
            button.addEventListener("click", function (event) {
                event.preventDefault();
                deleteModal.classList.add("hidden");
                deleteModal.style.display = "none";
                deleteModal.setAttribute("aria-hidden", "true");
            });
        });

        deleteModal.addEventListener("click", function (event) {
            if (event.target === deleteModal) {
                deleteModal.classList.add("hidden");
                deleteModal.style.display = "none";
                deleteModal.setAttribute("aria-hidden", "true");
            }
        });
    }
});
