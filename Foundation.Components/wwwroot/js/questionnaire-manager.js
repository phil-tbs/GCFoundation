window.questionnaireManager = (function () {
    let config = {};
    let form = {};

    function init(options) {
        config = options;
        bindEvents();
        initSortable();
    }

    function bindEvents() {
        // Form level events
        $('.save-form').on('click', saveForm);
        $('.export-json').on('click', exportJson);
        $('.publish-form').on('click', publishForm);

        // Section level events
        $('.add-section').on('click', addSection);
        $(document).on('click', '.delete-section', deleteSection);
        $(document).on('input', '.section-title, .section-description', updateSection);
        $(document).on('change', '.section-visible', updateSectionVisibility);

        // Question level events
        $(document).on('click', '.add-question', addQuestion);
        $(document).on('click', '.delete-question', deleteQuestion);
        $(document).on('input', '.question-text, .question-description', updateQuestion);
        $(document).on('change', '.question-type', updateQuestionType);
        $(document).on('change', '.question-required', updateQuestionRequired);
        $(document).on('input', '.question-placeholder, .question-help', updateQuestion);
        $(document).on('change', '.question-visible', updateQuestionVisibility);

        // Options events
        $(document).on('click', '.add-option', addOption);
        $(document).on('click', '.delete-option', deleteOption);
        $(document).on('input', '.option-label, .option-value', updateOption);

        // Validation events
        $(document).on('click', '.add-validation', addValidation);
        $(document).on('click', '.delete-validation', deleteValidation);
        $(document).on('change', '.validation-type, .validation-value', updateValidation);

        // Dependencies events
        $(document).on('click', '.add-dependency', addDependency);
        $(document).on('click', '.delete-dependency', deleteDependency);
        $(document).on('change', '.dependency-source, .dependency-operator', updateDependency);
        $(document).on('click', '.save-dependencies', saveDependencies);
    }

    function initSortable() {
        // Make sections sortable
        new Sortable($('.sections-container')[0], {
            handle: '.move-section',
            animation: 150,
            onEnd: updateSectionOrder
        });

        // Make questions sortable within sections
        $('.questions-container').each(function() {
            new Sortable(this, {
                handle: '.move-question',
                animation: 150,
                onEnd: updateQuestionOrder
            });
        });
    }

    function getFormData() {
        const form = {
            id: config.formId,
            title: $('#formTitle').val(),
            description: $('#formDescription').val(),
            sections: []
        };

        $('.form-section').each(function() {
            const section = {
                id: $(this).data('section-id'),
                title: $(this).find('.section-title').val(),
                description: $(this).find('.section-description').val(),
                order: $(this).index(),
                isVisible: $(this).find('.section-visible').prop('checked'),
                questions: [],
                visibilityDependencies: []
            };

            $(this).find('.form-question').each(function() {
                const question = {
                    id: $(this).data('question-id'),
                    text: $(this).find('.question-text').val(),
                    description: $(this).find('.question-description').val(),
                    type: $(this).find('.question-type').val(),
                    isRequired: $(this).find('.question-required').prop('checked'),
                    order: $(this).index(),
                    placeholder: $(this).find('.question-placeholder').val(),
                    helpText: $(this).find('.question-help').val(),
                    isVisible: $(this).find('.question-visible').prop('checked'),
                    options: [],
                    validationRules: [],
                    visibilityDependencies: []
                };

                $(this).find('.question-option').each(function() {
                    question.options.push({
                        id: $(this).data('option-id'),
                        label: $(this).find('.option-label').val(),
                        value: $(this).find('.option-value').val(),
                        order: $(this).index()
                    });
                });

                $(this).find('.validation-rule').each(function() {
                    question.validationRules.push({
                        type: $(this).find('.validation-type').val(),
                        value: $(this).find('.validation-value').val(),
                        message: $(this).find('.validation-message').val()
                    });
                });

                section.questions.push(question);
            });

            form.sections.push(section);
        });

        return form;
    }

    async function saveForm() {
        try {
            const formData = getFormData();
            const response = await fetch('/api/forms', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(formData)
            });

            if (!response.ok) {
                throw new Error('Failed to save form');
            }

            toastr.success(config.resources.saveSuccessMessage);
        } catch (error) {
            console.error('Error saving form:', error);
            toastr.error(config.resources.errorMessage);
        }
    }

    function exportJson() {
        const formData = getFormData();
        const jsonString = JSON.stringify(formData, null, 2);
        const blob = new Blob([jsonString], { type: 'application/json' });
        const url = URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `form-${formData.id}.json`;
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        URL.revokeObjectURL(url);
    }

    async function publishForm() {
        try {
            const response = await fetch(`/api/forms/${config.formId}/publish`, {
                method: 'POST'
            });

            if (!response.ok) {
                throw new Error('Failed to publish form');
            }

            toastr.success(config.resources.publishSuccessMessage);
        } catch (error) {
            console.error('Error publishing form:', error);
            toastr.error(config.resources.errorMessage);
        }
    }

    // Helper functions for generating unique IDs
    function generateId() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
            const r = Math.random() * 16 | 0;
            const v = c === 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    }

    return {
        init
    };
})(); 