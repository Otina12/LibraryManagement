$(document).ready(function () {
    function initializeSelect2(selector, options) {
        $(selector).select2(options);
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
            initializeModal('author', '/Author/Create', '/Author/Edit/', '/Author/Delete/');
            loadModalContent('/Author/Create', 'Add Author', 'create');
        });
    }

    initializeSelect2('.select2-authors', getAuthorsOptions());
    initializeSelect2('.select2-genres', getGenresOptions());

    handleAddAuthor();
});