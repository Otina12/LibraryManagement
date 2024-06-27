document.addEventListener('DOMContentLoaded', function () {
    const roomDropdown = document.getElementById('roomDropdown');
    const shelfDropdown = document.getElementById('shelfDropdown');
    const locationFormsDiv = document.getElementById('locationForms');
    const addLocationBtn = document.querySelector('.add-location-btn');
    let locationsViewModel = [];

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
        const locationForm = addLocationBtn.closest('.location-form-wrapper');
        const roomId = locationForm.querySelector('#roomDropdown').value.trim();
        const quantity = locationForm.querySelector('.quantity').value.trim();
        const shelfIdElement = locationForm.querySelector('#shelfDropdown');
        const shelfId = shelfIdElement && shelfIdElement.value.trim() !== '' ? shelfIdElement.value.trim() : "0";

        if (roomId === '' || quantity === '') {
            alert('Please fill in all the fields.');
            return;
        }

        const locationSummary = document.createElement('div');
        locationSummary.classList.add('location-summary');
        locationSummary.innerHTML = `Room ID: ${roomId}, Shelf ID: ${shelfId || '0'}, Quantity: ${quantity}`;

        const removeButton = document.createElement('button');
        removeButton.textContent = "✖";
        removeButton.addEventListener('click', function () {
            locationSummary.remove();
            removeLocationFromViewModel(roomId, shelfId, quantity);
        });

        locationSummary.appendChild(removeButton);
        locationFormsDiv.appendChild(locationSummary);

        locationsViewModel.push({ roomId, shelfId, quantity });

        locationForm.querySelector('#roomDropdown').value = '';
        locationForm.querySelector('#shelfDropdown').innerHTML = '<option value="">Shelf</option>';
        locationForm.querySelector('.quantity').value = '';
    });

    function removeLocationFromViewModel(roomId, shelfId, quantity) {
        locationsViewModel = locationsViewModel.filter(location =>
            !(location.roomId === roomId &&
                location.shelfId === shelfId &&
                location.quantity === quantity)
        );
    }

    document.getElementById('createBookForm').addEventListener('submit', function (event) {
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
    });
});