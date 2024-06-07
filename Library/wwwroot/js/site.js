// function to display success/failure notifications on each page after some action
document.addEventListener('DOMContentLoaded', function () {
    console.log("Entered this")
    const body = document.body;
    const notificationSuccess = body.getAttribute('data-notification-success').toLowerCase() === 'true';
    const notificationMessage = body.getAttribute('data-notification-message');

    if (notificationMessage) {
        showNotification(notificationSuccess, notificationMessage);
    } else {
        console.log("no message")
    }
});

function showNotification(isSuccess, message) {
    console.log("Entered notification")
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
        }, 500);
    }, 2500);
}

function configureTinyMce() {
    tinymce.init({
        selector: '#tinyMceEditor',
        plugins: [
            'advlist', 'autolink', 'link', 'image', 'lists', 'charmap', 'preview', 'anchor', 'pagebreak',
            'searchreplace', 'wordcount', 'visualblocks', 'code', 'fullscreen', 'insertdatetime', 'media',
            'table', 'emoticons', 'help'
        ],
        toolbar: 'undo redo | styles | bold italic | alignleft aligncenter alignright alignjustify | ' +
            'bullist numlist outdent indent | link image | print preview media fullscreen | ' +
            'forecolor backcolor emoticons | help',
        resize: true,
    });
}


function dismissEmployee(employeeId) {
    if (confirm("Are you sure you want to dismiss this employee?")) {
        const form = document.createElement('form');
        form.method = 'POST';
        form.action = '@Url.Action("Delete", "Employee")';

        const input = document.createElement('input');
        input.type = 'hidden';
        input.name = 'id';
        input.value = employeeId;

        form.appendChild(input);
        document.body.appendChild(form);
        form.submit();
    }
}

function renewEmployee(employeeId) {
    if (confirm("Are you sure you want to renew this employee?")) {
        const form = document.createElement('form');
        form.method = 'POST';
        form.action = '@Url.Action("Renew", "Employee")';

        const input = document.createElement('input');
        input.type = 'hidden';
        input.name = 'id';
        input.value = employeeId;

        form.appendChild(input);
        document.body.appendChild(form);
        form.submit();
    }
}


function sortTableByStartDate(ascending, isOneParameter, cellIndex) {
    const table = document.querySelector('table tbody');
    const rows = Array.from(table.rows);

    rows.sort((a, b) => {
        const cellA = a.cells[cellIndex];
        const cellB = b.cells[cellIndex];

        if (!cellA || !cellB) {
            return 0;
        }

        const dateA = isOneParameter ? new Date(cellA.innerText) : new Date(cellA.innerText.split(' - ')[0]);
        const dateB = isOneParameter ? new Date(cellB.innerText) : new Date(cellB.innerText.split(' - ')[0]);

        return ascending ? dateA - dateB : dateB - dateA;
    });

    rows.forEach(row => table.appendChild(row));
}
