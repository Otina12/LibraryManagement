$(document).ready(function () {
    $('#modelName').on('change', function () {
        var modelName = $(this).val();
        var reportTypeContainer = $('#reportTypeContainer');
        var reportTypeSelect = $('#reportType');

        reportTypeSelect.val('');
        $('#dateInputs').hide();
        $('#yearInput').hide();

        if (modelName) {
            reportTypeContainer.show();

            if (modelName === 'Book' || modelName == 'Customer') {
                if ($('#reportType option[value="BooksDamaged"]').length === 0) {
                    reportTypeSelect.append('<option value="BooksDamaged">Books Damaged</option>');
                }
            } else {
                $('#reportType option[value="BooksDamaged"]').remove();
            }
        } else {
            reportTypeContainer.hide();
        }
    });

    $('#reportType').on('change', function () {
        var reportType = $(this).val();
        var dateInputs = $('#dateInputs');
        var yearInput = $('#yearInput');

        if (reportType === 'Popularity') {
            dateInputs.show();
            yearInput.hide();
        } else if (reportType === 'Annual') {
            dateInputs.hide();
            yearInput.show();
        } else {
            dateInputs.show();
            yearInput.hide();
        }
    });
});
