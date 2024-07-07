document.addEventListener('DOMContentLoaded', function () {
    const customerIdInput = document.getElementById('customerIdInput');
    const verifyCustomerBtn = document.getElementById('verifyCustomer');
    const customerNameSpan = document.getElementById('customerName');
    const createCustomerLink = document.getElementById('createCustomerLink');
    const reservationFormContent = document.getElementById('reservationFormContent');
    const bookReservationContainer = document.getElementById('bookReservationContainer');

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
    const bookFormsDiv = document.getElementById('bookForms');
    const createReservationForm = document.getElementById('createReservationForm');
    let booksViewModel = [];

    addReservationBtn.addEventListener('click', function () {
        if (!isCustomerVerified) {
            alert('Please verify the customer first.');
            return;
        }

        const bookId = bookDropdown.value.trim();
        const quantity = parseInt(quantityInput.value.trim(), 10);
        const returnDate = returnDateInput.value.trim();

        if (bookId === '' || isNaN(quantity) || quantity < 1 || returnDate === '') {
            alert('Please select a book, enter a valid quantity, and select a return date.');
            return;
        }

        addNewBook(bookId, quantity, returnDate);

        bookDropdown.value = '';
        $('#bookDropdown').trigger('change');
        quantityInput.value = '';
        returnDateInput.value = '';
    });

    function addNewBook(bookId, quantity, returnDate) {
        const bookSummary = createBookSummary(bookId, quantity, returnDate, booksViewModel.length);
        bookFormsDiv.appendChild(bookSummary);
        booksViewModel.push({ bookId, quantity, supposedReturnDate: returnDate });
    }

    function createBookSummary(bookId, quantity, returnDate, index) {
        const bookSummary = document.createElement('div');
        bookSummary.classList.add('book-summary');
        bookSummary.dataset.index = index;

        const contentSpan = document.createElement('span');
        updateBookSummaryContent(contentSpan, bookId, quantity, returnDate);
        bookSummary.appendChild(contentSpan);

        const removeButton = document.createElement('button');
        removeButton.textContent = "✖";
        removeButton.addEventListener('click', function () {
            bookSummary.remove();
            removeBookFromViewModel(index);
        });
        bookSummary.appendChild(removeButton);

        return bookSummary;
    }

    function updateBookSummaryContent(element, bookId, quantity, returnDate) {
        const bookTitle = bookDropdown.querySelector(`option[value="${bookId}"]`).textContent;
        element.textContent = `Book: ${bookTitle}, Quantity: ${quantity}, Return Date: ${returnDate}`;
    }

    function removeBookFromViewModel(index) {
        booksViewModel.splice(index, 1);
        bookFormsDiv.querySelectorAll('.book-summary').forEach((summary, i) => {
            summary.dataset.index = i;
        });
    }

    if (createReservationForm) {
        createReservationForm.addEventListener('submit', function (event) {
            if (!isCustomerVerified) {
                event.preventDefault();
                alert('Please verify the customer before submitting the reservation.');
                return;
            }

            booksViewModel.forEach((book, index) => {
                ['BookId', 'Quantity', 'SupposedReturnDate'].forEach(prop => {
                    const input = document.createElement('input');
                    input.type = 'hidden';
                    input.name = `Books[${index}].${prop}`;
                    input.value = book[prop.toLowerCase()];
                    this.appendChild(input);
                });
            });
        });
    }

    $('#bookDropdown').select2({
        placeholder: 'Search for a book',
        allowClear: true,
        templateResult: formatBook,
        templateSelection: formatBookSelection
    });
});

function formatBook(book) {
    if (!book.id) return book.text;
    return $("<div class='select2-result-book'><div class='select2-result-book__title'></div></div>")
        .find(".select2-result-book__title").text(book.text).end();
}

function formatBookSelection(book) {
    return book.text;
}
