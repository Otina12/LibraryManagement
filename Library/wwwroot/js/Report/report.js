$(document).ready(function () {
    $('#modelName').on('change', function () {
        var modelName = $(this).val();
        var reportTypeContainer = $('#reportTypeContainer');

        if (modelName) {
            reportTypeContainer.show();
        } else {
            reportTypeContainer.hide();
            $('#dateInputs').hide();
            $('#yearInput').hide();
            $('#reportType').val('');
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
            dateInputs.hide();
            yearInput.hide();
        }
    });
});
