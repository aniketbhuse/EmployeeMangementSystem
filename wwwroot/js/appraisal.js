


//Aprisal Section 
debugger;
$(document).ready(function () {
    function loadTable() {
        debugger;
        $.getJSON('/EmployeeApproval/GetAll', function (data) {
            var tbody = $("#appraisalTable tbody");
            tbody.empty();
            debugger;
            $.each(data, function (index, item) {
                var row = "<tr>" +
                    "<td>" + item.AppraisalId + "</td>" +
                    "<td>" + item.F_Name + "</td>" +
                    "<td>" + item.L_Name + "</td>" +
                    "<td>" + item.OldRole + "</td>" +
                    "<td>" + item.NewRole + "</td>" +
                    "<td>" +
                    "<button class='btn btn-sm btn-primary edit-btn' data-id='" + item.AppraisalId + "'>Edit</button> " +
                    "<button class='btn btn-sm btn-danger delete-btn' data-id='" + item.AppraisalId + "'>Delete</button>" +
                    "</td>" +
                    "</tr>";
                tbody.append(row);
            });
        });
    }

    loadTable(); // initial load
    debugger;
    // Edit button click
    $(document).on('click', '.edit-btn', function () {
        var id = $(this).data('id');
        // Redirect to Edit page (or open modal)
        window.location.href = '/EmployeeApproval/Edit/' + id;
    });
    debugger;
    // Delete button click
    $(document).on('click', '.delete-btn', function () {
        var id = $(this).data('id');
        if (confirm("Are you sure you want to delete this appraisal?")) {
            $.ajax({
                url: '/EmployeeApproval/Delete/' + id,
                type: 'POST',
                success: function (result) {
                    alert("Deleted successfully!");
                    loadTable(); // reload table
                },
                error: function () {
                    alert("Error deleting record.");
                }
            });
        }
    });
});
