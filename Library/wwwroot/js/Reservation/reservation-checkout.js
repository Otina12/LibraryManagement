$(document).ready(function () {
    const form = $('#checkoutForm');
    const checkoutButtonContainer = $('#checkoutButtonContainer');
    const setAllStatusDropdown = $('#setAllStatus');

    function updateCheckoutButtonVisibility() {
        const hasChanges = form.find('select[name^="ReservationCopyCheckouts"]').filter(function () {
            return $(this).val() !== '';
        }).length > 0;
        checkoutButtonContainer.toggle(hasChanges);
    }

    form.on('change', 'select[name^="ReservationCopyCheckouts"]', updateCheckoutButtonVisibility);

    setAllStatusDropdown.on('change', function () {
        const selectedStatus = $(this).val();
        form.find('select[name^="ReservationCopyCheckouts"]').val(selectedStatus);
        updateCheckoutButtonVisibility();
    });

    form.on('submit', function (e) {
        e.preventDefault();

        const selectedDropdowns = form.find('select[name^="ReservationCopyCheckouts"]').filter(function () {
            return $(this).val() !== '';
        });

        if (selectedDropdowns.length === 0) {
            alert('Please select a status for at least one book copy.');
            return;
        }

        const formData = new FormData();
        const reservationId = $('input[name="ReservationId"]').val();
        formData.append('ReservationId', reservationId);

        selectedDropdowns.each(function (index) {
            const select = $(this);
            const hiddenInputs = select.siblings('input[type="hidden"]');
            const reservationCopyId = hiddenInputs.filter('[name$=".ReservationCopyId"]').val();
            const bookCopyId = hiddenInputs.filter('[name$=".BookCopyId"]').val();
            const newStatus = select.val();

            formData.append(`ReservationCopyCheckouts[${index}].ReservationCopyId`, reservationCopyId);
            formData.append(`ReservationCopyCheckouts[${index}].BookCopyId`, bookCopyId);
            formData.append(`ReservationCopyCheckouts[${index}].NewStatus`, newStatus);
        });

        $.ajax({
            url: form.attr('action'),
            method: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.success) {
                    location.reload();
                } else {
                    alert(response.message);
                }
            },
            error: function (xhr, status, error) {
                console.error('Error submitting form:', error);
                alert('An error occurred while submitting the form. Please try again.');
            }
        });
    });
});
