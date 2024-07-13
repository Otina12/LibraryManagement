document.addEventListener('DOMContentLoaded', function () {
    // for customer verification
    const customerIdInput = document.getElementById('customerIdInput');
    const verifyCustomerBtn = document.getElementById('verifyCustomer');
    const customerNameSpan = document.getElementById('customerName');
    const createCustomerLink = document.getElementById('createCustomerLink');
    const reservationFormContent = document.getElementById('reservationFormContent');

    // for reservation form
    const bookDropdown = document.getElementById('bookDropdown');
    const quantityInput = document.querySelector('.quantity');
    const returnDateInput = document.querySelector('.return-date');
    const addReservationBtn = document.querySelector('.add-reservation-btn');
    const selectedBooksContainer = document.getElementById('selectedBooks');
    const createReservationForm = document.getElementById('createReservationForm');

    let isCustomerVerified = false;

    $(bookDropdown).select2({
        placeholder: 'Search for a book',
        allowClear: true
    });

    function reinitializeSelect2() {
        $('#bookDropdown').select2('destroy').empty();

        booksData.forEach(book => {
            const option = new Option(`${book.title} (Available: ${book.quantity})`, book.id, false, false);
            $('#bookDropdown').append(option);
        });

        $('#bookDropdown').select2({
            placeholder: 'Search for a book',
            allowClear: true
        });
    }

    if (booksData && Array.isArray(booksData)) {
        booksData.forEach(book => {
            const option = bookDropdown.querySelector(`option[value="${book.id}"]`);
            if (option) {
                option.textContent = `${book.title} (Available: ${book.quantity})`;
            }
        });
    }

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
            alert('An error occurred while verifying the customer');
        }
    });

    createCustomerLink.addEventListener('click', function (e) {
        e.preventDefault();
        window.location.href = '/Customer/Create';
    });

    function updateBookQuantity(bookId, quantityToReduce) {
        const bookIndex = booksData.findIndex(book => book.id === bookId);
        if (bookIndex !== -1) {
            booksData[bookIndex].quantity -= quantityToReduce;
            updateBookDropdownOption(bookId);
        }
    }

    function updateBookDropdownOption(bookId) {
        const option = bookDropdown.querySelector(`option[value="${bookId}"]`);
        const book = booksData.find(book => book.id === bookId);
        if (option && book) {
            option.textContent = `${book.title} (Available: ${book.quantity})`;
        }
        $('#bookDropdown').trigger('change');
    }

    function checkQuantity(bookId, requestedQuantity) {
        const selectedBook = booksData.find(book => book.id === bookId);

        if (!selectedBook) {
            console.error('Selected book not found in booksData');
            return false;
        }

        if (requestedQuantity > selectedBook.quantity) {
            alert(`Error: Only ${selectedBook.quantity} copies of this book are available.`);
            return false;
        }
        return true;
    }

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

        if (!checkQuantity(bookId, quantity)) {
            return;
        }

        const bookTitle = bookDropdown.options[bookDropdown.selectedIndex].text;
        const bookElement = createBookElement(bookId, bookTitle, quantity, returnDate);
        selectedBooksContainer.appendChild(bookElement);

        updateBookQuantity(bookId, quantity); // reduce the quantity in dropdown and in array
        reinitializeSelect2(); // reinitialize select2 dropdown to change quantities

        bookDropdown.value = '';
        quantityInput.value = '';
        returnDateInput.value = '';
        $(bookDropdown).trigger('change');

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
            updateBookQuantity(bookId, -quantity); // add back to quantity
            reinitializeSelect2(); // reinitialize select2 dropdown to change quantities
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

    addReservationBtn.addEventListener('click', addBook);

    bookDropdown.addEventListener('change', function () {
        const selectedBookId = this.value;
        const selectedBook = booksData.find(book => book.id === selectedBookId);
        if (selectedBook) {
            quantityInput.max = selectedBook.quantity;
            quantityInput.value = '';
        }
    });

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
});