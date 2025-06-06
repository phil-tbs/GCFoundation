document.addEventListener("DOMContentLoaded", () => {
    initializeDependencies();
});

/**
 * Initialize all form elements with dependencies
 */
const initializeDependencies = () => {
    // Create a map of source elements to their dependencies
    const dependencyMap = new Map();

    // First pass: collect all dependencies and organize them by source
    document.querySelectorAll('[data-dependencies]').forEach(function (el) {
        try {
            const dependencies = JSON.parse(el.getAttribute('data-dependencies'));
            dependencies.forEach(dependency => {
                const sourceId = dependency.sourceQuestionId;
                if (!dependencyMap.has(sourceId)) {
                    dependencyMap.set(sourceId, []);
                }
                // Store the target ID instead of the element reference
                dependencyMap.get(sourceId).push({
                    targetId: el.getAttribute('input-id') || 
                             el.getAttribute('select-id') || 
                             el.getAttribute('textarea-id') || 
                             el.getAttribute('radio-id') || 
                             el.getAttribute('checkbox-id') || 
                             el.getAttribute('id'),
                    dependency: dependency
                });
            });
        } catch (error) {
            console.error('Error parsing dependencies:', error);
        }
    });

    // Second pass: set up event listeners on source elements
    dependencyMap.forEach((dependencyList, sourceId) => {
        // Find the source element
        const sourceElement = document.querySelector(`
            [input-id="${sourceId}"],
            [select-id="${sourceId}"],
            [textarea-id="${sourceId}"],
            [id="${sourceId}"],
            [fieldset-id="${sourceId}"]
        `);

        if (!sourceElement) {
            console.warn(`Source element ${sourceId} not found for dependencies`);
            return;
        }

        // Initial evaluation for all dependencies
        const currentValue = getGCDSValue(sourceElement);
        dependencyList.forEach(({ targetId, dependency }) => {
            // Find the current element in the DOM
            const targetElement = document.querySelector(`
                [input-id="${targetId}"],
                [select-id="${targetId}"],
                [textarea-id="${targetId}"],
                [radio-id="${targetId}"],
                [checkbox-id="${targetId}"],
                [id="${targetId}"]
            `);

            if (!targetElement) {
                console.warn(`Target element with ID ${targetId} not found`);
                return;
            }

            const shouldApply = evaluateCondition(dependency, currentValue);
            applyDependencyAction(targetElement, dependency.action, shouldApply, dependency);
        });

        // Set up event listeners
        sourceElement.addEventListener("gcdsChange", (event) => {
            
            const value = event.detail;
            
            dependencyList.forEach(({ targetId, dependency }) => {
                // Find the current element in the DOM
                const targetElement = document.querySelector(`
                    [input-id="${targetId}"],
                    [select-id="${targetId}"],
                    [textarea-id="${targetId}"],
                    [radio-id="${targetId}"],
                    [checkbox-id="${targetId}"],
                    [id="${targetId}"]
                `);

                if (!targetElement) {
                    console.warn(`Target element with ID ${targetId} not found`);
                    return;
                }

                const shouldApply = evaluateCondition(dependency, value);
                applyDependencyAction(targetElement, dependency.action, shouldApply, dependency);
            });
        });
    });
};


/**
 * Get the value from a GCDS component
 */
const getGCDSValue = (element) => {
    if (!element) return null;

    // Handle different GCDS component types
    switch (element.tagName.toLowerCase()) {
        case 'gcds-input':
        case 'gcds-select':
        case 'gcds-textarea':
            return element.value;
        case 'gcds-checkbox':
            if (element.name) {
                const fieldset = element.closest('gcds-fieldset');
                if (fieldset) {
                    return Array.from(fieldset.querySelectorAll('gcds-checkbox:checked'))
                        .map(cb => cb.value);
                }
                const checkboxes = document.querySelectorAll(`gcds-checkbox[name="${element.name}"]:checked`);
                return Array.from(checkboxes).map(cb => cb.value);
            }
            return element.checked;
        case 'gcds-radio':
            const checkedRadio = document.querySelector(`gcds-radio[name="${element.name}"]:checked`);
            return checkedRadio ? checkedRadio.value : null;
        default:
            return element.value;
    }
};

/**
 * Evaluate if a dependency condition is met
 */
const evaluateCondition = (dependency, currentValue) => {
    const triggerValue = dependency.triggerValue;
    // Default to equals comparison since we don't have condition in the model anymore
    return String(currentValue) === String(triggerValue);
};

/**
 * Apply the dependency action to the target element
 */
