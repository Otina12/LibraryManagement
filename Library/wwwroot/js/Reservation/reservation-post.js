document.addEventListener('DOMContentLoaded', function () {
    const customerIdInput = document.getElementById('customerIdInput');
    const verifyCustomerBtn = document.getElementById('verifyCustomer');
    const customerNameSpan = document.getElementById('customerName');
    const createCustomerLink = document.getElementById('createCustomerLink');
    const reservationFormContent = document.getElementById('reservationFormContent');

    let isCustomerVerified = false;

    verifyCustomerBtn.addEventListener('click', async function () {
        const customerId = customerIdInput.value;
        if (!customerId) {
            alert('Please enter a Customer ID');
            return;
        }

        try {
            const response = await fetch(`/Reservation/CustomerExists?Id=${customerId}`);
            const data = await response.json();

            if (data.success) {
                customerNameSpan.textContent = `Customer found: ${data.customerDto.name}`;
                createCustomerLink.style.display = 'none';
                reservationFormContent.style.display = 'block';
                isCustomerVerified = true;
                document.getElementById('hiddenCustomerId').value = customerId;
            } else {
                customerNameSpan.textContent = 'Customer not found';
                createCustomerLink.style.display = 'inline';
                reservationFormContent.style.display = 'none';
                isCustomerVerified = false;
            }
        } catch (error) {
            console.error('Error:', error);
            alert('An error occurred while verifying the customer');
        }
    });

    createCustomerLink.addEventListener('click', function (e) {
        e.preventDefault();
        window.location.href = '/Customer/Create';
    });

    const bookDropdown = document.getElementById('bookDropdown');
    const quantityInput = document.querySelector('.quantity');
    const returnDateInput = document.querySelector('.return-date');
    const addReservationBtn = document.querySelector('.add-reservation-btn');
    const selectedBooksContainer = document.getElementById('selectedBooks');
    const createReservationForm = document.getElementById('createReservationForm');

    addReservationBtn.addEventListener('click', addBook);

    function addBook() {
        if (!isCustomerVerified) {
            alert('Please verify the customer first.');
            return;
        }

        const bookId = bookDropdown.value;
        const quantity = parseInt(quantityInput.value, 10);
        const returnDate = returnDateInput.value;

        if (!bookId || isNaN(quantity) || quantity < 1 || !returnDate) {
            alert('Please select a book, enter a valid quantity, and select a return date.');
            return;
        }

        const bookTitle = bookDropdown.options[bookDropdown.selectedIndex].text;
        const bookElement = createBookElement(bookId, bookTitle, quantity, returnDate);
        selectedBooksContainer.appendChild(bookElement);

        bookDropdown.value = '';
        quantityInput.value = '';
        returnDateInput.value = '';
        $('#bookDropdown').trigger('change');

        updateBookIndices();
    }

    function createBookElement(bookId, title, quantity, returnDate) {
        const div = document.createElement('div');
        div.className = 'book-summary';
        div.innerHTML = `
        <span>${title} - Quantity: ${quantity}, Return Date: ${returnDate}</span>
        <button type="button" class="remove-book">✖</button>
        <input type="hidden" name="Books[0].BookId" value="${bookId}">
        <input type="hidden" name="Books[0].Quantity" value="${quantity}">
        <input type="hidden" name="Books[0].SupposedReturnDate" value="${returnDate}">
    `;
        div.querySelector('.remove-book').addEventListener('click', function () {
            div.remove();
            updateBookIndices();
        });
        return div;
    }

    function updateBookIndices() {
        const books = selectedBooksContainer.querySelectorAll('.book-summary');
        books.forEach((book, index) => {
            book.querySelectorAll('input[type="hidden"]').forEach(input => {
                const name = input.getAttribute('name');
                const newName = name.replace(/Books\[\d+\]/, `Books[${index}]`);
                input.setAttribute('name', newName);
            });
        });
    }

    createReservationForm.addEventListener('submit', function (event) {
        if (!isCustomerVerified) {
            alert('Please verify the customer before submitting the reservation.');
            event.preventDefault();
            return;
        }

        const selectedBooks = selectedBooksContainer.querySelectorAll('.book-summary');
        if (selectedBooks.length === 0) {
            alert('Please add at least one book to the reservation.');
            event.preventDefault();
            return;
        }

        updateBookIndices();
    });

    $('#bookDropdown').select2({
        placeholder: 'Search for a book',
        allowClear: true
    });
});