function dismissEmployee(employeeId, deleteLink) {
    var deleteUrl = deleteLink;

    if (confirm("Are you sure you want to dismiss this employee?")) {
        const form = $('<form>', { method: 'POST', action: deleteUrl });
        const input = $('<input>', { type: 'hidden', name: 'id', value: employeeId });

        form.append(input);
        $('body').append(form);
        form.submit();
    }
}

function renewEmployee(employeeId, renewLink) {
    var renewUrl = renewLink;

    if (confirm("Are you sure you want to renew this employee?")) {
        const form = $('<form>', { method: 'POST', action: renewUrl });
        const input = $('<input>', { type: 'hidden', name: 'id', value: employeeId });

        form.append(input);
        $('body').append(form);
        form.submit();
    }
}

$(document).ready(function () {
    const roleSelect = $("#roles");
    const roleList = $("#roleList");
    const saveChangesButton = $("#saveChangesButton");
    let changesMade = false;

    roleSelect.change(function () {
        const selectedRole = roleSelect.val();
        if (selectedRole) {
            let isPresent = false;
            roleList.find("li").each(function () {
                if ($(this).text().includes(selectedRole)) {
                    isPresent = true;
                    return false;
                }
            });

            if (!isPresent) {
                const newRoleItem = $("<li>", { class: "role-tag" });
                newRoleItem.text(selectedRole);

                const removeButton = $("<button>", { class: "remove-role", text: "✖" }).click(function () {
                    removeRole(this);
                });

                newRoleItem.append(removeButton);
                roleList.append(newRoleItem);
                showSaveButton();
            }
            roleSelect.prop('selectedIndex', 0);
        }
    });

    window.removeRole = function (button) {
        $(button).parent().remove();
        showSaveButton();
    };

    function showSaveButton() {
        if (!changesMade) {
            saveChangesButton.show();
            changesMade = true;
        }
    }
});

function changeRoles(employeeId, callbackLink) {
    const form = $('<form>', { method: 'post', action: callbackLink });

    const employeeIdInput = $('<input>', { type: 'hidden', name: 'employeeId', value: employeeId });
    form.append(employeeIdInput);

    const updatedRoles = [];
    $('#roleList li').each(function () {
        let role = $(this).text().trim().replace(/✖/g, "").trim();
        updatedRoles.push(role);
    });

    $.each(updatedRoles, function (index, role) {
        const roleInput = $('<input>', { type: 'hidden', name: `roles[${index}]`, value: role });
        form.append(roleInput);
    });

    $('body').append(form);
    form.submit();
}
