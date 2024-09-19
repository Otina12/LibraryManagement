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

    $('.generate-pdf-report-btn').click(function () {
        var startDate = $('#pdf-startDate').val();
        var endDate = $('#pdf-endDate').val();

        if (!startDate || !endDate) {
            alert('Please select both start and end dates.');
            return;
        }

        $.ajax({
            url: '/Report/ExportPdfGeneralPopularityReport',
            type: 'GET',
            data: {
                startDate: startDate,
                endDate: endDate
            },
            xhrFields: {
                responseType: 'blob'
            },
            success: function (data) {
                var downloadUrl = URL.createObjectURL(data);
                var a = document.createElement('a');
                a.href = downloadUrl;
                a.download = `CombinedReport_${startDate.replace(/-/g, '')}_${endDate.replace(/-/g, '')}.pdf`;
                document.body.appendChild(a);
                a.click();
                URL.revokeObjectURL(downloadUrl);
            },
            error: function () {
                alert('Could not generate the PDF report. Please try again.');
            }
        });
    });
});
