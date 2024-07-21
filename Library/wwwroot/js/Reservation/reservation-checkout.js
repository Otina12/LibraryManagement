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

        const hasSelectedStatus = form.find('select[name^="ReservationCopyCheckouts"]').filter(function () {
            return $(this).val() !== '';
        }).length > 0;

        if (!hasSelectedStatus) {
            alert('Please select a status for at least one book copy.');
            return;
        }

        form.find('select[name^="ReservationCopyCheckouts"]').each(function () {
            if ($(this).val() === '') {
                $(this).prop('disabled', true);
                $(this).siblings('input[type="hidden"]').prop('disabled', true);
            }
        });

        this.submit();

        setTimeout(function () {
            form.find('select[name^="ReservationCopyCheckouts"], input[type="hidden"]').prop('disabled', false);
        }, 100);
    });
});