class FDCPModal {
    constructor(element) {
        this.modal = element;
        this.isStatic = element.dataset.static === 'true';
        this.closeButton = element.querySelector('.fdcp-modal__close');
        this.backdrop = element.querySelector('.fdcp-modal__backdrop');
        
        this.bindEvents();
    }

    bindEvents() {
        if (this.closeButton) {
            this.closeButton.addEventListener('click', () => this.hide());
        }

        if (!this.isStatic) {
            this.backdrop.addEventListener('click', () => this.hide());
        }

        document.addEventListener('keydown', (e) => {
            if (e.key === 'Escape' && !this.isStatic && this.modal.classList.contains('show')) {
                this.hide();
            }
        });
    }

    show() {
        this.modal.classList.add('show');
        document.body.style.overflow = 'hidden';
    }

    hide() {
        this.modal.classList.remove('show');
        document.body.style.overflow = '';
    }
}

// Initialize all modals on the page
document.addEventListener('DOMContentLoaded', () => {
    document.querySelectorAll('.fdcp-modal').forEach(modal => {
        new FDCPModal(modal);
    });
});