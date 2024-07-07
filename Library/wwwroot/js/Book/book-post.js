document.addEventListener('DOMContentLoaded', function () {
    const roomDropdown = document.getElementById('roomDropdown');
    const shelfDropdown = document.getElementById('shelfDropdown');
    const locationFormsDiv = document.getElementById('locationForms');
    const addLocationBtn = document.querySelector('.add-location-btn');
    let locationsViewModel = [];

    if (typeof existingLocations !== 'undefined') {
        locationsViewModel = existingLocations.map(location => ({
            roomId: location.roomId,
            shelfId: location.shelfId || '0',
            quantity: location.quantity
        }));

        document.querySelectorAll('.location-summary').forEach(summary => summary.remove());

        locationsViewModel.forEach((location, index) => {
            const locationSummary = createLocationSummary(location.roomId, location.shelfId || '0', location.quantity, index);
            locationFormsDiv.appendChild(locationSummary);
        });
    }

    roomDropdown.addEventListener('change', function () {
        const roomId = this.value;
        shelfDropdown.innerHTML = '<option value="">Shelf</option>';
        if (roomId && serializedShelves[roomId]) {
            serializedShelves[roomId].forEach(shelf => {
                const option = document.createElement('option');
                option.value = shelf;
                option.textContent = shelf;
                shelfDropdown.appendChild(option);
            });
        }
    });

    addLocationBtn.addEventListener('click', function () {
        const locationForm = addLocationBtn.closest('.post-form-wrapper');
        const roomId = locationForm.querySelector('#roomDropdown').value.trim();
        const quantity = parseInt(locationForm.querySelector('.quantity').value.trim(), 10);
        const shelfIdElement = locationForm.querySelector('#shelfDropdown');
        const shelfId = shelfIdElement && shelfIdElement.value.trim() !== '' ? shelfIdElement.value.trim() : "0";

        if (roomId === '' || quantity === '') {
            alert('Please fill in all the fields with valid values.');
            return;
        }

        // check if the location already exists
        const existingLocationIndex = locationsViewModel.findIndex(location =>
            location.roomId === roomId && location.shelfId === shelfId
        );

        if (existingLocationIndex !== -1) { // if it does, increase the quantity
            locationsViewModel[existingLocationIndex].quantity += quantity;
            updateLocationSummary(existingLocationIndex);
        } else {
            addNewLocation(roomId, shelfId, quantity);
        }

        locationForm.querySelector('#roomDropdown').value = '';
        locationForm.querySelector('#shelfDropdown').innerHTML = '<option value="">Shelf</option>';
        locationForm.querySelector('.quantity').value = '';
    });

    function addNewLocation(roomId, shelfId, quantity) {
        const locationSummary = createLocationSummary(roomId, shelfId, quantity, locationsViewModel.length);
        locationFormsDiv.appendChild(locationSummary);
        locationsViewModel.push({ roomId, shelfId, quantity });
    }

    function createLocationSummary(roomId, shelfId, quantity, index) {
        const locationSummary = document.createElement('div');
        locationSummary.classList.add('location-summary');
        locationSummary.dataset.index = index;

        const contentSpan = document.createElement('span');
        updateLocationSummaryContent(contentSpan, roomId, shelfId, quantity);
        locationSummary.appendChild(contentSpan);

        const removeButton = document.createElement('button');
        removeButton.textContent = "✖";
        removeButton.addEventListener('click', function () {
            locationSummary.remove();
            removeLocationFromViewModel(roomId, shelfId);
        });
        locationSummary.appendChild(removeButton);

        return locationSummary;
    }

    function updateLocationSummary(index) {
        const location = locationsViewModel[index];
        const locationSummary = locationFormsDiv.querySelector(`.location-summary[data-index="${index}"]`);
        if (locationSummary) {
            const contentSpan = locationSummary.querySelector('span');
            updateLocationSummaryContent(contentSpan, location.roomId, location.shelfId, location.quantity);
        }
    }

    function updateLocationSummaryContent(element, roomId, shelfId, quantity) {
        element.textContent = `Room ID: ${roomId}, Shelf ID: ${shelfId || '0'}, Quantity: ${quantity}`;
    }

    function removeLocationFromViewModel(roomId, shelfId) {
        locationsViewModel = locationsViewModel.filter(location =>
            !(location.roomId === roomId && location.shelfId === shelfId)
        );

        document.querySelectorAll('.location-summary').forEach((summary, index) => {
            summary.dataset.index = index;
        });
    }

    const createBookForm = document.getElementById('createBookForm');
    const editBookForm = document.getElementById('editBookForm');

    if (createBookForm) {
        createBookForm.addEventListener('submit', handleFormSubmission);
    }

    if (editBookForm) {
        editBookForm.addEventListener('submit', handleFormSubmission);
    }

    function handleFormSubmission(event) {
        const form = this;
        locationsViewModel.forEach((location, index) => {
            const roomIdInput = document.createElement('input');
            roomIdInput.type = 'hidden';
            roomIdInput.name = `Locations[${index}].RoomId`;
            roomIdInput.value = location.roomId;
            form.appendChild(roomIdInput);

            const shelfIdInput = document.createElement('input');
            shelfIdInput.type = 'hidden';
            shelfIdInput.name = `Locations[${index}].ShelfId`;
            shelfIdInput.value = location.shelfId || '';
            form.appendChild(shelfIdInput);

            const quantityInput = document.createElement('input');
            quantityInput.type = 'hidden';
            quantityInput.name = `Locations[${index}].Quantity`;
            quantityInput.value = location.quantity;
            form.appendChild(quantityInput);
        });
    }
});