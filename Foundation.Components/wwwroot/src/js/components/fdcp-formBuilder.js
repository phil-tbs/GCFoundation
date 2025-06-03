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
                dependencyMap.get(sourceId).push({
                    targetElement: el,
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
            [radio-id="${sourceId}"],
            [checkbox-id="${sourceId}"]
        `);

        if (!sourceElement) {
            console.warn(`Source element ${sourceId} not found for dependencies`);
            return;
        }

        // Initial evaluation for all dependencies
        const currentValue = getGCDSValue(sourceElement);
        dependencyList.forEach(({ targetElement, dependency }) => {
            const shouldApply = evaluateCondition(dependency, currentValue);
            applyDependencyAction(targetElement, dependency.action, shouldApply, dependency);
        });

        // Set up event listeners
        sourceElement.addEventListener("gcdsChange", (event) => {
            
            const value = event.detail; //getGCDSValue(event.target);
            
            dependencyList.forEach(({ targetElement, dependency }) => {
                const shouldApply = evaluateCondition(dependency, value);
                applyDependencyAction(targetElement, dependency.action, shouldApply, dependency);
            });
        });
    });
};

/**
 * Handle dependencies for an element
 */
const handleDependencies = (el, currentValue) => {
    try {
        const dependencies = JSON.parse(el.getAttribute('data-dependencies'));
        
        dependencies.forEach((dependency) => {
            // Try to find the target GCDS component
            const targetWrapper = document.querySelector(`
                [input-id="${dependency.targetQuestionId}"],
                [select-id="${dependency.targetQuestionId}"],
                [textarea-id="${dependency.targetQuestionId}"],
                [radio-id="${dependency.targetQuestionId}"],
                [checkbox-id="${dependency.targetQuestionId}"]
            `);

            if (!targetWrapper) {
                console.warn(`Target element ${dependency.targetQuestionId} not found for dependency`, dependency);
                return;
            }

            const shouldApply = evaluateCondition(dependency, currentValue);
            applyDependencyAction(targetWrapper, dependency.action, shouldApply, dependency);
        });
    } catch (error) {
        console.error('Error handling dependencies:', error);
    }
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
                // For checkbox groups
                const checkboxGroup = document.querySelector(`gcds-checkbox-group[checkbox-id="${element.name}"]`);
                if (checkboxGroup) {
                    return Array.from(checkboxGroup.querySelectorAll('gcds-checkbox:checked'))
                        .map(cb => cb.value);
                }
                // Fallback to regular checkbox group
                const checkboxes = document.querySelectorAll(`gcds-checkbox[name="${element.name}"]:checked`);
                return Array.from(checkboxes).map(cb => cb.value);
            }
            return element.checked;
        case 'gcds-checkbox-group':
            return Array.from(element.querySelectorAll('gcds-checkbox:checked'))
                .map(cb => cb.value);
        case 'gcds-radio':
            const checkedRadio = document.querySelector(`gcds-radio[name="${element.name}"]:checked`);
            return checkedRadio ? checkedRadio.value : null;
        case 'gcds-radio-group':
            // For GCDS radio groups, the value is stored in the component itself
            return element.value;
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

    // Handle enum values (0 = Require, 1 = Show, 2 = Hide, etc.)
    switch (action) {
        case 0: // Require
            setRequired(element, shouldApply);
            break;
            
        case 1: // Show
            toggleVisibility(wrapper, shouldApply);
            break;
            
        case 2: // Hide
            toggleVisibility(wrapper, !shouldApply);
            break;
            
        case 3: // Enable
            toggleDisabled(element, !shouldApply);
            break;
            
        case 4: // Disable
            toggleDisabled(element, shouldApply);
            break;
            
        case 5: // ClearValue
            if (shouldApply) {
                clearElementValue(element);
            }
            break;
            
        case 6: // SetValue
            if (shouldApply && dependency.setValue) {
                setElementValue(element, dependency.setValue);
            }
            break;
            
        default:
            console.warn(`Unsupported action: ${action}`);
    }

    // Trigger change event for cascading dependencies
    if (shouldApply) {
        triggerChangeEvent(element);
    }
};

/**
 * Find the form group wrapper for an element
 */
const findFormGroupWrapper = (element) => {
    // Try to find the closest wrapper, fallback to element itself
    return element.closest('.gc-form-group') || 
           element.parentElement || 
           element;
};

/**
 * Toggle element visibility
 */
const toggleVisibility = (element, show) => {
    if (show) {
        element.style.removeProperty('display');
    } else {
        element.style.setProperty('display', 'none');
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
    if (!element || element.required === required) return;

    try {
        // For GCDS components, try to set the required attribute directly
        if (element.tagName.toLowerCase().startsWith('gcds-')) {
            element.required = required;
            element.setAttribute('required', required);
            return;
        }

        // For native elements, set both the property and attribute
        element.required = required;
        if (required) {
            element.setAttribute('required', '');
        } else {
            element.removeAttribute('required');
        }
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

/**
 * Get the current value of an element
 */
const getElementValue = (element) => {
    if (element.type === 'checkbox') {
        if (element.name) {
            // For checkbox groups, get all checked values
            const checkboxes = document.querySelectorAll(`input[name="${element.name}"]:checked`);
            return Array.from(checkboxes).map(cb => cb.value);
        }
        return element.checked;
    }
    if (element.type === 'radio') {
        const checkedRadio = document.querySelector(`input[name="${element.name}"]:checked`);
        return checkedRadio ? checkedRadio.value : null;
    }
    return element.value;
};

/**
 * Copy event listeners from one element to another
 */
const copyEventListeners = (oldElement, newElement) => {
    const listeners = getEventListeners(oldElement);
    listeners.forEach(({ type, listener, options }) => {
        newElement.addEventListener(type, listener, options);
    });
};

/**
 * Trigger a change event on an element
 */
const triggerChangeEvent = (element) => {
    // Trigger native change event
    const changeEvent = new Event('change', { bubbles: true });
    element.dispatchEvent(changeEvent);

    // Trigger GCDS change event
    const gcdsChangeEvent = new CustomEvent('gcdsChange', {
        bubbles: true,
        detail: { value: getElementValue(element) }
    });
    element.dispatchEvent(gcdsChangeEvent);
};

/**
 * Get all event listeners attached to an element
 */
const getEventListeners = (element) => {
    const listeners = [];
    const elementPrototype = Element.prototype;
    const addEventListenerOriginal = elementPrototype.addEventListener;

    // Override addEventListener to capture listeners
    elementPrototype.addEventListener = function (type, listener, options) {
        listeners.push({ type, listener, options });
        addEventListenerOriginal.call(this, type, listener, options);
    };

    // Clone the element to trigger its event bindings
    element.cloneNode(true);

    // Restore original addEventListener
    elementPrototype.addEventListener = addEventListenerOriginal;

    return listeners;
};


