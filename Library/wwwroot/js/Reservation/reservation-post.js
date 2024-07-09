//document.addEventListener('DOMContentLoaded', function () {
//    const customerIdInput = document.getElementById('customerIdInput');
//    const verifyCustomerBtn = document.getElementById('verifyCustomer');
//    const customerNameSpan = document.getElementById('customerName');
//    const createCustomerLink = document.getElementById('createCustomerLink');
//    const reservationFormContent = document.getElementById('reservationFormContent');
//    const bookReservationContainer = document.getElementById('bookReservationContainer');

//    let isCustomerVerified = false;

//    verifyCustomerBtn.addEventListener('click', async function () {
//        const customerId = customerIdInput.value;
//        if (!customerId) {
//            alert('Please enter a Customer ID');
//            return;
//        }

//        try {
//            const response = await fetch(`/Reservation/CustomerExists?Id=${customerId}`);
//            const data = await response.json();

//            if (data.success) {
//                customerNameSpan.textContent = `Customer found: ${data.customerDto.name}`;
//                createCustomerLink.style.display = 'none';
//                reservationFormContent.style.display = 'block';
//                isCustomerVerified = true;
//                document.getElementById('hiddenCustomerId').value = customerId;
//            } else {
//                customerNameSpan.textContent = 'Customer not found';
//                createCustomerLink.style.display = 'inline';
//                reservationFormContent.style.display = 'none';
//                isCustomerVerified = false;
//            }
//        } catch (error) {
//            console.error('Error:', error);
//            alert('An error occurred while verifying the customer');
//        }
//    });

//    createCustomerLink.addEventListener('click', function (e) {
//        e.preventDefault();
//        window.location.href = '/Customer/Create';
//    });

//    const bookDropdown = document.getElementById('bookDropdown');
//    const quantityInput = document.querySelector('.quantity');
//    const returnDateInput = document.querySelector('.return-date');
//    const addReservationBtn = document.querySelector('.add-reservation-btn');
//    const bookFormsDiv = document.getElementById('bookForms');
//    const createReservationForm = document.getElementById('createReservationForm');
//    let booksViewModel = [];

//    addReservationBtn.addEventListener('click', function () {
//        if (!isCustomerVerified) {
//            alert('Please verify the customer first.');
//            return;
//        }

//        const bookId = bookDropdown.value.trim();
//        const quantity = parseInt(quantityInput.value.trim(), 10);
//        const returnDate = returnDateInput.value.trim();

//        if (bookId === '' || isNaN(quantity) || quantity < 1 || returnDate === '') {
//            alert('Please select a book, enter a valid quantity, and select a return date.');
//            return;
//        }

//        addNewBook(bookId, quantity, returnDate);

//        bookDropdown.value = '';
//        $('#bookDropdown').trigger('change');
//        quantityInput.value = '';
//        returnDateInput.value = '';
//    });

//    function addNewBook(bookId, quantity, returnDate) {
//        const bookSummary = createBookSummary(bookId, quantity, returnDate, booksViewModel.length);
//        bookFormsDiv.appendChild(bookSummary);
//        booksViewModel.push({ bookId, quantity, supposedReturnDate: returnDate });
//        console.log('Book added to viewModel:', { bookId, quantity, supposedReturnDate: returnDate });
//        console.log('Current booksViewModel:', booksViewModel);
//    }

//    function createBookSummary(bookId, quantity, returnDate, index) {
//        const bookSummary = document.createElement('div');
//        bookSummary.classList.add('book-summary');
//        bookSummary.dataset.index = index;

//        const contentSpan = document.createElement('span');
//        updateBookSummaryContent(contentSpan, bookId, quantity, returnDate);
//        bookSummary.appendChild(contentSpan);

//        const removeButton = document.createElement('button');
//        removeButton.textContent = "✖";
//        removeButton.addEventListener('click', function () {
//            bookSummary.remove();
//            removeBookFromViewModel(index);
//        });
//        bookSummary.appendChild(removeButton);

//        return bookSummary;
//    }

//    function updateBookSummaryContent(element, bookId, quantity, returnDate) {
//        const bookTitle = bookDropdown.querySelector(`option[value="${bookId}"]`).textContent;
//        element.textContent = `Book: ${bookTitle}, Quantity: ${quantity}, Return Date: ${returnDate}`;
//    }

//    function removeBookFromViewModel(index) {
//        booksViewModel.splice(index, 1);
//        bookFormsDiv.querySelectorAll('.book-summary').forEach((summary, i) => {
//            summary.dataset.index = i;
//        });
//    }

//    if (createReservationForm) {
//        createReservationForm.addEventListener('submit', function (event) {
//            event.preventDefault();

//            if (!isCustomerVerified) {
//                alert('Please verify the customer before submitting the reservation.');
//                return;
//            }

//            console.log('Submitting form. Books in viewModel:', booksViewModel);

//            const existingHiddenInputs = this.querySelectorAll('input[type="hidden"][name^="Books["]');
//            existingHiddenInputs.forEach(input => input.remove());

//            booksViewModel.forEach((book, index) => {
//                ['BookId', 'Quantity', 'SupposedReturnDate'].forEach(prop => {
//                    const input = document.createElement('input');
//                    input.type = 'hidden';
//                    input.name = `Books[${index}].${prop}`;
//                    input.value = book[prop.charAt(0).toLowerCase() + prop.slice(1)];
//                    this.appendChild(input);
//                    console.log(`Created hidden input: ${input.name} = ${input.value}`);
//                });
//            });

//            const formData = new FormData(this);
//            for (let [key, value] of formData.entries()) {
//                console.log(`${key}: ${value}`);
//            }

//            this.submit();
//        });
//    }

//    $('#bookDropdown').select2({
//        placeholder: 'Search for a book',
//        allowClear: true,
//        templateResult: formatBook,
//        templateSelection: formatBookSelection
//    });
//});

//function formatBook(book) {
//    if (!book.id) return book.text;
//    return $("<div class='select2-result-book'><div class='select2-result-book__title'></div></div>")
//        .find(".select2-result-book__title").text(book.text).end();
//}

//function formatBookSelection(book) {
//    return book.text;
//}

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