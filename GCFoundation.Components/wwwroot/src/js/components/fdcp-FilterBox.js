document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('[data-fdcp-collapse-toggle]').forEach(button => {
        button.addEventListener('click', function () {
            const targetId = this.getAttribute('data-fdcp-collapse-toggle');
            const target = document.getElementById(targetId);

            const isExpanded = this.getAttribute('aria-expanded') === 'true';
            this.setAttribute('aria-expanded', (!isExpanded).toString());

            if (target) {
                target.classList.toggle('fdcp-show', !isExpanded);
            }
        });
    });
});