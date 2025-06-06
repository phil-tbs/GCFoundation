document.addEventListener("DOMContentLoaded", () => {
    initializeValidation();
});

/**
 * Initialize validation for all form elements with validation rules
 */
const initializeValidation = () => {
    // Find all elements with validation rules
    document.querySelectorAll('[data-validation-rules]').forEach(element => {
        const rules = JSON.parse(element.getAttribute('data-validation-rules'));
        
        // Set up blur event handler if validate-on-blur is present
        if (element.hasAttribute('validate-on-blur')) {
            element.addEventListener('blur', () => validateElement(element));
        }

        // Set up form submit validation
        const form = element.closest('form');
        if (form) {
            if (!form.hasAttribute('validation-initialized')) {
                form.addEventListener('submit', (event) => validateForm(event));
                form.setAttribute('validation-initialized', 'true');
            }
        }
    });
};

/**
 * Validate a single form element
 */
const validateElement = (element) => {
    const rules = JSON.parse(element.getAttribute('data-validation-rules'));
    const value = getGCDSValue(element);
    const language = element.getAttribute('lang') || 'en';
    let isValid = true;
    let errorMessage = '';

    for (const rule of rules) {
        if (!validateRule(rule, value)) {
            isValid = false;
            errorMessage = rule.errorMessages[language.toLowerCase()] || 
                         rule.errorMessages['en'] || 
                         'Invalid value';
            break;
        }
    }

    updateElementValidation(element, isValid, errorMessage);
    return isValid;
};

/**
 * Validate all form elements on submit
 */
const validateForm = (event) => {
    const form = event.target;
    let isValid = true;

    form.querySelectorAll('[data-validation-rules]').forEach(element => {
        if (!validateElement(element)) {
            isValid = false;
        }
    });

    if (!isValid) {
        event.preventDefault();
        // Focus the first invalid element
        const firstInvalid = form.querySelector('.gcds-form-error');
        if (firstInvalid) {
            firstInvalid.focus();
        }
    }
};

/**
 * Validate a single rule against a value
 */
const validateRule = (rule, value) => {
    if (!value && rule.type !== 'required') return true;

    switch (rule.type) {
        case 'required':
            return value !== null && value !== undefined && value !== '';
        case 'regex':
            return new RegExp(rule.pattern).test(value);
        case 'email':
            return /^[^@\s]+@[^@\s]+\.[^@\s]+$/.test(value);
        case 'minlength':
            return value.length >= rule.min;
        case 'maxlength':
            return value.length <= rule.max;
        case 'minvalue':
            return parseFloat(value) >= rule.min;
        case 'maxvalue':
            return parseFloat(value) <= rule.max;
        default:
            return true;
    }
};

/**
 * Update element validation state and error message
 */
const updateElementValidation = (element, isValid, errorMessage) => {
    // Remove existing error state
    element.classList.remove('gcds-form-error');
    const existingError = element.nextElementSibling;
    if (existingError?.classList.contains('gcds-error-message')) {
        existingError.remove();
    }

    if (!isValid) {
        // Add error state
        element.classList.add('gcds-form-error');
        const errorElement = document.createElement('div');
        errorElement.className = 'gcds-error-message';
        errorElement.textContent = errorMessage;
        element.parentNode.insertBefore(errorElement, element.nextSibling);
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
            return element.checked;
        case 'gcds-radio':
            return element.checked ? element.value : null;
        default:
            return element.value;
    }
}; 