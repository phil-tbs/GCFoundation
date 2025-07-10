document.addEventListener('DOMContentLoaded', () => {
    const btn = document.querySelector('[button-id=timeout-modal]');
    console.log("Button found:", btn);

    if (btn) {
        btn.addEventListener("click", (e) => {
            e.preventDefault();

            const modalElement = document.getElementById('timeout-modal');
            if (modalElement && modalElement.FDCPModalInstance) {
                modalElement.FDCPModalInstance.show();
            } else if (modalElement) {
                // Fallback: create instance if not already created
                modalElement.FDCPModalInstance = new FDCPModal(modalElement);
                modalElement.FDCPModalInstance.show();
            } else {
                console.warn("Timeout modal element not found.");
            }
        });
    } else {
        console.warn("Timeout modal button not found.");
    }
});