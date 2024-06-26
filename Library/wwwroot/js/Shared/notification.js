(function () {
    const body = document.body;
    const notificationSuccess = body.getAttribute('data-notification-success').toLowerCase() === 'true';
    const notificationMessage = body.getAttribute('data-notification-message');

    if (notificationMessage) {
        showNotification(notificationSuccess, notificationMessage);
    }
})();

function showNotification(isSuccess, message) {
    console.log("Entered notification");
    const notification = document.createElement('div');
    notification.className = 'notification';
    notification.classList.add(isSuccess ? 'success' : 'error');
    notification.textContent = message;
    document.body.appendChild(notification);
    setTimeout(() => {
        notification.classList.add('show');
    }, 10);
    setTimeout(() => {
        notification.classList.remove('show');
        setTimeout(() => {
            document.body.removeChild(notification);
            clearNotificationData();
        }, 500);
    }, 2500);
}

function clearNotificationData() {
    document.body.removeAttribute('data-notification-success');
    document.body.removeAttribute('data-notification-message');
}