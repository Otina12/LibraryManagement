$(document).ready(function () {
    const customerIdInput = $('#customerIdInput');
    const verifyCustomerBtn = $('#verifyCustomer');
    const customerNameSpan = $('#customerName');
    const createCustomerLink = $('#createCustomerLink');

    const reservationFormContent = $('#reservationFormContent');
    const originalBookDropdown = $('#originalBookDropdown');
    const bookEditionDropdown = $('#bookEditionDropdown');
    const quantityInput = $('.quantity');
    const returnDateInput = $('.return-date');
    const addReservationBtn = $('.add-reservation-btn');
    const selectedBooksContainer = $('#selectedBooks');
    const createReservationForm = $('#createReservationForm');
    let isCustomerVerified = false;

    function initializeDropdowns() {
        originalBookDropdown.select2({
            placeholder: 'Search for an original book',
            allowClear: true
        });
        bookEditionDropdown.select2({
            placeholder: 'Select a book edition',
            allowClear: true
        });
    }

    function attachEventListeners() {
        originalBookDropdown.on('select2:select change', updateBookEditionDropdown);
        bookEditionDropdown.on('change', updateQuantityInput);
        addReservationBtn.on('click', addBook);
        createReservationForm.on('submit', validateForm);
    }

    function updateBookEditionDropdown() {
        const selectedOriginalBookId = originalBookDropdown.val();
        bookEditionDropdown.empty().prop('disabled', true);

        if (selectedOriginalBookId && bookEditions[selectedOriginalBookId]) {
            const editions = bookEditions[selectedOriginalBookId].Editions;
            bookEditionDropdown.append(new Option('Select a book edition', '', false, false));
            editions.forEach(edition => {
                const option = new Option(`${edition.ISBN} - ${edition.PublisherName} (Edition: ${edition.Edition}, Available: ${edition.AvailableQuantity})`, edition.Id, false, false);
                bookEditionDropdown.append(option);
            });
            bookEditionDropdown.prop('disabled', false).trigger('change.select2');
        } else {
            console.log('Invalid original book selection or no editions available');
        }
    }

    function updateQuantityInput() {
        const selectedEditionId = bookEditionDropdown.val();
        const selectedOriginalBookId = originalBookDropdown.val();
        const selectedEdition = bookEditions[selectedOriginalBookId]?.Editions.find(edition => edition.Id === selectedEditionId);

        if (selectedEdition) {
            quantityInput.prop('max', selectedEdition.AvailableQuantity).attr('data-max-quantity', selectedEdition.AvailableQuantity).attr('placeholder', `Quantity (Max: ${selectedEdition.AvailableQuantity})`);
        } else {
            quantityInput.prop('max', '').attr('data-max-quantity', '').attr('placeholder', 'Quantity');
        }
        quantityInput.val('');
    }

    function addBook() {
        if (!isCustomerVerified) {
            alert('Please verify the customer first.');
            return;
        }

        const originalBookId = originalBookDropdown.val();
        const bookId = bookEditionDropdown.val();
        const quantity = parseInt(quantityInput.val(), 10);
        const returnDate = returnDateInput.val();

        if (!originalBookId || !bookId || isNaN(quantity) || quantity < 1 || !returnDate) {
            alert('Please fill in all fields correctly.');
            return;
        }

        if (!checkQuantity(originalBookId, bookId, quantity)) return;

        const originalBookTitle = bookEditions[originalBookId].Title;
        const bookEditionText = bookEditionDropdown.find('option:selected').text();
        const bookElement = createBookElement(originalBookId, bookId, `${originalBookTitle} - ${bookEditionText}`, quantity, returnDate);
        selectedBooksContainer.append(bookElement);

        updateBookQuantity(originalBookId, bookId, quantity);
        resetForm();
        updateBookIndices();
    }

    function createBookElement(originalBookId, bookId, title, quantity, returnDate) {
        const bookElement = $(`
            <div class="book-summary">
                <span>${title} - Quantity: ${quantity}, Return Date: ${returnDate}</span>
                <button type="button" class="remove-book">✖</button>
                <input type="hidden" name="Books[0].OriginalBookId" value="${originalBookId}">
                <input type="hidden" name="Books[0].BookId" value="${bookId}">
                <input type="hidden" name="Books[0].Quantity" value="${quantity}">
                <input type="hidden" name="Books[0].SupposedReturnDate" value="${returnDate}">
            </div>
        `);
        bookElement.find('.remove-book').on('click', function () {
            updateBookQuantity(originalBookId, bookId, -quantity);
            bookElement.remove();
            updateBookIndices();
        });
        return bookElement;
    }

    function checkQuantity(originalBookId, bookId, requestedQuantity) {
        const selectedEdition = bookEditions[originalBookId].Editions.find(edition => edition.Id === bookId);
        if (!selectedEdition || requestedQuantity > selectedEdition.AvailableQuantity) {
            alert(`Error: Only ${selectedEdition ? selectedEdition.AvailableQuantity : 0} copies of this book edition are available.`);
            return false;
        }
        return true;
    }

    function updateBookQuantity(originalBookId, bookId, quantityToReduce) {
        const edition = bookEditions[originalBookId].Editions.find(edition => edition.Id === bookId);
        if (edition) {
            edition.AvailableQuantity -= quantityToReduce;
            updateBookEditionDropdown();
        }
    }

    function resetForm() {
        originalBookDropdown.val(null).trigger('change');
        bookEditionDropdown.val(null).trigger('change');
        quantityInput.val('');
        returnDateInput.val('');
    }

    function updateBookIndices() {
        selectedBooksContainer.find('.book-summary').each((index, book) => {
            $(book).find('input[type="hidden"]').each(function () {
                const name = $(this).attr('name');
                $(this).attr('name', name.replace(/Books\[\d+\]/, `Books[${index}]`));
            });
        });
    }

    function validateForm(event) {
        if (!isCustomerVerified) {
            event.preventDefault();
            alert('Please verify the customer first.');
        }
    }

    verifyCustomerBtn.on('click', async function () {
        const customerId = customerIdInput.val();
        if (!customerId) {
            alert('Please enter a Customer ID');
            return;
        }
        try {
            const response = await fetch(`/Reservation/CustomerExists?Id=${customerId}`);
            const data = await response.json();
            if (data.success) {
                customerNameSpan.text(data.customerDto.name).addClass('verified');
                createCustomerLink.hide();
                customerIdInput.prop('disabled', true);
                isCustomerVerified = true;
                reservationFormContent.show();
                $('#hiddenCustomerId').val(customerId);
                initializeDropdowns();
            } else {
                customerNameSpan.text('Customer not found').removeClass('verified');
                createCustomerLink.show();
                customerIdInput.prop('disabled', false);
                isCustomerVerified = false;
                reservationFormContent.hide();
            }
        } catch (error) {
            alert('An error occurred while verifying the customer');
        }
    });

    createCustomerLink.on('click', function (e) {
        e.preventDefault();
        window.location.href = '/Customer/Create';
    });

    initializeDropdowns();
    attachEventListeners();
});