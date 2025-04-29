
// Open the global modal with custom title/body/footer
    function openGlobalModal(options) {
        const modalTitle = document.querySelector('#globalModal .modal-title');
        const modalBody = document.querySelector('#globalModalBody');
        const modalFooter = document.querySelector('#globalModalFooter');

        if (modalTitle) modalTitle.innerHTML = options.title || '';
        if (modalBody) modalBody.innerHTML = options.body || '';
        if (modalFooter) modalFooter.innerHTML = options.footer || '';

        const modalElement = document.getElementById('globalModal');
        if (modalElement) {
            const modal = new bootstrap.Modal(modalElement);
            modal.show();
        } else {
            console.error("Modal element not found");
        }
    }

    // Example helper to open a confirmation modal
    function openConfirmModal(message, confirmCallback) {
        openGlobalModal({
            title: 'Confirm Action',
            body: `<p>${message}</p>`,
            footer: `
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                <button type="button" class="btn btn-primary" id="globalModalConfirmButton">Confirm</button>
            `
        });

        // Attach confirm callback after rendering
        setTimeout(() => {
        document.getElementById('globalModalConfirmButton').onclick = function () {
            confirmCallback();
            bootstrap.Modal.getInstance(document.getElementById('globalModal')).hide();
        };
        }, 100); 
    }

    // Example for session timeout warning
    function openSessionTimeoutWarning(timeoutCallback) {
        openGlobalModal({
            title: 'Session Expiring Soon',
            body: `<p>Your session is about to expire. Do you want to continue?</p>`,
            footer: `
                <button type="button" class="btn btn-danger" data-bs-dismiss="modal">Logout</button>
                <button type="button" class="btn btn-success" id="globalModalContinueSessionButton">Continue</button>
            `
        });

        setTimeout(() => {
        document.getElementById('globalModalContinueSessionButton').onclick = function () {
            timeoutCallback();
            bootstrap.Modal.getInstance(document.getElementById('globalModal')).hide();
        };
        }, 100);
    }

