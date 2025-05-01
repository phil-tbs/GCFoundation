document.addEventListener("DOMContentLoaded", () => {
    const sessionModalElement = document.getElementById('session-extend-modal');

    if (sessionModalElement) {
        const bootstrapModal = new bootstrap.Modal(sessionModalElement);
        const timer = parseInt(sessionModalElement.dataset.sessionTimeout, 10);
        const reminder = parseInt(sessionModalElement.dataset.reminderTime, 10);

        const countdownElement = document.getElementById('session-countdown');
        const reminderTimeInMs = (timer - reminder) * 60 * 1000;

        let countdownInterval;
        let secondsRemaining = reminder * 60;

        const startCountdown = () => {
            if (!countdownElement) return;

            countdownElement.textContent = `${secondsRemaining}s`;

            countdownInterval = setInterval(() => {
                secondsRemaining--;

                if (secondsRemaining <= 0) {
                    clearInterval(countdownInterval);
                    // Automatically log out if time runs out
                    window.location.href = sessionModalElement.dataset.logoutUrl;
                } else {
                    countdownElement.textContent = `${secondsRemaining}s`;
                }
            }, 1000);
        };

        setTimeout(() => {
            const event = new CustomEvent("foundation:session-reminder", {
                detail: { remainingTime: reminder * 60 }
            });
            window.dispatchEvent(event);

            bootstrapModal.show();
            startCountdown();
        }, reminderTimeInMs);

        const refreshBtn = document.querySelector('[button-id="session-extend-refresh-btn"]');
        const logoutBtn = document.getElementById('[button-id="session-extend-logout-btn"]');

        if (refreshBtn) {
            refreshBtn.addEventListener("click", () => {
                console.log("click");
                // Send AJAX call to extend session
                fetch(sessionModalElement.dataset.refresh, {
                    method: 'POST',
                    credentials: 'include'
                }).then(() => {
                    bootstrapModal.hide();
                });
            });
        }

        if (logoutBtn) {
            logoutBtn.addEventListener("click", () => {
                window.location.href = sessionModalElement.dataset.logoutUrl;
            });
        }
    }
});