function initializeModal(entityType, createUrl, editUrl, deleteUrl, renewUrl) {
    $(document).ready(function () {
        $(`#add-${entityType}-btn`).click(function (e) {
            e.preventDefault();
            loadModalContent(createUrl, `Add ${entityType.charAt(0).toUpperCase() + entityType.slice(1)}`, 'create');
        });
        $(`.btn-edit`).click(function (e) {
            e.stopPropagation();
            e.preventDefault();
            var entityId = $(this).data(`entity-id`);
            loadModalContent(`${editUrl}${entityId}`, `Edit ${entityType.charAt(0).toUpperCase() + entityType.slice(1)}`, 'edit');
        });
        $(`.btn-delete`).click(function (e) {
            e.stopPropagation();
            e.preventDefault();
            var entityId = $(this).data(`entity-id`);
            showDeleteConfirmation(entityId, entityType, deleteUrl);
        });
        $(`.btn-renew`).click(function (e) {
            e.stopPropagation();
            e.preventDefault();
            var entityId = $(this).data(`entity-id`);
            showRenewConfirmation(entityId, entityType, renewUrl);
        });
        $(document).on('submit', `#create${entityType.charAt(0).toUpperCase() + entityType.slice(1)}Form`, function (e) {
            e.preventDefault();
            if (entityType != 'bookCopy') { // custom submission handling is implemented for book copies in book-post.js
                submitForm($(this), createUrl);
            }
        });
        $(document).on('submit', `#edit${entityType.charAt(0).toUpperCase() + entityType.slice(1)}Form`, function (e) {
            e.preventDefault();
            var entityId = $(this).data(`entity-id`);
            submitForm($(this), `${editUrl}${entityId}`);
        });
        $(document).on('click', '.custom-close', function () {
            $(`#${entityType}Modal`).fadeOut();
        });
        $(document).on('click', '#confirm-delete-button', function () {
            console.log('entered delete')
            var entityId = $(this).data('entity-id');
            deleteEntity(entityId, deleteUrl);
        });
        $(document).on('click', '#confirm-renew-button', function () {
            console.log('entered renew')
            var entityId = $(this).data('entity-id');
            renewEntity(entityId, renewUrl);
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
            $('#confirm-renew-button').hide();
            $('#publisherModal, #authorModal, #customerModal, #originalBookModal, #bookCopyModal').fadeIn();
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
                $('#publisherModal, #authorModal, #customerModal, #originalBookModal').fadeOut();
                location.reload();
            } else if (typeof response === 'object' && response.message) {
                alert('Error: ' + response.message);
            } else {
                $('#modalBody').html(response);
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
    $('#confirm-delete-button').show().data('entity-id', entityId).data('delete-url', deleteUrl);
    $(`#${entityType}Modal`).fadeIn();
}

function showRenewConfirmation(entityId, entityType, renewUrl) {
    $('#modalTitle').text(`Renew ${entityType.charAt(0).toUpperCase() + entityType.slice(1)}`);
    $('#modalBody').html(`<p>Are you sure you want to renew this ${entityType}?</p>`);
    $('#add-button, #edit-button').hide();
    $('#confirm-renew-button').show().data('entity-id', entityId).data('renew-url', renewUrl);
    $(`#${entityType}Modal`).fadeIn();
}

function deleteEntity(entityId, deleteUrl) {
    $.ajax({
        url: deleteUrl,
        type: 'POST',
        data: { id: entityId },
        success: function (response) {
            if (response.success) {
                $('#publisherModal, #authorModal, #customerModal, #originalBookModal, #bookCopyModal').fadeOut();
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

function renewEntity(entityId, renewUrl) {
    $.ajax({
        url: renewUrl,
        type: 'POST',
        data: { id: entityId },
        success: function (response) {
            if (response.success) {
                $('#publisherModal, #authorModal, #customerModal, #originalBookModal, #bookCopyModal').fadeOut();
                location.reload();
            } else {
                alert('Error renewing the entity: ' + response.message);
            }
        },
        error: function (xhr, status, error) {
            alert('Error renewing the entity: ' + error);
        }
    });
}