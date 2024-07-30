$(document).ready(function () {
    const roomDropdown = $('#roomDropdown');
    const shelfDropdown = $('#shelfDropdown');
    const locationFormsDiv = $('#locationForms');
    const addLocationBtn = $('.add-location-btn');
    let locationsViewModel = [];

    if (typeof existingLocations !== 'undefined') {
        locationsViewModel = existingLocations.map(location => ({
            roomId: location.roomId,
            shelfId: location.shelfId || '0',
            quantity: location.quantity
        }));

        $('.location-summary').remove();

        locationsViewModel.forEach((location, index) => {
            const locationSummary = createLocationSummary(location.roomId, location.shelfId || '0', location.quantity, index);
            locationFormsDiv.append(locationSummary);
        });
    }

    roomDropdown.change(function () {
        const roomId = $(this).val();
        shelfDropdown.html('<option value="">Shelf</option>');
        if (roomId && serializedShelves[roomId]) {
            serializedShelves[roomId].forEach(shelf => {
                shelfDropdown.append(new Option(shelf, shelf));
            });
        }
    });

    addLocationBtn.click(function () {
        const locationForm = $(this).closest('.post-form-wrapper');
        const roomId = locationForm.find('#roomDropdown').val().trim();
        const quantity = parseInt(locationForm.find('.quantity').val().trim(), 10);
        const shelfId = locationForm.find('#shelfDropdown').val().trim() || "0";

        if (roomId === '' || isNaN(quantity)) {
            alert('Please fill in all the fields with valid values.');
            return;
        }

        const existingLocationIndex = locationsViewModel.findIndex(location =>
            location.roomId === roomId && location.shelfId === shelfId
        );

        if (existingLocationIndex !== -1) {
            locationsViewModel[existingLocationIndex].quantity += quantity;
            updateLocationSummary(existingLocationIndex);
        } else {
            addNewLocation(roomId, shelfId, quantity);
        }

        locationForm.find('#roomDropdown').val('');
        locationForm.find('#shelfDropdown').html('<option value="">Shelf</option>');
        locationForm.find('.quantity').val('');
    });

    function addNewLocation(roomId, shelfId, quantity) {
        const locationSummary = createLocationSummary(roomId, shelfId, quantity, locationsViewModel.length);
        locationFormsDiv.append(locationSummary);
        locationsViewModel.push({ roomId, shelfId, quantity });
    }

    function createLocationSummary(roomId, shelfId, quantity, index) {
        const locationSummary = $('<div>').addClass('location-summary').attr('data-index', index);

        const $contentSpan = $('<span>');
        updateLocationSummaryContent($contentSpan, roomId, shelfId, quantity);
        locationSummary.append($contentSpan);

        const $removeButton = $('<button>').text("✖").click(function () {
            locationSummary.remove();
            removeLocationFromViewModel(roomId, shelfId);
        });
        locationSummary.append($removeButton);

        return locationSummary;
    }

    function updateLocationSummary(index) {
        const location = locationsViewModel[index];
        const locationSummary = locationFormsDiv.find(`.location-summary[data-index="${index}"]`);
        if (locationSummary.length) {
            const $contentSpan = locationSummary.find('span');
            updateLocationSummaryContent($contentSpan, location.roomId, location.shelfId, location.quantity);
        }
    }

    function updateLocationSummaryContent(e, roomId, shelfId, quantity) {
        e.text(`Room ID: ${roomId}, Shelf ID: ${shelfId || '0'}, Quantity: ${quantity}`);
    }

    function removeLocationFromViewModel(roomId, shelfId) {
        locationsViewModel = locationsViewModel.filter(location =>
            !(location.roomId === roomId && location.shelfId === shelfId)
        );

        $('.location-summary').each(function (index) {
            $(this).attr('data-index', index);
        });
    }

    const createBookForm = $('#createBookForm');
    const editBookForm = $('#editBookForm');

    if (createBookForm.length) {
        createBookForm.submit(handleFormSubmission);
    }

    if (editBookForm.length) {
        editBookForm.submit(handleFormSubmission);
    }

    function handleFormSubmission(event) {
        const form = $(this);
        locationsViewModel.forEach((location, index) => {
            form.append($('<input>', {
                type: 'hidden',
                name: `Locations[${index}].RoomId`,
                value: location.roomId
            }));

            form.append($('<input>', {
                type: 'hidden',
                name: `Locations[${index}].ShelfId`,
                value: location.shelfId || ''
            }));

            form.append($('<input>', {
                type: 'hidden',
                name: `Locations[${index}].Quantity`,
                value: location.quantity
            }));
        });
    }
});
