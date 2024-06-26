function dismissEmployee(employeeId, deleteLink) {
    var deleteUrl = deleteLink;

    if (confirm("Are you sure you want to dismiss this employee?")) {
        const form = document.createElement('form');
        form.method = 'POST';
        form.action = deleteUrl;

        const input = document.createElement('input');
        input.type = 'hidden';
        input.name = 'id';
        input.value = employeeId;

        form.appendChild(input);
        document.body.appendChild(form);
        form.submit();
    }
}

function renewEmployee(employeeId, renewLink) {
    var renewUrl = renewLink;

    if (confirm("Are you sure you want to renew this employee?")) {
        const form = document.createElement('form');
        form.method = 'POST';
        form.action = renewUrl;

        const input = document.createElement('input');
        input.type = 'hidden';
        input.name = 'id';
        input.value = employeeId;

        form.appendChild(input);
        document.body.appendChild(form);
        form.submit();
    }
}

document.addEventListener("DOMContentLoaded", function () {
    const roleSelect = document.getElementById("roles");
    const roleList = document.getElementById("roleList");
    const saveChangesButton = document.getElementById("saveChangesButton");
    let changesMade = false;

    roleSelect.addEventListener("change", function () {
        const selectedRole = roleSelect.value;
        if (selectedRole) {
            const roleItems = roleList.getElementsByTagName("li");
            let isPresent = false;
            for (let i = 0; i < roleItems.length; i++) {
                if (roleItems[i].textContent.includes(selectedRole)) {
                    isPresent = true;
                    break;
                }
            }

            if (!isPresent) {
                const newRoleItem = document.createElement("li");
                newRoleItem.className = "role-tag";

                const roleText = document.createTextNode(selectedRole);
                newRoleItem.appendChild(roleText);

                const removeButton = document.createElement("button");
                removeButton.className = "remove-role";
                removeButton.textContent = "✖";
                removeButton.onclick = function () {
                    removeRole(this);
                };

                newRoleItem.appendChild(removeButton);
                roleList.appendChild(newRoleItem);
                showSaveButton();
            }
            roleSelect.selectedIndex = 0;
        }
    });

    window.removeRole = function (button) {
        const roleItem = button.parentNode;
        roleList.removeChild(roleItem);
        showSaveButton();
    }

    function showSaveButton() {
        if (!changesMade) {
            saveChangesButton.style.display = "block";
            changesMade = true;
        }
    }
});

function changeRoles(employeeId, callbackLink) {
    const form = document.createElement("form");
    form.method = "post";
    form.action = callbackLink;

    const employeeIdInput = document.createElement("input");
    employeeIdInput.type = "hidden";
    employeeIdInput.name = "employeeId";
    employeeIdInput.value = employeeId;
    form.appendChild(employeeIdInput);

    const updatedRoles = [];
    const roleItems = document.getElementById("roleList").getElementsByTagName("li");
    for (let i = 0; i < roleItems.length; i++) {
        let role = roleItems[i].textContent.trim();
        role = role.replace(/✖/g, "").trim();
        updatedRoles.push(role);
    }

    updatedRoles.forEach((role, index) => {
        const roleInput = document.createElement("input");
        roleInput.type = "hidden";
        roleInput.name = `roles[${index}]`;
        roleInput.value = role;
        form.appendChild(roleInput);
    });

    document.body.appendChild(form);
    form.submit();
}