document.addEventListener('DOMContentLoaded', () => {
    const btn = document.querySelector('[button-id=timeout-modal]');
    console.log("Button found:", btn);

    if (btn) {
        btn.addEventListener("click", (e) => {
            e.preventDefault();

            openGlobalModal({
                title: 'Session Expiring Soon',
                body: `<p>Your session is about to expire. Do you want to continue?</p>`,
                footer: `
                            <button type="button" class="btn btn-danger" data-bs-dismiss="modal">Logout</button>
                            <button type="button" class="btn btn-success" id="globalModalContinueSessionButton">Continue</button>
                        `
            });
        });
    } else {
        console.warn("Timeout modal button not found.");
    }
});