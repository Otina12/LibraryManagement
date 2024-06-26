document.addEventListener('DOMContentLoaded', function () {
    const roomDropdown = document.getElementById('roomDropdown');
    const shelfDropdown = document.getElementById('shelfDropdown');

    const authorSelectList = document.getElementById('authorSelectList');
    const selectedAuthorsDiv = document.getElementById('selectedAuthors');

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
        locationsViewModel = locationsViewModel.filter(location => !(location.roomId === roomId && location.shelfId === shelfId && location.quantity === quantity));
    }

    document.getElementById('createBookForm').addEventListener('submit', function (event) {
        const form = $(this);
        const formData = form.serializeArray();

        formData.forEach((field, index) => {
            if (field.name === 'SelectedAuthorIds') {
                formData.splice(index, 1);
            }
        });

        console.log('Before adding locations:', formData);

        locationsViewModel.forEach((location, index) => {
            formData.push({ name: `Locations[${index}].RoomId`, value: location.roomId });
            formData.push({ name: `Locations[${index}].ShelfId`, value: location.shelfId });
            formData.push({ name: `Locations[${index}].Quantity`, value: location.quantity });
        });

        console.log('After adding locations:', formData);

        const selectedAuthors = [];
        document.querySelectorAll('.selected-author').forEach(authorDiv => {
            selectedAuthors.push(authorDiv.dataset.value);
        });

        selectedAuthors.forEach((authorId, index) => {
            formData.push({ name: `SelectedAuthorIds[${index}]`, value: authorId });
        });

        console.log('Final formData:', formData);
    });

    authorSelectList.addEventListener('change', function () {
        const selectedOption = authorSelectList.options[authorSelectList.selectedIndex];
        if (selectedOption.value) {
            const existingAuthor = selectedAuthorsDiv.querySelector(`.selected-author[data-value="${selectedOption.value}"]`);
            if (!existingAuthor) {
                const authorDiv = document.createElement('div');
                authorDiv.textContent = selectedOption.text;
                authorDiv.classList.add('selected-author');
                authorDiv.dataset.value = selectedOption.value;
                const removeButton = document.createElement('button');
                removeButton.textContent = "✖";
                removeButton.addEventListener('click', function () {
                    authorDiv.remove();
                });
                authorDiv.appendChild(removeButton);
                selectedAuthorsDiv.appendChild(authorDiv);
            }
        }
    });
});
