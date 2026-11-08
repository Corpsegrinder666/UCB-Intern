
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
     * Sidebar and menu functionality
     */

    const sidebar = document.getElementById("sidebar");
    const sidebarToggle = document.getElementById("sidebarToggle");

    if (sidebar) {
        if (localStorage.getItem("cibSidebarCollapsed") === "true") {
            sidebar.classList.add("collapsed");
        }
    }

    if (sidebar && sidebarToggle) {
        sidebarToggle.addEventListener("click", function () {
            sidebar.classList.toggle("collapsed");

            localStorage.setItem(
                "cibSidebarCollapsed",
                sidebar.classList.contains("collapsed")
            );
        });
    }

    document.querySelectorAll("[data-menu-toggle]").forEach(function (button) {
        button.addEventListener("click", function () {
            const menuId = button.getAttribute("data-menu-toggle");
            const menu = document.getElementById(menuId);

            if (!menu) {
                return;
            }

            if (menu.style.display === "none") {
                menu.style.display = "block";
                button.classList.add("open");
            } else {
                menu.style.display = "none";
                button.classList.remove("open");
            }
        });
    });

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
     * Personal Information page functionality
     */

    const personalDeleteOpen = document.querySelector(
        "[data-personal-delete-open]"
    );

    if (personalDeleteOpen) {
        personalDeleteOpen.addEventListener("click", function () {
            const checkboxes = document.querySelectorAll(
                '#deleteForm input[name="SelectedCodes"]:checked'
            );

            if (checkboxes.length === 0) {
                window.showSharedMessage(
                    "warning",
                    "Please select at least one record to delete."
                );
                return;
            }

            const modal = document.getElementById("deleteConfirmModal");

            if (modal) {
                modal.classList.remove("hidden");
                modal.style.display = "flex";
                modal.setAttribute("aria-hidden", "false");
            }
        });
    }

    const personalDeleteConfirm = document.querySelector(
        "[data-personal-delete-confirm]"
    );

    if (personalDeleteConfirm) {
        personalDeleteConfirm.addEventListener("click", function () {
            const form = document.getElementById("deleteForm");
            const modal = document.getElementById("deleteConfirmModal");

            if (form) {
                if (modal) {
                    modal.classList.add("hidden");
                    modal.style.display = "none";
                    modal.setAttribute("aria-hidden", "true");
                }

                form.requestSubmit();
            }
        });
    }

    const personalRefresh = document.querySelector(
        "[data-personal-refresh]"
    );

    if (personalRefresh) {
        personalRefresh.addEventListener("click", function () {
            window.location.href = "/Subject/PersonalInformation";
        });
    }

    /*
     * Institution Information page functionality
     */

    const institutionDeleteOpen = document.querySelector(
        "[data-institution-delete-open]"
    );

    if (institutionDeleteOpen) {
        institutionDeleteOpen.addEventListener("click", function () {
            const selected = document.querySelectorAll(
                '#deleteForm input[name="SelectedCodes"]:checked'
            );

            if (selected.length === 0) {
                window.showSharedMessage(
                    "warning",
                    "Please select at least one record to delete."
                );
                return;
            }

            const modal = document.getElementById("deleteConfirmModal");

            if (modal) {
                modal.classList.remove("hidden");
                modal.style.display = "flex";
                modal.setAttribute("aria-hidden", "false");
            }
        });
    }

    const institutionDeleteConfirm = document.querySelector(
        "[data-institution-delete-confirm]"
    );

    if (institutionDeleteConfirm) {
        institutionDeleteConfirm.addEventListener("click", function () {
            const form = document.getElementById("deleteForm");
            const modal = document.getElementById("deleteConfirmModal");

            if (form) {
                if (modal) {
                    modal.classList.add("hidden");
                    modal.style.display = "none";
                    modal.setAttribute("aria-hidden", "true");
                }

                form.requestSubmit();
            }
        });
    }

    const institutionRefresh = document.querySelector(
        "[data-institution-refresh]"
    );

    if (institutionRefresh) {
        institutionRefresh.addEventListener("click", function () {
            window.location.reload();
        });
    }

    /*
     * Owner Links page functionality
     */

    const ownerDeleteOpen = document.querySelector(
        "[data-owner-delete-open]"
    );

    if (ownerDeleteOpen) {
        ownerDeleteOpen.addEventListener("click", function () {
            const checkboxes = document.querySelectorAll(
                '#deleteForm input[name="SelectedLinks"]:checked'
            );

            if (checkboxes.length === 0) {
                window.showSharedMessage(
                    "warning",
                    "Please select at least one owner link record before deleting."
                );
                return;
            }

            const modal = document.getElementById("deleteConfirmModal");

            if (modal) {
                modal.classList.remove("hidden");
                modal.style.display = "flex";
                modal.setAttribute("aria-hidden", "false");
            }
        });
    }

    const ownerDeleteConfirm = document.querySelector(
        "[data-owner-delete-confirm]"
    );

    if (ownerDeleteConfirm) {
        ownerDeleteConfirm.addEventListener("click", function () {
            const form = document.getElementById("deleteForm");
            const modal = document.getElementById("deleteConfirmModal");

            if (form) {
                if (modal) {
                    modal.classList.add("hidden");
                    modal.style.display = "none";
                    modal.setAttribute("aria-hidden", "true");
                }

                form.requestSubmit();
            }
        });
    }

    const ownerRefresh = document.querySelector(
        "[data-owner-refresh]"
    );

    if (ownerRefresh) {
        ownerRefresh.addEventListener("click", function () {
            window.location.reload();
        });
    }

    /*
     * Installment Contract page functionality
     */

    const installmentSearchForm = document.getElementById("searchForm");

    if (installmentSearchForm) {
        installmentSearchForm.addEventListener("submit", function (event) {
            const fields = [
                "BranchCode",
                "ContractCode",
                "FISubjectCode",
                "ContractPhase",
                "UbsAcNo"
            ];

            const hasSearchValue = fields.some(function (name) {
                const field = installmentSearchForm.querySelector(
                    '[name="' + name + '"]'
                );

                return field && field.value.trim() !== "";
            });

            if (!hasSearchValue) {
                event.preventDefault();

                window.showSharedMessage(
                    "warning",
                    "Please enter or select at least one search option."
                );
            }
        });
    }

    const installmentDeleteOpen = document.querySelector(
        "[data-installment-delete-open]"
    );

    if (installmentDeleteOpen) {
        installmentDeleteOpen.addEventListener("click", function () {
            const form = document.getElementById("deleteContractsForm");

            if (!form) {
                return;
            }

            const selected = form.querySelectorAll(
                'input[name="SelectedCodes"]:checked'
            );

            if (selected.length === 0) {
                window.showSharedMessage(
                    "warning",
                    "Please select at least one record to delete."
                );
                return;
            }

            const modal = document.getElementById("deleteConfirmModal");

            if (modal) {
                modal.classList.remove("hidden");
                modal.style.display = "flex";
                modal.setAttribute("aria-hidden", "false");
            }
        });
    }

    const installmentDeleteConfirm = document.querySelector(
        "[data-installment-delete-confirm]"
    );

    if (installmentDeleteConfirm) {
        installmentDeleteConfirm.addEventListener("click", function () {
            const form = document.getElementById("deleteContractsForm");
            const modal = document.getElementById("deleteConfirmModal");

            if (form) {
                if (modal) {
                    modal.classList.add("hidden");
                    modal.style.display = "none";
                    modal.setAttribute("aria-hidden", "true");
                }

                form.requestSubmit();
            }
        });
    }

    const installmentRefresh = document.querySelector(
        "[data-installment-refresh]"
    );

    if (installmentRefresh) {
        installmentRefresh.addEventListener("click", function () {
            window.location.reload();
        });
    }

    /*
     * Non-Installment Contract page functionality
     */

    const nonInstallmentDeleteForm = document.getElementById("deleteContractsForm");
    const nonInstallmentDeleteModal = document.getElementById("deleteConfirmModal");
    const nonInstallmentConfirmButton = document.getElementById("confirmDeleteButton");
    const nonInstallmentCancelButton = document.getElementById("cancelDeleteButton");

    if (
        nonInstallmentDeleteForm &&
        nonInstallmentDeleteModal &&
        nonInstallmentConfirmButton &&
        nonInstallmentCancelButton
    ) {
        let deleteConfirmed = false;

        function openNonInstallmentDeleteConfirmation() {
            nonInstallmentDeleteModal.style.display = "flex";
            nonInstallmentDeleteModal.style.removeProperty("display");
            nonInstallmentDeleteModal.classList.add("show");
            nonInstallmentConfirmButton.focus();
        }

        function closeNonInstallmentDeleteConfirmation() {
            nonInstallmentDeleteModal.classList.remove("show");
            nonInstallmentDeleteModal.style.display = "none";
        }

        nonInstallmentDeleteForm.addEventListener("submit", function (event) {
            if (deleteConfirmed) {
                return;
            }

            event.preventDefault();

            const selected = nonInstallmentDeleteForm.querySelectorAll(
                'input[name="SelectedCodes"]:checked'
            );

            if (selected.length === 0) {
                window.showSharedMessage(
                    "warning",
                    "Please select at least one record to delete."
                );

                return;
            }

            openNonInstallmentDeleteConfirmation();
        });

        nonInstallmentConfirmButton.addEventListener("click", function () {
            deleteConfirmed = true;
            closeNonInstallmentDeleteConfirmation();
            nonInstallmentDeleteForm.submit();
        });

        nonInstallmentCancelButton.addEventListener("click", function () {
            closeNonInstallmentDeleteConfirmation();
        });

        nonInstallmentDeleteModal.addEventListener("click", function (event) {
            if (event.target === nonInstallmentDeleteModal) {
                closeNonInstallmentDeleteConfirmation();
            }
        });
    }

    document.querySelectorAll("[data-noninstallment-refresh]").forEach(function (button) {
        button.addEventListener("click", function () {
            location.reload();
        });
    });

    /*
     * Contract Links page functionality
     */

    const contractCodeInput = document.getElementById("fiContractCode");
    const primaryCodeInput = document.getElementById("fiPrimaryCode");

    if (contractCodeInput && primaryCodeInput) {
        let lastLookupCode = "";
        let lookupRequest = 0;
        let lookupTimer = null;

        async function lookupPrimaryCode() {
            const contractCode = contractCodeInput.value.trim();

            if (!contractCode) {
                lookupRequest++;
                lastLookupCode = "";
                primaryCodeInput.value = "";
                primaryCodeInput.setCustomValidity("");
                return;
            }

            if (
                contractCode === lastLookupCode &&
                primaryCodeInput.value.trim() !== ""
            ) {
                return;
            }

            lastLookupCode = contractCode;

            const currentRequest = ++lookupRequest;

            primaryCodeInput.value = "";
            primaryCodeInput.setCustomValidity("");

            try {
                const lookupUrl = new URL(
                    contractCodeInput.dataset.primaryCodeLookupUrl,
                    window.location.origin
                );

                lookupUrl.searchParams.set("contractCode", contractCode);

                const response = await fetch(lookupUrl);

                if (!response.ok) {
                    throw new Error("The contract lookup request failed.");
                }

                const result = await response.json();

                // Ignore an outdated response if the user has typed a new code.
                if (currentRequest !== lookupRequest) {
                    return;
                }

                if (result.success) {
                    primaryCodeInput.value = result.primaryCode;
                    primaryCodeInput.setCustomValidity("");
                } else {
                    lastLookupCode = "";

                    primaryCodeInput.value = "";
                    primaryCodeInput.setCustomValidity(
                        result.message || "No matching contract was found."
                    );

                    primaryCodeInput.reportValidity();
                }
            } catch (error) {
                if (currentRequest !== lookupRequest) {
                    return;
                }

                lastLookupCode = "";

                primaryCodeInput.value = "";
                primaryCodeInput.setCustomValidity(
                    "Unable to retrieve the primary code. Please try again."
                );

                primaryCodeInput.reportValidity();

                console.error("Primary code lookup error:", error);
            }
        }

        /*
         * Automatically look up the primary code while typing.
         * The short delay prevents unnecessary requests for every keystroke.
         */

        contractCodeInput.addEventListener("input", function () {
            clearTimeout(lookupTimer);

            const contractCode = contractCodeInput.value.trim();

            // Invalidate any previous lookup response.
            lookupRequest++;
            lastLookupCode = "";

            // Clear the previous primary code immediately.
            primaryCodeInput.value = "";
            primaryCodeInput.setCustomValidity("");

            if (!contractCode) {
                return;
            }

            lookupTimer = setTimeout(function () {
                lookupPrimaryCode();
            }, 300);
        });

        /*
         * If the user leaves the field before the delay finishes,
         * perform the lookup immediately.
         */

        contractCodeInput.addEventListener("blur", function () {
            clearTimeout(lookupTimer);
            lookupPrimaryCode();
        });
    }

    const contractLinksDeleteForm = document.getElementById("deleteLinksForm");
    const contractLinksDeleteModal = document.getElementById("deleteConfirmModal");

    const contractLinksDeleteOpenButton = document.querySelector(
        "[data-contract-links-delete-open]"
    );

    const contractLinksDeleteConfirmButton = document.querySelector(
        "[data-contract-links-delete-confirm]"
    );

    if (
        contractLinksDeleteForm &&
        contractLinksDeleteModal &&
        contractLinksDeleteOpenButton &&
        contractLinksDeleteConfirmButton
    ) {
        function openContractLinksDeleteConfirmation() {
            const selected = contractLinksDeleteForm.querySelectorAll(
                'input[name="SelectedLinks"]:checked'
            );

            if (selected.length === 0) {
                window.showSharedMessage(
                    "warning",
                    "Please select at least one record to delete."
                );
                return;
            }

            contractLinksDeleteModal.classList.remove("hidden");
            contractLinksDeleteModal.style.display = "flex";
            contractLinksDeleteModal.setAttribute("aria-hidden", "false");

            contractLinksDeleteConfirmButton.focus();
        }

        function closeContractLinksDeleteConfirmation() {
            contractLinksDeleteModal.classList.add("hidden");
            contractLinksDeleteModal.style.display = "none";
            contractLinksDeleteModal.setAttribute("aria-hidden", "true");
        }

        contractLinksDeleteOpenButton.addEventListener("click", function () {
            openContractLinksDeleteConfirmation();
        });

        contractLinksDeleteConfirmButton.addEventListener("click", function () {
            closeContractLinksDeleteConfirmation();
            contractLinksDeleteForm.requestSubmit();
        });

        contractLinksDeleteModal.addEventListener("click", function (event) {
            if (event.target === contractLinksDeleteModal) {
                closeContractLinksDeleteConfirmation();
            }
        });
    }

    const contractLinksRefresh = document.querySelector(
        "[data-contract-links-refresh]"
    );

    if (contractLinksRefresh) {
        contractLinksRefresh.addEventListener("click", function () {
            window.location.reload();
        });
    }

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
     * Yes, Delete is handled by the page-specific confirmation handler.
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
