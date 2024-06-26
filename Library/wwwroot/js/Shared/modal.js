function initializeModal(entityType, createUrl, editUrl) {
    $(document).ready(function () {
        $(`#add-${entityType}-btn`).click(function (e) {
            e.preventDefault();
            loadModalContent(createUrl, `Add ${entityType.charAt(0).toUpperCase() + entityType.slice(1)}`, true);
        });

        $(`.btn-edit`).click(function (e) {
            e.stopPropagation();
            e.preventDefault();
            var entityId = $(this).data(`${entityType}-id`);
            loadModalContent(`${editUrl}${entityId}`, `Edit ${entityType.charAt(0).toUpperCase() + entityType.slice(1)}`, false);
        });

        $(document).on('submit', `#create${entityType.charAt(0).toUpperCase() + entityType.slice(1)}Form`, function (e) {
            e.preventDefault();
            submitForm($(this), createUrl);
        });

        $(document).on('submit', `#edit${entityType.charAt(0).toUpperCase() + entityType.slice(1)}Form`, function (e) {
            e.preventDefault();
            var entityId = $(this).data(`${entityType}-id`);
            submitForm($(this), `${editUrl}${entityId}`);
        });

        $(document).on('click', '.custom-close', function () {
            $(`#${entityType}Modal`).fadeOut();
        });
    });
}

function loadModalContent(url, title, isCreate) {
    $.ajax({
        url: url,
        type: 'GET',
        success: function (data) {
            $('#modalBody').html(data);
            $('#modalTitle').text(title);
            $('#add-button').toggle(isCreate);
            $('#edit-button').toggle(!isCreate);
            $('#publisherModal, #authorModal').fadeIn();
        },
        error: function () {
            alert('Error loading the form');
        }
    });
}

function submitForm(form, url) {
    $.ajax({
        url: url,
        type: form.attr('method'),
        data: form.serialize(),
        success: function (response) {
            if (response.success) {
                $('#publisherModal, #authorModal').fadeOut();
                location.reload();
            } else {
                $('#publisherModal, #authorModal').fadeOut();
                location.reload();
            }
        },
        error: function () {
            alert('Error saving/updating the entity');
        }
    });
}