function initializeModal(entityType, createUrl, editUrl, deleteUrl) {
    $(document).ready(function () {
        $(`#add-${entityType}-btn`).click(function (e) {
            e.preventDefault();
            loadModalContent(createUrl, `Add ${entityType.charAt(0).toUpperCase() + entityType.slice(1)}`, 'create');
        });
        $(`.btn-edit`).click(function (e) {
            e.stopPropagation();
            e.preventDefault();
            var entityId = $(this).data(`${entityType}-id`);
            loadModalContent(`${editUrl}${entityId}`, `Edit ${entityType.charAt(0).toUpperCase() + entityType.slice(1)}`, 'edit');
        });
        $(`.btn-delete`).click(function (e) {
            e.stopPropagation();
            e.preventDefault();
            var entityId = $(this).data(`${entityType}-id`);
            showDeleteConfirmation(entityId, entityType, deleteUrl);
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
        $(document).on('click', '#confirm-delete-button', function () {
            var entityId = $(this).data('entity-id');
            deleteEntity(entityId, deleteUrl);
        });
    });
}

function loadModalContent(url, title, action) {
    $.ajax({
        url: url,
        type: 'GET',
        success: function (data) {
            $('#modalBody').html(data);
            $('#modalTitle').text(title);
            $('#add-button').toggle(action === 'create');
            $('#edit-button').toggle(action === 'edit');
            $('#confirm-delete-button').hide();
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

function showDeleteConfirmation(entityId, entityType, deleteUrl) {
    $('#modalTitle').text(`Delete ${entityType.charAt(0).toUpperCase() + entityType.slice(1)}`);
    $('#modalBody').html(`<p>Are you sure you want to delete this ${entityType}?</p>`);
    $('#add-button, #edit-button').hide();
    $('#confirm-delete-button').show();
    $('#confirm-delete-button').data('entity-id', entityId);
    $('#confirm-delete-button').data('delete-url', deleteUrl);
    $(`#${entityType}Modal`).fadeIn();
}

function deleteEntity(entityId, deleteUrl) {
    $.ajax({
        url: deleteUrl,
        type: 'POST',
        data: { id: entityId },
        success: function (response) {
            if (response.success) {
                $('#publisherModal, #authorModal').fadeOut();
                location.reload();
            } else {
                alert('Error deleting the entity: ' + response.message);
            }
        },
        error: function (xhr, status, error) {
            alert('Error deleting the entity: ' + error);
        }
    });
}