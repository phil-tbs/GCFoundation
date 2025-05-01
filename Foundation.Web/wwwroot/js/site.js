document.addEventListener('DOMContentLoaded', () => {
    const btn = document.querySelector('[button-id=timeout-modal]');
    console.log("Button found:", btn);

    if (btn) {
        btn.addEventListener("click", (e) => {
            e.preventDefault();

            const modalElement = document.getElementById('timeout-modal');

            const modal = new bootstrap.Modal(modalElement);
            modal.show();

        });
    } else {
        console.warn("Timeout modal button not found.");
    }
});