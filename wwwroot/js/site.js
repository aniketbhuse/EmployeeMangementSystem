$(document).ready(function () {
    $('#submitBtn').click(function (e) {
        e.preventDefault();
        // Collect form values
        var employeeData = {
            F_Name: $('#EmployeeName').val(),
            L_Name: $('#L_Name').val(),
            Phone_Number: $('#Phone_Number').val(),
            Address: $('#Address').val(), 
            RoleId: $('#RoleId').val(),
            DepartmentId: $('#DepartmentId').val(),
            JoinDate: $('#JoiningDate').val()

        };
        // Send to controller
        $.ajax({
            type: "POST",
            url: "/Home/CreateEmployee",  // Adjust this if your controller name is different
            data: JSON.stringify(employeeData),
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                console.log("Success:", response);
                alert("Employee created successfully!");
                $('#EmployeeForm')[0].reset();
            },
            error: function (xhr, status, error) {
                console.error("Error:", error);
                alert("Something went wrong!");
            }
        });
    });
});

$(document).ready(function () {
    loadEmployees();
    function loadEmployees() {
        $.ajax({
            url: '/Home/GetEmployees',
            type: 'GET',
            success: function (data) {
                var rows = '';
                $.each(data, function (i, employee) {
                    rows += '<tr>'
                        + '<td>' + employee.empId + '</td>'
                        + '<td>' + employee.f_Name + '</td>'
                        + '<td>' + employee.l_Name + '</td>'
                        + '<td>' + employee.joinDate.substring(0, 10) + '</td>'
                        + '<td>' + employee.phone_Number + '</td>'
                        + '<td>' + employee.address + '</td>'
                        + '<td>' + employee.roleName + '</td>'         // ✅ Role Type
                        + '<td>' + employee.departmentName + '</td>'   // ✅ Department Name
                        + '<td>'
                        + '<button class="action-btn edit-btn" onclick="editEmployee(' + employee.empId + ')">Edit</button>'
                        + '<button class="action-btn delete-btn" onclick="deleteEmployee(' + employee.empId + ')">Delete</button>'
                        + '</td>'
                        + '</tr>';
                });
                $('#employeeTable tbody').html(rows);
            },
            error: function () {
                alert('Failed to fetch employee data.');
            }
        });
    }
});





// Delete function
function deleteEmployee(empId) {
    if (confirm("Are you sure you want to delete this employee?")) {
        $.ajax({
            url: '/Home/DeleteEmployee/' + empId,
            type: 'POST',
            success: function (response) {
                if (response.success) {
                    alert("Employee deleted successfully!");
                    location.reload(); // reload table
                } else {
                    alert("Failed to delete employee.");
                }
            },
            error: function () {
                alert("Error deleting employee.");
            }
        });
    }
}
debugger;
function editEmployee(empId) {
    $.ajax({
        url: '/Home/EditEmployee',
        type: 'GET',
        data: { id: empId },
        success: function (data) {
            // Load partial into placeholder div
            $('#editEmployeeModal').html(data);

            // Show Bootstrap modal
            $('#editEmployeeModalContent').modal('show');

            // Attach submit event
            $('#editEmployeeForm').on('submit', function (e) {
                e.preventDefault();
                debugger;
                $.ajax({
                    url: '/Home/UpdateEmployee',
                    type: 'POST',
                    data: $(this).serialize(),
                    success: function (result) {
                        if (result.success) {
                            alert('Employee updated successfully');
                            $('#editEmployeeModalContent').modal('hide');
                            loadEmployees(); // refresh table
                        } else {
                            alert(result.message || 'Update failed');
                        }
                    },
                    error: function () {
                        alert('Error while updating employee.');
                    }
                });
            });
        },
        error: function () {
            alert('Failed to load edit form.');
        }
    });
}