const applyDependencyAction = (element, action, shouldApply, dependency) => {
    let wrapper = findFormGroupWrapper(element);

    switch (action) {
        case 0: // Require
            setRequired(element, shouldApply);
            break;
            
        case 1: // Show
            toggleVisibility(wrapper, shouldApply);
            if (shouldApply) {
                // reinitializeGCDSComponent(element);
            }
            break;
            
        case 2: // Hide
            toggleVisibility(wrapper, !shouldApply);
            if (!shouldApply) {
                // reinitializeGCDSComponent(element);
            }
            break;
            
        case 3: // Enable
            toggleDisabled(element, !shouldApply);
            // reinitializeGCDSComponent(element);
            break;
            
        case 4: // Disable
            toggleDisabled(element, shouldApply);
            // reinitializeGCDSComponent(element);
            break;
            
        case 5: // ClearValue
            if (shouldApply) {
                clearElementValue(element);
                // reinitializeGCDSComponent(element);
            }
            break;
            
        case 6: // SetValue
            if (shouldApply && dependency.setValue) {
                setElementValue(element, dependency.setValue);
                // reinitializeGCDSComponent(element);
            }
            break;
            
        default:
            console.warn(`Unsupported action: ${action}`);
    }
};

/**
 * Reinitialize a GCDS component without cloning
 */
const reinitializeGCDSComponent = (element) => {
    if (!element || !element.tagName.toLowerCase().startsWith('gcds-')) return;
    try {
        // First check if element is in the DOM
        if (!element.parentNode) {
            console.log(element);
            console.warn('Cannot reinitialize detached element:', element);
            return;
        }

        // Store current state
        const currentValue = getGCDSValue(element);
        const isRequired = element.hasAttribute('required');
        const isDisabled = element.hasAttribute('disabled');

        // Create and configure clone
        const clone = element.cloneNode(true);
        
        // Copy the current state to the clone
        if (isRequired) clone.setAttribute('required', '');
        if (isDisabled) clone.setAttribute('disabled', '');
        
        // Replace the element
        element.parentNode.replaceChild(clone, element);

        // Ensure the value is properly set after reinitialization
        if (currentValue !== null && currentValue !== undefined) {
            setTimeout(() => {
                setElementValue(clone, currentValue);
            }, 0);
        }

        return clone; // Return the new element for reference
    } catch (error) {
        console.error('Error reinitializing GCDS component:', error);
    }
};

/**
 * Find the form group wrapper for an element
 */
const findFormGroupWrapper = (element) => {
    // First try to find the closest form group
    const formGroup = element.closest('.gc-form-group');
    if (formGroup) return formGroup;
    
    // If element is or is inside a fieldset, return the fieldset's form group
    const fieldset = element.closest('gcds-fieldset') || 
                    (element.tagName.toLowerCase() === 'gcds-fieldset' ? element : null);
    if (fieldset) {
        return fieldset.closest('.gc-form-group') || fieldset.parentElement || element;
    }
    
    // Fallback to original behavior
    return element.parentElement || element;
};

/**
 * Toggle element visibility using classes instead of inline styles
 */
const toggleVisibility = (element, show) => {
    if (!element) return;
    
    if (show) {
        element.classList.remove('gc-form-hidden');
        // Reinitialize GCDS components when showing
        element.querySelectorAll('[class^="gcds-"]').forEach(gcdsElement => {
            reinitializeGCDSComponent(gcdsElement);
        });
    } else {
        element.classList.add('gc-form-hidden');
    }
};

/**
 * Toggle element disabled state
 */
const toggleDisabled = (element, disabled) => {
    if (element.disabled !== disabled) {
        element.disabled = disabled;
        element.setAttribute('aria-disabled', disabled);
    }
};

/**
 * Set required state for an element
 */
const setRequired = (element, required) => {
    if (!element) return;
    try {

        const clone = element.cloneNode(true);

        if (clone.tagName.toLowerCase().startsWith('gcds-')) {
            if (required) {
                clone.setAttribute('required', '');
            } else {
                clone.removeAttribute('required');
            }
        } else {
            // For native elements
            clone.required = required;
            if (required) {
                clone.setAttribute('required', '');
            } else {
                clone.removeAttribute('required');
            }
        }

        element.parentNode.replaceChild(clone, element);

    } catch (error) {
        console.warn('Error setting required state:', error);
    }
};

/**
 * Clear the value of an element
 */
const clearElementValue = (element) => {
    if (element.type === 'checkbox' || element.type === 'radio') {
        element.checked = false;
    } else {
        element.value = '';
    }
    triggerChangeEvent(element);
};

/**
 * Set the value of an element
 */
const setElementValue = (element, value) => {
    if (element.type === 'checkbox' || element.type === 'radio') {
        element.checked = value === true || value === 'true';
    } else {
        element.value = value;
    }
    triggerChangeEvent(element);
};
