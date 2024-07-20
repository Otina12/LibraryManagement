$(document).ready(function () {
    const table = $('#reservationTable');
    const checkoutButtonContainer = $('#checkoutButtonContainer');
    const checkoutButton = $('#checkoutButton');

    table.on('change', 'select', function () {
        const hasChanges = table.find('select').filter(function () {
            return $(this).val() !== '';
        }).length > 0;

        checkoutButtonContainer.toggle(hasChanges);
    });

    checkoutButton.on('click', function () {
        const reservationCheckoutDto = {
            reservationId: '@Model.Id',
            reservationCopyCheckouts: []
        };

        table.find('tr').each(function () {
            const row = $(this);
            const bookCopyId = row.find('td:first').text().trim();
            const reservationCopyId = row.data('reservation-copy-id');
            const newStatus = row.find('select').val() || null;

            if (newStatus) {
                reservationCheckoutDto.reservationCopyCheckouts.push({
                    reservationCopyId: reservationCopyId,
                    bookCopyId: bookCopyId,
                    newStatus: newStatus
                });
            }
        });

        if (reservationCheckoutDto.reservationCopyCheckouts.length === 0) {
            return;
        }

        $('#reservationCheckoutData').val(JSON.stringify(reservationCheckoutDto));

        $('#checkoutForm').submit();
    });
});
