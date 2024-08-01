$(document).ready(function () {
    function initializeSelect2(selector, options) {
        $(selector).select2(options);
        console.log(selector);
    }

    function getAuthorsOptions() {
        return {
            placeholder: 'Select authors',
            multiple: true,
            allowClear: true,
            language: {
                noResults: function () {
                    return 'No results found. <a href="/Author/Create" class="dropdown-add-author">Add an author</a>';
                }
            },
            escapeMarkup: function (markup) {
                return markup;
            }
        };
    }

    function getGenresOptions() {
        return {
            placeholder: 'Select genres',
            multiple: true,
            allowClear: true,
            width: '100%'
        };
    }

    function handleAddAuthor() {
        $(document).on('click', '.dropdown-add-author', function (e) {
            e.preventDefault();
            e.stopPropagation();
            initializeModal('author', '/Author/Create/', '/Author/Edit/', '/Author/Delete/', '/Author/Renew/');
            loadModalContent('/Author/Create', 'Add Author', 'create');
        });
    }

    function initializeShelfDropdown() {
        const shelfDropdown = $('#shelfDropdown');
        $(document).on('change', '#roomDropdown', function () {
            const roomId = $(this).val();
            shelfDropdown.html('<option value="">Shelf</option>');
            if (roomId && serializedShelves[roomId]) {
                serializedShelves[roomId].forEach(shelf => {
                    shelfDropdown.append(new Option(shelf, shelf));
                });
            }
        });
    }

    function initializeBookDropdowns() {
        const originalBookDropdown = $('#originalBookDropdown');
        const bookEditionDropdown = $('#bookEditionDropdown');

        originalBookDropdown.select2({
            placeholder: 'Search for an original book',
            allowClear: true
        });

        bookEditionDropdown.select2({
            placeholder: 'Select a book edition',
            allowClear: true
        });

        originalBookDropdown.on('select2:select change', updateBookEditionDropdown);
        bookEditionDropdown.on('change', updateQuantityInput);
    }

    function updateBookEditionDropdown() {
        const originalBookDropdown = $('#originalBookDropdown');
        const bookEditionDropdown = $('#bookEditionDropdown');
        const selectedOriginalBookId = originalBookDropdown.val();
        bookEditionDropdown.empty().prop('disabled', true);

        if (selectedOriginalBookId && bookEditions[selectedOriginalBookId]) {
            const editions = bookEditions[selectedOriginalBookId].Editions;
            bookEditionDropdown.append(new Option('Select a book edition', '', false, false));
            editions.forEach(edition => {
                const option = new Option(`${edition.ISBN} - ${edition.PublisherName} (Edition: ${edition.Edition}, Available: ${edition.Quantity})`, edition.Id, false, false);
                bookEditionDropdown.append(option);
            });
            bookEditionDropdown.prop('disabled', false).trigger('change.select2');
        } else {
            console.log('Invalid original book selection or no editions available');
        }
    }

    function updateQuantityInput() {
        const bookEditionDropdown = $('#bookEditionDropdown');
        const originalBookDropdown = $('#originalBookDropdown');
        const quantityInput = $('.quantity');
        const selectedEditionId = bookEditionDropdown.val();
        const selectedOriginalBookId = originalBookDropdown.val();
        const selectedEdition = bookEditions[selectedOriginalBookId]?.Editions.find(edition => edition.Id === selectedEditionId);

        if (selectedEdition) {
            quantityInput.prop('max', selectedEdition.Quantity).attr('data-max-quantity', selectedEdition.Quantity).attr('placeholder', `Quantity (Max: ${selectedEdition.Quantity})`);
        } else {
            quantityInput.prop('max', '').attr('data-max-quantity', '').attr('placeholder', 'Quantity');
        }
        quantityInput.val('');
    }

    initializeSelect2('.select2-authors', getAuthorsOptions());
    initializeSelect2('.select2-genres', getGenresOptions());
    handleAddAuthor();
    initializeShelfDropdown();
    initializeBookDropdowns();
});