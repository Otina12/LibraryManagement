$(document).ready(function () {
    const form = $('#checkoutForm');
    const checkoutButtonContainer = $('#checkoutButtonContainer');

    form.on('change', 'select', function () {
        const hasChanges = form.find('select').filter(function () {
            return $(this).val() !== '';
        }).length > 0;
        checkoutButtonContainer.toggle(hasChanges);
    });

    form.on('submit', function (e) {
        e.preventDefault();

        const hasSelectedStatus = form.find('select').filter(function () {
            return $(this).val() !== '';
        }).length > 0;

        if (!hasSelectedStatus) {
            alert('Please select a status for at least one book copy.');
            return;
        }

        form.find('select').each(function () {
            if ($(this).val() === '') {
                $(this).prop('disabled', true);
                $(this).siblings('input[type="hidden"]').prop('disabled', true);
            }
        });

        this.submit();
    });
});