document.addEventListener("DOMContentLoaded", () => {
    initializeDependencies();
});

/**
 * Initialize all form elements with dependencies
 */
const initializeDependencies = () => {
    document.querySelectorAll('[data-dependencies]').forEach(function (el) {
        // Initial evaluation
        handleDependencies(el, getElementValue(el));

        // Listen for GCDS component changes
        el.addEventListener("gcdsChange", (event) => {
            handleDependencies(el, getElementValue(event.target));
        });

        // Listen for native changes (backup)
        el.addEventListener("change", (event) => {
            handleDependencies(el, getElementValue(event.target));
        });

        // Listen for input events (for text fields)
        el.addEventListener("input", (event) => {
            handleDependencies(el, getElementValue(event.target));
        });
    });
};

// ... (rest of the JavaScript implementation) 