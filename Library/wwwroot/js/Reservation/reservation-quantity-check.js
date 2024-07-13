document.addEventListener('DOMContentLoaded', function () {
    console.log('DOM fully loaded and parsed');
    console.log('booksData:', booksData);

    const bookDropdown = document.getElementById('bookDropdown');
    const quantityInput = document.querySelector('.quantity');
    const addButton = document.querySelector('.add-reservation-btn');
    const form = document.getElementById('createReservationForm');

    console.log('bookDropdown:', bookDropdown);
    console.log('quantityInput:', quantityInput);
    console.log('addButton:', addButton);
    console.log('form:', form);

    if (booksData && Array.isArray(booksData)) {
        booksData.forEach(book => {
            const option = bookDropdown.querySelector(`option[value="${book.id}"]`);
            if (option) {
                option.textContent = `${book.title} (Available: ${book.quantity})`;
            }
        });
    } else {
        console.error('booksData is not an array or is undefined');
    }

    if ($ && $.fn.select2) {
        $(bookDropdown).select2();
    } else {
        console.error('Select2 is not available');
    }

    function checkQuantity() {
        const selectedBookId = bookDropdown.value;
        console.log('Selected Book ID:', selectedBookId);

        if (!selectedBookId) {
            console.error('No book selected');
            return false;
        }

        const selectedBook = booksData.find(book => book.id === selectedBookId);
        console.log('Selected Book:', selectedBook);

        if (!selectedBook) {
            console.error('Selected book not found in booksData');
            return false;
        }

        const requestedQuantity = parseInt(quantityInput.value, 10);
        console.log('Requested Quantity:', requestedQuantity);
        console.log('Available Quantity:', selectedBook.quantity);

        if (isNaN(requestedQuantity)) {
            alert('Please enter a valid quantity');
            return false;
        }

        if (requestedQuantity > selectedBook.quantity) {
            alert(`Error: Only ${selectedBook.quantity} copies of this book are available.`);
            return false;
        }
        return true;
    }

    addButton.addEventListener('click', function (e) {
        console.log('Add button clicked');
        if (!checkQuantity()) {
            e.preventDefault();
        }
    });

    form.addEventListener('submit', function (e) {
        console.log('Form submitted');
        if (!checkQuantity()) {
            e.preventDefault();
        }
    });

    bookDropdown.addEventListener('change', function () {
        console.log('Book dropdown changed');
        const selectedBookId = this.value;
        const selectedBook = booksData.find(book => book.id === selectedBookId);
        if (selectedBook) {
            quantityInput.max = selectedBook.quantity;
            quantityInput.value = '';
        }
    });
});