document.addEventListener("DOMContentLoaded", () => {
    const sessionModalElement = document.getElementById('session-extend-modal');

    if (!sessionModalElement) return;

    const bootstrapModal = new bootstrap.Modal(sessionModalElement);
    const timer = parseInt(sessionModalElement.dataset.sessionTimeout, 10);
    const reminder = parseInt(sessionModalElement.dataset.reminderTime, 10);
    const refreshUrl = sessionModalElement.dataset.refresh;
    const logoutUrl = sessionModalElement.dataset.logoutUrl;

    const countdownElement = document.getElementById('session-countdown');

    let reminderTimeout;
    let countdownInterval;
    let secondsRemaining;

    const startReminderTimer = () => {
        // Clear existing timers
        clearTimeout(reminderTimeout);
        clearInterval(countdownInterval);

        const reminderTimeInMs = (timer - reminder) * 60 * 1000;

        reminderTimeout = setTimeout(() => {
            const event = new CustomEvent("foundation:session-reminder", {
                detail: { remainingTime: reminder * 60 }
            });
            window.dispatchEvent(event);

            secondsRemaining = reminder * 60;
            bootstrapModal.show();
            startCountdown();
        }, reminderTimeInMs);
    };

    const startCountdown = () => {
        if (!countdownElement) return;

        countdownElement.textContent = `${secondsRemaining}s`;

        countdownInterval = setInterval(() => {
            secondsRemaining--;

            if (secondsRemaining <= 0) {
                clearInterval(countdownInterval);
                window.location.href = logoutUrl;
            } else {
                countdownElement.textContent = `${secondsRemaining}s`;
            }
        }, 1000);
    };

    const refreshBtn = document.querySelector('[button-id="session-extend-refresh-btn"]');
    const logoutBtn = document.querySelector('[button-id="session-extend-logout-btn"]');

    if (refreshBtn) {
        refreshBtn.addEventListener("click", () => {
            fetch(refreshUrl, {
                method: 'POST',
                credentials: 'include'
            }).then(() => {
                bootstrapModal.hide();
                startReminderTimer(); // Reset everything
            });
        });
    }

    if (logoutBtn) {
        logoutBtn.addEventListener("click", () => {
            window.location.href = logoutUrl;
        });
    }

    // Kick off the first reminder timer
    startReminderTimer();
});