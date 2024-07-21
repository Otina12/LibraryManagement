$(document).ready(function () {
    const table = $('#reservationTable');
    const checkoutButtonContainer = $('#checkoutButtonContainer');
    const checkoutButton = $('#checkoutButton');

    const reservationId = table.data('reservation-id');

    table.on('change', 'select', function () {
        const hasChanges = table.find('select').filter(function () {
            return $(this).val() !== '';
        }).length > 0;
        checkoutButtonContainer.toggle(hasChanges);
    });

    checkoutButton.on('click', function () {
        const reservationCheckoutDto = {
            reservationId: reservationId,
            reservationCopyCheckouts: []
        };

        table.find('tr').each(function () {
            const row = $(this);
            const reservationCopyId = row.data('reservation-copy-id');
            const bookCopyId = row.find('td:first').text().trim();
            const newStatus = row.find('select').val();

            if (newStatus && newStatus !== '') {
                reservationCheckoutDto.reservationCopyCheckouts.push({
                    reservationCopyId: reservationCopyId,
                    bookCopyId: bookCopyId,
                    newStatus: parseInt(newStatus)
                });
            }
        });

        if (reservationCheckoutDto.reservationCopyCheckouts.length === 0) {
            alert('Please select a status for at least one book copy.');
            return;
        }

        console.log('Sending data:', JSON.stringify(reservationCheckoutDto));

        $.ajax({
            url: '/Reservation/Checkout',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(reservationCheckoutDto),
            success: function (response) {
                console.log('Checkout successful:', response);
                alert('Checkout completed successfully!');
                location.reload();
            },
            error: function (xhr, status, error) {
                console.error('Checkout failed:', error);
                alert('An error occurred during checkout. Please try again.');
            }
        });
    });
});
